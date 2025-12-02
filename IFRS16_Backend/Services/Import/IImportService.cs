using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IFRS16_Backend.Services.Import
{
    public interface IImportService
    {
        // Returns null on success, or error message on failure
        Task<string?> ImportFromZipAsync(IFormFile zipFile);
    }
}
