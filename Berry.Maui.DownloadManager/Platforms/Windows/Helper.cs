using Berry.Maui.Abstractions;
using Windows.Networking.BackgroundTransfer;

namespace Berry.Maui;

public static class Helper
{
    public static DownloadFileStatus ToDownloadFileStatus(this BackgroundTransferStatus status)
    {
        return status switch
        {
            BackgroundTransferStatus.Running => DownloadFileStatus.RUNNING,
            BackgroundTransferStatus.PausedByApplication => DownloadFileStatus.PAUSED,
            BackgroundTransferStatus.PausedCostedNetwork => DownloadFileStatus.PAUSED,
            BackgroundTransferStatus.PausedNoNetwork => DownloadFileStatus.PAUSED,
            BackgroundTransferStatus.Error => DownloadFileStatus.FAILED,
            BackgroundTransferStatus.Completed => DownloadFileStatus.COMPLETED,
            BackgroundTransferStatus.Canceled => DownloadFileStatus.CANCELED,
            BackgroundTransferStatus.Idle
            or BackgroundTransferStatus.PausedSystemPolicy
                => DownloadFileStatus.PAUSED,
            _ => DownloadFileStatus.INITIALIZED,
        };
    }
}
