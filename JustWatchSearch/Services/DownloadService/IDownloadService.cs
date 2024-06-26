using Common.Models;

namespace JustWatchSearch.Services.VT
{
    public interface IDownloadService
    {
        Task<bool> IsServiceAvailable();
        Task<List<ServiceInfo>> GetAvailableServices();
        Task<bool> StartDownload(string serviceId, string url, string quality);

    }
}
