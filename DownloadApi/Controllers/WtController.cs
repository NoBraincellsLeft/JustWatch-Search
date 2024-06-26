using Common.Models;
using DownloadApi.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DownloadApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WtController : ControllerBase
    {
        private readonly IDownloadService _downloadService;
        private readonly IBackgroundJobClient _jobClient;

        public WtController(IDownloadService downloadService, IBackgroundJobClient jobClient)
        {
            _downloadService = downloadService;
            _jobClient = jobClient;
        }
        [HttpGet]
        public IActionResult GetServices()
        {
            _downloadService.IsDownloaderAvailable();
            return Ok(_downloadService.GetAvailableServices());
        }

        [HttpPost]
        public IActionResult StartDownload([FromBody] StartDownloadRequest request)
        {
            _jobClient.Enqueue<IDownloadService>(serv =>
                serv.StartServiceDownload(request.ServiceId, request.Url, request.Quality));
            return Ok();
        }
    }
}
