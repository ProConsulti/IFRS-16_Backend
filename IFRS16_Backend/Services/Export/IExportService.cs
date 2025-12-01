using System.Threading.Tasks;

namespace IFRS16_Backend.Services.Export
{
    public interface IExportService
    {
        Task<string> ExportCompanyData(int companyId);
    }
}
