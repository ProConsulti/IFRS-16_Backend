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

        [HttpGet("LeaseData/{companyId}")]
        public async Task<IActionResult> ExportCompany(int companyId)
        {
            var result = await _export.ExportCompanyData(companyId);
            if (result.Content == null || result.Content.Length == 0)
                return NotFound("Export failed.");

            return File(result.Content, "application/zip", result.FileName);
        }
    }
}
