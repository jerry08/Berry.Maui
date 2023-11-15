using System;
using System.Collections.Generic;
using System.ComponentModel;
using Berry.Maui.Abstractions;
using Foundation;

namespace Berry.Maui;

public class DownloadFileImplementation : IDownloadFile
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /**
     * The task, running in the background
     */
    public NSUrlSessionTask Task;

    public string Url { get; }

    private string? _destinationPathName;

    public string? DestinationPathName
    {
        get { return _destinationPathName; }
        set
        {
            if (Equals(_destinationPathName, value))
                return;
            _destinationPathName = value;
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(DestinationPathName))
            );
        }
    }

    public IDictionary<string, string> Headers { get; }

    DownloadFileStatus _status;

    public DownloadFileStatus Status
    {
        get { return _status; }
        set
        {
            if (Equals(_status, value))
                return;
            _status = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
        }
    }

    string? _statusDetails;

    public string? StatusDetails
    {
        get { return _statusDetails; }
        set
        {
            if (Equals(_statusDetails, value))
                return;
            _statusDetails = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusDetails)));
        }
    }

    private float _totalBytesExpected;

    public float TotalBytesExpected
    {
        get { return _totalBytesExpected; }
        set
        {
            if (Equals(_totalBytesExpected, value))
                return;
            _totalBytesExpected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalBytesExpected)));
        }
    }

    private float _totalBytesWritten;

    public float TotalBytesWritten
    {
        get { return _totalBytesWritten; }
        set
        {
            if (Equals(_totalBytesWritten, value))
                return;
            _totalBytesWritten = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalBytesWritten)));
        }
    }

    /**
     * Initializing a new object to add it to the download-queue
     */
    public DownloadFileImplementation(string url, IDictionary<string, string> headers)
    {
        Url = url;
        Headers = headers;

        Status = DownloadFileStatus.INITIALIZED;
    }

    /**
     * Called when re-initializing the app after the app shut down to be able to still handle on-success calls.
     */
    public DownloadFileImplementation(NSUrlSessionTask task)
    {
        Url = task.OriginalRequest.Url.AbsoluteString;
        Headers = new Dictionary<string, string>();

        foreach (var header in task.OriginalRequest.Headers)
        {
            Headers.Add(
                new KeyValuePair<string, string>(header.Key.ToString(), header.Value.ToString())
            );
        }

        Status = task.State switch
        {
            NSUrlSessionTaskState.Running => DownloadFileStatus.RUNNING,
            NSUrlSessionTaskState.Completed => DownloadFileStatus.COMPLETED,
            NSUrlSessionTaskState.Canceling => DownloadFileStatus.RUNNING,
            NSUrlSessionTaskState.Suspended => DownloadFileStatus.PAUSED,
            _ => throw new ArgumentOutOfRangeException(),
        };
        Task = task;
    }

    public void StartDownload(NSUrlSession session, bool allowsCellularAccess)
    {
        using var downloadUrl = NSUrl.FromString(Url);
        using var request = new NSMutableUrlRequest(downloadUrl);
        if (Headers != null)
        {
            var headers = new NSMutableDictionary();
            foreach (var header in Headers)
            {
                headers.SetValueForKey(new NSString(header.Value), new NSString(header.Key));
            }
            request.Headers = headers;
        }

        request.AllowsCellularAccess = allowsCellularAccess;

        Task = session.CreateDownloadTask(request);
        Task.Resume();
    }
}
