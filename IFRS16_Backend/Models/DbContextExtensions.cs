using Microsoft.EntityFrameworkCore;

namespace IFRS16_Backend.Models
{
    public static class DbContextExtensions
    {
        public static async Task<List<Dictionary<string, object>>> RawQueryDictionary(
            this DbContext db, string sql)
        {
            using var cmd = db.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;

            await db.Database.OpenConnectionAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var result = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var dict = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    dict[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);

                result.Add(dict);
            }

            return result;
        }
    }

}
