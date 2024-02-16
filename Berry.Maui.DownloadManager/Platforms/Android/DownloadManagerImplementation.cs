using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Berry.Maui.Abstractions;

namespace Berry.Maui;

/// <summary>
/// The android implementation of the download manager.
/// </summary>
public class DownloadManagerImplementation : IDownloadManager
{
    private Java.Lang.Runnable _downloadWatcherHandlerRunnable = default!;

    private readonly Android.App.DownloadManager _downloadManager;

    private readonly IList<IDownloadFile> _queue;

    public IEnumerable<IDownloadFile> Queue
    {
        get
        {
            lock (_queue)
            {
                return _queue.ToList();
            }
        }
    }

    public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanged;

    public Func<IDownloadFile, string>? PathNameForDownloadedFile { get; set; }

    public DownloadVisibility NotificationVisibility;

    public bool IsVisibleInDownloadsUi { get; set; } = true; // true is the default behavior from Android DownloadManagerApi

    public DownloadManagerImplementation()
    {
        _queue = [];

        _downloadManager = (Android.App.DownloadManager)
            Application.Context.GetSystemService(Context.DownloadService)!;

        // Add all items to the Queue that are pending, paused or running
        LoopOnDownloads(new Action<ICursor>(cursor => ReinitializeFile(cursor)));

        // Check sequentially if parameters for any of the registered downloads changed
        StartDownloadWatcher();
    }

    public IDownloadFile CreateDownloadFile(string url)
    {
        return CreateDownloadFile(url, new Dictionary<string, string>());
    }

    public IDownloadFile CreateDownloadFile(string url, IDictionary<string, string> headers)
    {
        return new DownloadFileImplementation(url, headers);
    }

    public void Start(IDownloadFile i, bool mobileNetworkAllowed = true)
    {
        var file = (DownloadFileImplementation)i;

        var destinationPathName = string.Empty;
        if (PathNameForDownloadedFile != null)
        {
            destinationPathName = PathNameForDownloadedFile(file);
        }

        file.StartDownload(
            _downloadManager,
            destinationPathName,
            mobileNetworkAllowed,
            NotificationVisibility,
            IsVisibleInDownloadsUi
        );
        AddFile(file);
    }

    public void Abort(IDownloadFile i)
    {
        var file = (DownloadFileImplementation)i;

        file.Status = DownloadFileStatus.CANCELED;
        _downloadManager.Remove(file.Id);
        RemoveFile(file);
    }

    public void AbortAll()
    {
        foreach (var file in Queue)
        {
            Abort(file);
        }
    }

    void LoopOnDownloads(Action<ICursor> runnable)
    {
        // Reinitialize downloads that were started before the app was terminated or suspended
        var query = new Android.App.DownloadManager.Query();
        query.SetFilterByStatus(
            DownloadStatus.Paused | DownloadStatus.Pending | DownloadStatus.Running
        );

        try
        {
            using var cursor = _downloadManager.InvokeQuery(query);
            while (cursor?.MoveToNext() == true)
            {
                runnable.Invoke(cursor);
            }
            cursor?.Close();
        }
        catch (Android.Database.Sqlite.SQLiteException)
        {
            // I lately got an exception that the database was unaccessible ...
        }
    }

    void ReinitializeFile(ICursor cursor)
    {
        var downloadFile = new DownloadFileImplementation(cursor);

        AddFile(downloadFile);
        UpdateFileProperties(cursor, downloadFile);
    }

    void StartDownloadWatcher()
    {
        // Create an instance for a runnable-handler
        var downloadWatcherHandler = new Handler(Looper.MainLooper!);

        // Create a runnable, restarting itself to update every file in the queue
        _downloadWatcherHandlerRunnable = new Java.Lang.Runnable(() =>
        {
            var downloads = Queue.Cast<DownloadFileImplementation>().ToList();

            foreach (var file in downloads)
            {
                var query = new Android.App.DownloadManager.Query();
                query.SetFilterById(file.Id);

                try
                {
                    using var cursor = _downloadManager.InvokeQuery(query);
                    if (cursor?.MoveToNext() == true)
                    {
                        UpdateFileProperties(cursor, file);
                    }
                    else
                    {
                        // This file is not listed in the native download manager anymore. Let's mark it as canceled.
                        Abort(file);
                    }
                    cursor?.Close();
                }
                catch (Android.Database.Sqlite.SQLiteException)
                {
                    // I lately got an exception that the database was unaccessible ...
                }
            }

            downloadWatcherHandler.PostDelayed(_downloadWatcherHandlerRunnable, 1000);
        });

        // Start this playing handler immediately
        downloadWatcherHandler.PostDelayed(_downloadWatcherHandlerRunnable, 0);
    }

