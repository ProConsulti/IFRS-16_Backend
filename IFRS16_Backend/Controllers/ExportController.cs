using IFRS16_Backend.Services.Export;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IFRS16_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ExportController(IExportService export) : ControllerBase
    {
        private readonly IExportService _export = export;

        [HttpPost("LeaseData")]
        public async Task<IActionResult> ExportCompany(int companyId)
        { // start export (this runs synchronously in request — if it takes too long, consider background job)
            var zipPath = await _export.ExportCompanyData(companyId);

            if (!System.IO.File.Exists(zipPath))
                return NotFound("Export failed.");

            var fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = File(fs, "application/zip", Path.GetFileName(zipPath));
            // Optionally: You can schedule deletion of zipPath after some time.
            return result;
        }
    }
}
