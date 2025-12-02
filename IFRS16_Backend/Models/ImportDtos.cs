using Microsoft.AspNetCore.Http;

namespace IFRS16_Backend.Models
{
    public class ImportUploadDto
    {
        public IFormFile File { get; set; }
    }
}
