using System.Threading.Tasks;

namespace IFRS16_Backend.Services.Export
{
    public interface IExportService
    {
        // Returns zip content bytes and suggested file name
        Task<(byte[] Content, string FileName)> ExportCompanyData(int companyId);
    }
}
