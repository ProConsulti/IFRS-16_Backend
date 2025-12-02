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

        public async Task<(byte[] Content, string FileName)> ExportCompanyData(int companyId)
        {
            // build files in memory using temporary directories in memory stream -> zip
            using var tempStream = new MemoryStream();
            using (var archive = new ZipArchive(tempStream, ZipArchiveMode.Create, true))
            {
                // LeaseData
                var leaseDataEntry = archive.CreateEntry("LeaseData.sql");
                using (var entryStream = leaseDataEntry.Open())
                using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    await WriteTableToWriter("LeaseData", $"CompanyID = {companyId}", writer);
                }

                // Related tables
                var irEntry = archive.CreateEntry("InitialRecognition.sql");
                using (var entryStream = irEntry.Open())
                using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    await WriteTableToWriter("InitialRecognition", "LeaseID IN (SELECT LeaseID FROM LeaseData WHERE CompanyID = " + companyId + ")", writer);
                }

                var llEntry = archive.CreateEntry("LeaseLiability.sql");
                using (var entryStream = llEntry.Open())
                using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    await WriteTableToWriter("LeaseLiability", "LeaseID IN (SELECT LeaseID FROM LeaseData WHERE CompanyID = " + companyId + ")", writer);
                }

                var rouEntry = archive.CreateEntry("ROUSchedule.sql");
                using (var entryStream = rouEntry.Open())
                using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    await WriteTableToWriter("ROUSchedule", "LeaseID IN (SELECT LeaseID FROM LeaseData WHERE CompanyID = " + companyId + ")", writer);
                }

                var jeEntry = archive.CreateEntry("JournalEntries.sql");
                using (var entryStream = jeEntry.Open())
                using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    await WriteTableToWriter("JournalEntries", "LeaseID IN (SELECT LeaseID FROM LeaseData WHERE CompanyID = " + companyId + ")", writer);
                }
            }

            tempStream.Position = 0;
            var bytes = tempStream.ToArray();
            string fileName = $"Company_{companyId}_Export_{DateTime.Now:yyyyMMddHHmmss}.zip";
            return (bytes, fileName);
        }

        private async Task WriteTableToWriter(string table, string where, StreamWriter writer)
        {
            writer.WriteLine($"-- Export of {table}");
            writer.WriteLine();

            bool isLeaseData = table.Equals("LeaseData", StringComparison.OrdinalIgnoreCase);
            if (isLeaseData)
            {
                writer.WriteLine($"SET IDENTITY_INSERT [{table}] ON;");
                writer.WriteLine();
            }

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
                    // Determine columns to exclude: for LeaseData skip LeaseID; for other tables skip ID (identity)
                    var excludedColumn = "ID";

                    var allColumns = row.Keys.ToList();
                    var columnsToInclude = allColumns
                        .Where(c => !string.Equals(c, excludedColumn, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (columnsToInclude.Count == 0)
                        continue; // nothing to insert after exclusion

                    string columns = string.Join(",", columnsToInclude.Select(c => $"[{c}]"));
                    string values = string.Join(",", columnsToInclude.Select(c => FormatValue(row[c])));

                    writer.WriteLine($"INSERT INTO {table} ({columns}) VALUES ({values});");
                }

                offset += batch;
            }

            if (isLeaseData)
            {
                writer.WriteLine();
                writer.WriteLine($"SET IDENTITY_INSERT [{table}] OFF;");
            }


            await writer.FlushAsync();
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
