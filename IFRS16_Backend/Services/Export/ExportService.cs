using IFRS16_Backend.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFRS16_Backend.Services.Export
{
    public class ExportService(ApplicationDbContext context) : IExportService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<string> ExportCompanyData(int companyId)
        {
            // create unique folder for this export
            string root = Path.Combine(AppContext.BaseDirectory, "Exports");
            Directory.CreateDirectory(root);

            string folder = Path.Combine(root, $"Company_{companyId}_{DateTime.Now:yyyyMMddHHmmss}");
            Directory.CreateDirectory(folder);

            // Export each table to its own .sql file
            await ExportTable("LeaseData", $"CompanyID = {companyId}", Path.Combine(folder, "LeaseData.sql"));
            await ExportRelated("InitialRecognition", "LeaseID", "LeaseData", companyId, Path.Combine(root, "InitialRecognition.sql"));
            await ExportRelated("LeaseLiability", "LeaseID", "LeaseData", companyId, Path.Combine(root, "LeaseLiability.sql"));
            await ExportRelated("ROUSchedule", "LeaseID", "LeaseData", companyId, Path.Combine(root, "ROUSchedule.sql"));
            await ExportRelated("JournalEntries", "LeaseID", "LeaseData", companyId, Path.Combine(root, "JournalEntries.sql"));

            // create zip
            string zipPath = Path.Combine(root, $"Company_{companyId}_Export_{DateTime.Now:yyyyMMddHHmmss}.zip");

            // If zip exists, delete
            if (File.Exists(zipPath)) File.Delete(zipPath);

            ZipFile.CreateFromDirectory(folder, zipPath, CompressionLevel.Optimal, false);

            return zipPath;
        }


        private async Task ExportRelated(string table, string foreignKey, string parent, int companyId, string filePath)
        {
            string where = $"{foreignKey} IN (SELECT LeaseID FROM {parent} WHERE CompanyID = {companyId})";
            await ExportTable(table, where, filePath);
        }

        private async Task ExportTable(string table, string where, string filePath)
        {
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

            writer.WriteLine($"-- Export of {table}");
            writer.WriteLine();

            int batch = 10000;
            int offset = 0;

            while (true)
            {
                string sql = $@"
                SELECT * FROM {table}
                WHERE {where}
                ORDER BY (SELECT NULL)
                OFFSET {offset} ROWS FETCH NEXT {batch} ROWS ONLY";

                var rows = await _context.RawQueryDictionary(sql);

                if (rows.Count == 0)
                    break;

                foreach (var row in rows)
                {
                    string columns = string.Join(",", row.Keys);
                    string values = string.Join(",", row.Values.Select(FormatValue));

                    writer.WriteLine($"INSERT INTO {table} ({columns}) VALUES ({values});");
                }

                offset += batch;
            }
        }

        private static string FormatValue(object v)
        {
            if (v == null) return "NULL";
            if (v is DateTime dt) return $"'{dt:yyyy-MM-dd HH:mm:ss}'";
            if (v is string s) return $"'{s.Replace("'", "''")}'";
            if (v is bool b) return b ? "1" : "0";
            return v.ToString();
        }
    }
}
