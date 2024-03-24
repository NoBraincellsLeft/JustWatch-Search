using System.Text;
using Common.Models;
using Newtonsoft.Json;

namespace JustWatchSearch.Services.VT;

public class VTDownloadService : IDownloadService
{
    private readonly HttpClient _client;
    private readonly ILogger<VTDownloadService> _logger;

    public VTDownloadService(HttpClient client, ILogger<VTDownloadService> logger)
    {
        _client = client;
        _logger = logger;
    }
    public async Task<bool> IsServiceAvailable()
    {
        try
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            var response = await _client.GetAsync("api/Health", cts.Token);
            return response.IsSuccessStatusCode;
        }
        catch (Exception exception)
        {

            _logger.LogError(exception,"Cannot connect ot api ");
            return false;
        }
    }

    public async Task<List<ServiceInfo>> GetAvailableServices()
    {
        var result = new List<ServiceInfo>();
        try
        {
            var response = await _client.GetAsync("api/Wt/GetServices");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<List<ServiceInfo>>(responseJson);
                result.AddRange(responseObj);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error getting services");
        }
        return result;
    }

    public async Task<bool> StartDownload(string serviceId, string url, string quality)
    {
        var request = new StartDownloadRequest()
        {
            Url = url,
            Quality = quality,
            ServiceId = serviceId
        };
       
        var content = new StringContent(JsonConvert.SerializeObject(request),Encoding.UTF8,"application/json");
        var resp = await _client.PostAsync("api/Wt/StartDownload",content);
        resp.EnsureSuccessStatusCode();
        return true;
    }
}