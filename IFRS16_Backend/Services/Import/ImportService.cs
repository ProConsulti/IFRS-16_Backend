using IFRS16_Backend.Models;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace IFRS16_Backend.Services.Import
{
    public class ImportService(ApplicationDbContext context) : IImportService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<string?> ImportFromZipAsync(IFormFile zipFile)
        {
            if (zipFile == null || zipFile.Length == 0)
                return "File can't be empty.";

            using var ms = new MemoryStream();
            await zipFile.CopyToAsync(ms);
            ms.Position = 0;

            using var archive = new ZipArchive(ms, ZipArchiveMode.Read, false);

            // Process SQL entries in deterministic order
            var sqlEntries = archive.Entries
                .Where(e => e.Name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Use underlying DbConnection and DbTransaction to execute statements
            var connection = _context.Database.GetDbConnection();
            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var dbTransaction = await connection.BeginTransactionAsync();

                try
                {
                    foreach (var entry in sqlEntries)
                    {
                        using var entryStream = entry.Open();
                        using var reader = new StreamReader(entryStream, Encoding.UTF8);
                        string sql = await reader.ReadToEndAsync();

                        if (string.IsNullOrWhiteSpace(sql))
                            continue;

                        var statements = sql.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrWhiteSpace(s));

                        foreach (var stmt in statements)
                        {
                            using var cmd = connection.CreateCommand();
                            cmd.Transaction = dbTransaction;
                            cmd.CommandText = stmt + ";";
                            cmd.CommandType = CommandType.Text;

                            try
                            {
                                await cmd.ExecuteNonQueryAsync();
                            }
                            catch (Exception ex)
                            {
                                // return DB error message
                                try { await dbTransaction.RollbackAsync(); } catch { }
                                return ExtractDbErrorMessage(ex);
                            }
                        }
                    }

                    await dbTransaction.CommitAsync();
                    return null; // success
                }
                catch (Exception ex)
                {
                    try { await dbTransaction.RollbackAsync(); } catch { }
                    return ExtractDbErrorMessage(ex);
                }
            }
            finally
            {
                try { await connection.CloseAsync(); } catch { }
            }
        }

        private static string ExtractDbErrorMessage(Exception ex)
        {
            if (ex == null) return "Unknown error";

            // Walk inner exceptions to find a database-specific message
            var current = ex;
            while (current.InnerException != null)
                current = current.InnerException;

            if (current is SqlException sqlEx)
            {
                // Include number and message, and server if available
                return $"SQL Error {sqlEx.Number}: {sqlEx.Message}" + (string.IsNullOrEmpty(sqlEx.Server) ? string.Empty : $" (Server: {sqlEx.Server})");
            }

            if (current is DbException dbEx)
            {
                return $"DB Error: {dbEx.Message}";
            }

            return current.Message ?? ex.Message;
        }
    }
}
