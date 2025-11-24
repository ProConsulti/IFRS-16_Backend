using IFRS16_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace IFRS16_Backend.Services.Downlaod
{
    public interface IDownloadService
    {
        FileStreamResult DownloadLeaseTemplate(string fileName);
    }
}
