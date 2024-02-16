using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Berry.Maui.Abstractions;
using Foundation;
using UIKit;

namespace Berry.Maui;

/// <summary>
/// The iOS implementation of the download manager.
/// </summary>
public class DownloadManagerImplementation : IDownloadManager
{
    private string _identifier =>
        NSBundle.MainBundle.BundleIdentifier + ".BackgroundTransferSession";

    private readonly bool _avoidDiscretionaryDownloadInBackground;

    private readonly int _httpMaximumConnectionsPerHost;

    private readonly NSUrlSession _backgroundSession;

    private readonly NSUrlSession _session;

    public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanged;

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

    public Func<IDownloadFile, string>? PathNameForDownloadedFile { get; set; }

    public DownloadManagerImplementation(
        UrlSessionDownloadDelegate sessionDownloadDelegate,
        bool avoidDiscretionaryDownloadInBackground,
        int httpMaximumConnectionsPerHost
    )
    {
        _avoidDiscretionaryDownloadInBackground = avoidDiscretionaryDownloadInBackground;
        _httpMaximumConnectionsPerHost = httpMaximumConnectionsPerHost;
        _queue = [];

        if (avoidDiscretionaryDownloadInBackground)
        {
            _session = InitDefaultSession(sessionDownloadDelegate);
        }

        _backgroundSession = InitBackgroundSession(sessionDownloadDelegate);

        // Reinitialize tasks that were started before the app was terminated or suspended
        _backgroundSession.GetTasks(
            (dataTasks, uploadTasks, downloadTasks) =>
            {
                foreach (var task in downloadTasks)
                {
                    AddFile(new DownloadFileImplementation(task));
                }
            }
        );
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

        AddFile(file);

        NSOperationQueue.MainQueue.BeginInvokeOnMainThread(() =>
        {
            NSUrlSession session;

            var inBackground =
                UIApplication.SharedApplication.ApplicationState == UIApplicationState.Background;

            if (_avoidDiscretionaryDownloadInBackground && inBackground)
            {
                session = _session;
            }
            else
            {
                session = _backgroundSession;
            }

            file.StartDownload(session, mobileNetworkAllowed);
        });
    }

    public void Abort(IDownloadFile i)
    {
        var file = (DownloadFileImplementation)i;

        file.Status = DownloadFileStatus.CANCELED;
        file.Task?.Cancel();

        RemoveFile(file);
    }

    public void AbortAll()
    {
        foreach (var file in Queue)
        {
            Abort(file);
        }
    }

    /**
     * We initialize the background session with the following options
     * - nil as queue: The method, called on events could end up on any thread
     * - Only one connection per host
     */
    NSUrlSession InitBackgroundSession(UrlSessionDownloadDelegate sessionDownloadDelegate)
    {
        sessionDownloadDelegate.Controller = this;

        NSUrlSessionConfiguration configuration;

        if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
        {
            configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(
                _identifier
            );
        }
        else
        {
            configuration = NSUrlSessionConfiguration.BackgroundSessionConfiguration(_identifier);
        }

        return InitSession(sessionDownloadDelegate, configuration);
    }

    NSUrlSession InitDefaultSession(UrlSessionDownloadDelegate sessionDownloadDelegate)
    {
        return InitSession(
            sessionDownloadDelegate,
            NSUrlSessionConfiguration.DefaultSessionConfiguration
        );
    }

    NSUrlSession InitSession(
        UrlSessionDownloadDelegate sessionDownloadDelegate,
        NSUrlSessionConfiguration configuration
    )
    {
        sessionDownloadDelegate.Controller = this;

        using (configuration)
        {
            return createSession(configuration, sessionDownloadDelegate);
        }
    }

    private NSUrlSession createSession(
        NSUrlSessionConfiguration configuration,
        UrlSessionDownloadDelegate sessionDownloadDelegate
    )
    {
        configuration.HttpMaximumConnectionsPerHost = _httpMaximumConnectionsPerHost;

        return NSUrlSession.FromConfiguration(configuration, sessionDownloadDelegate, null);
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
