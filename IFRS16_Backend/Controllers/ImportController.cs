using IFRS16_Backend.Services.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IFRS16_Backend.Models;

namespace IFRS16_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ImportController(IImportService importService) : ControllerBase
    {
        private readonly IImportService _importService = importService;

        [HttpPost("UploadZip")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadZip([FromForm] ImportUploadDto dto)
        {
            if (dto?.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded.");

            var res = await _importService.ImportFromZipAsync(dto.File!);
            if (!string.IsNullOrEmpty(res))
                return BadRequest(new { message = res });

            return Ok(new { message = "Import successful." });
        }
    }
}
