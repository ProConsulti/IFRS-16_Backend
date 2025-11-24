using IFRS16_Backend.Services.Downlaod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IFRS16_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController(IDownloadService downloadService) : ControllerBase
    {
        private readonly IDownloadService _downloadService = downloadService;

        [HttpGet("leaseTemplate/{fileName}")]
        [AllowAnonymous]
        public IActionResult DownloadReport(string fileName)
        {
            try
            {
                var file = _downloadService.DownloadLeaseTemplate(fileName);
                return file;
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
