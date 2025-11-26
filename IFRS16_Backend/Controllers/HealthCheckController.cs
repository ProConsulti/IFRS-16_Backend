using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IFRS16_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow
            });
        }

    }
}
