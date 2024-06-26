using Common.Models;

namespace DownloadApi.Services
{
    public interface IDownloadService
    {
        bool IsDownloaderAvailable();
        List<ServiceInfo> GetAvailableServices();

        int StartServiceDownload(string serviceId, string url, string quality);

    }
}
