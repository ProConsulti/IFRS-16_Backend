using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace IFRS16_Backend.Services.Downlaod
{
    public class DownloadService : IDownloadService
    {
        public FileStreamResult DownloadLeaseTemplate(string fileName)
        {
            // Assets/ExcelFiles folder
            var excelFolder = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ExcelFiles");
            var filePath = Path.Combine(excelFolder, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{fileName}' not found.");
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