    /**
     * Update the properties for a file by it's cursor.
     * This method should be called in an interval and on reinitialization.
     */
    public void UpdateFileProperties(ICursor cursor, DownloadFileImplementation downloadFile)
    {
        downloadFile.TotalBytesWritten = cursor.GetFloat(
            cursor.GetColumnIndex(Android.App.DownloadManager.ColumnBytesDownloadedSoFar)
        );
        downloadFile.TotalBytesExpected = cursor.GetFloat(
            cursor.GetColumnIndex(Android.App.DownloadManager.ColumnTotalSizeBytes)
        );

        switch (
            (DownloadStatus)
                cursor.GetInt(cursor.GetColumnIndex(Android.App.DownloadManager.ColumnStatus))
        )
        {
            case DownloadStatus.Successful:
                downloadFile.DestinationPathName = cursor.GetString(
                    cursor.GetColumnIndex("local_uri")
                );
                downloadFile.StatusDetails = default!;
                downloadFile.Status = DownloadFileStatus.COMPLETED;
                RemoveFile(downloadFile);
                break;

            case DownloadStatus.Failed:
                var reasonFailed = cursor.GetInt(
                    cursor.GetColumnIndex(Android.App.DownloadManager.ColumnReason)
                );
                if (reasonFailed < 600)
                {
                    downloadFile.StatusDetails = "Error.HttpCode: " + reasonFailed;
                }
                else
                {
                    downloadFile.StatusDetails = (DownloadError)reasonFailed switch
                    {
                        DownloadError.CannotResume => "Error.CannotResume",
                        DownloadError.DeviceNotFound => "Error.DeviceNotFound",
                        DownloadError.FileAlreadyExists => "Error.FileAlreadyExists",
                        DownloadError.FileError => "Error.FileError",
                        DownloadError.HttpDataError => "Error.HttpDataError",
                        DownloadError.InsufficientSpace => "Error.InsufficientSpace",
                        DownloadError.TooManyRedirects => "Error.TooManyRedirects",
                        DownloadError.UnhandledHttpCode => "Error.UnhandledHttpCode",
                        DownloadError.Unknown => "Error.Unknown",
                        _ => "Error.Unregistered: " + reasonFailed,
                    };
                }
                downloadFile.Status = DownloadFileStatus.FAILED;
                RemoveFile(downloadFile);
                break;

            case DownloadStatus.Paused:
                var reasonPaused = cursor.GetInt(
                    cursor.GetColumnIndex(Android.App.DownloadManager.ColumnReason)
                );
                downloadFile.StatusDetails = (DownloadPausedReason)reasonPaused switch
                {
                    DownloadPausedReason.QueuedForWifi => "Paused.QueuedForWifi",
                    DownloadPausedReason.WaitingToRetry => "Paused.WaitingToRetry",
                    DownloadPausedReason.WaitingForNetwork => "Paused.WaitingForNetwork",
                    DownloadPausedReason.Unknown => "Paused.Unknown",
                    _ => "Paused.Unregistered: " + reasonPaused,
                };
                downloadFile.Status = DownloadFileStatus.PAUSED;
                break;

            case DownloadStatus.Pending:
                downloadFile.StatusDetails = default!;
                downloadFile.Status = DownloadFileStatus.PENDING;
                break;

            case DownloadStatus.Running:
                downloadFile.StatusDetails = default!;
                downloadFile.Status = DownloadFileStatus.RUNNING;
                break;
        }
    }

    protected internal void AddFile(IDownloadFile file)
    {
        lock (_queue)
        {
            _queue.Add(file);
        }

        CollectionChanged?.Invoke(
            Queue,
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, file)
        );
    }

    protected internal void RemoveFile(IDownloadFile file)
    {
        lock (_queue)
        {
            _queue.Remove(file);
        }

        CollectionChanged?.Invoke(
            Queue,
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, file)
        );
    }
}
