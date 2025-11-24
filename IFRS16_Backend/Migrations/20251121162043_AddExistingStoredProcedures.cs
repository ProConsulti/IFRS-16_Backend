using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFRS16_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddExistingStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Read the SQL file and execute it
            var DeleteLeaseWithDependencies = File.ReadAllText(Path.Combine("Scripts", "dbo.DeleteLeaseWithDependencies.StoredProcedure.sql"));
            migrationBuilder.Sql(DeleteLeaseWithDependencies);

            var GetAllLeasesReport = File.ReadAllText(Path.Combine("Scripts", "dbo.GetAllLeasesReport.StoredProcedure.sql"));
            migrationBuilder.Sql(GetAllLeasesReport);

            var GetDisclouserForMaturityAnaylsis = File.ReadAllText(Path.Combine("Scripts", "dbo.GetDisclouserForMaturityAnaylsis.StoredProcedure.sql"));
            migrationBuilder.Sql(GetDisclouserForMaturityAnaylsis);

            var GetInitialRecognitionPaginated = File.ReadAllText(Path.Combine("Scripts", "dbo.GetInitialRecognitionPaginated.StoredProcedure.sql"));
            migrationBuilder.Sql(GetInitialRecognitionPaginated);

            var GetJournalEntriesPaginated = File.ReadAllText(Path.Combine("Scripts", "dbo.GetJournalEntriesPaginated.StoredProcedure.sql"));
            migrationBuilder.Sql(GetJournalEntriesPaginated);

            var GetJournalEntryReport = File.ReadAllText(Path.Combine("Scripts", "dbo.GetJournalEntryReport.StoredProcedure.sql"));
            migrationBuilder.Sql(GetJournalEntryReport);

            var GetLeaseDataPaginated = File.ReadAllText(Path.Combine("Scripts", "dbo.GetLeaseDataPaginated.StoredProcedure.sql"));
            migrationBuilder.Sql(GetLeaseDataPaginated);

            var GetLeaseLiabilityAggregation = File.ReadAllText(Path.Combine("Scripts", "dbo.GetLeaseLiabilityAggregation.StoredProcedure.sql"));
            migrationBuilder.Sql(GetLeaseLiabilityAggregation);

            var GetLeaseLiabilityPaginated = File.ReadAllText(Path.Combine("Scripts", "dbo.GetLeaseLiabilityPaginated.StoredProcedure.sql"));
            migrationBuilder.Sql(GetLeaseLiabilityPaginated);

            var GetROUAggregation = File.ReadAllText(Path.Combine("Scripts", "dbo.GetROUAggregation.StoredProcedure.sql"));
            migrationBuilder.Sql(GetROUAggregation);

            var GetROUSchedulePaginated = File.ReadAllText(Path.Combine("Scripts", "dbo.GetROUSchedulePaginated.StoredProcedure.sql"));
            migrationBuilder.Sql(GetROUSchedulePaginated);

            var GetSummarizeLeasesReport = File.ReadAllText(Path.Combine("Scripts", "dbo.GetSummarizeLeasesReport.StoredProcedure.sql"));
            migrationBuilder.Sql(GetSummarizeLeasesReport);

            var ModifyLease = File.ReadAllText(Path.Combine("Scripts", "dbo.ModifyLease.StoredProcedure.sql"));
            migrationBuilder.Sql(ModifyLease);

            var TerminateLease = File.ReadAllText(Path.Combine("Scripts", "dbo.TerminateLease.StoredProcedure.sql"));
            migrationBuilder.Sql(TerminateLease);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Optionally drop the stored procedures if needed. Leaving empty for now.
        }
    }
}
