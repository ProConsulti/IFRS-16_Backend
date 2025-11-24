using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFRS16_Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rental",
                table: "LeaseData",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<double>(
                name: "IBR",
                table: "LeaseData",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AssetType",
                table: "LeaseData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "LeaseData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "LeaseData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "GRV",
                table: "LeaseData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IDC",
                table: "LeaseData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Increment",
                table: "LeaseData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncrementalFrequency",
                table: "LeaseData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LeaseData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "LeaseData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "LeaseData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "LeaseData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AllLeasesReport",
                columns: table => new
                {
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    LeaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rental = table.Column<double>(type: "float", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommencementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpeningLL = table.Column<double>(type: "float", nullable: true),
                    Interest = table.Column<double>(type: "float", nullable: true),
                    Payment = table.Column<double>(type: "float", nullable: true),
                    ClosingLL = table.Column<double>(type: "float", nullable: true),
                    OpeningROU = table.Column<double>(type: "float", nullable: true),
                    Amortization = table.Column<double>(type: "float", nullable: true),
                    ClosingROU = table.Column<double>(type: "float", nullable: true),
                    Exchange_Gain_Loss = table.Column<double>(type: "float", nullable: true),
                    ModificationAdjustmentLL = table.Column<double>(type: "float", nullable: true),
                    CurrencyID = table.Column<int>(type: "int", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionsDuringYearLL = table.Column<double>(type: "float", nullable: true),
                    AdditionsDuringYearROU = table.Column<double>(type: "float", nullable: true),
                    ModificationAdjustmentROU = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyID);
                });

            migrationBuilder.CreateTable(
                name: "DisclouserMaturityAnalysis",
                columns: table => new
                {
                    LessThanOneYear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BetweenOneAndFiveYears = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AfterFiveYears = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    ExchangeRateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyID = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExchangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.ExchangeRateID);
                });

            migrationBuilder.CreateTable(
                name: "FC_JournalEntries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    JE_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Particular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FC_JournalEntries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FC_LeaseLiability",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    LeaseLiability_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opening = table.Column<double>(type: "float", nullable: false),
                    Interest = table.Column<double>(type: "float", nullable: false),
                    Payment = table.Column<double>(type: "float", nullable: false),
                    Closing = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FC_LeaseLiability", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FC_ROUSchedule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    ROU_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opening = table.Column<double>(type: "float", nullable: false),
                    Amortization = table.Column<double>(type: "float", nullable: false),
                    Closing = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FC_ROUSchedule", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InitialRecognition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rental = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NPV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialRecognition", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    JE_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Particular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryReport",
                columns: table => new
                {
                    Particular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LeaseDataContracts",
                columns: table => new
                {
                    LeaseContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    ContractDoc = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DocFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseDataContracts", x => x.LeaseContractId);
                });

            migrationBuilder.CreateTable(
                name: "LeaseDataSP",
                columns: table => new
                {
                    LeaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    LeaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rental = table.Column<double>(type: "float", nullable: false),
                    CommencementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Annuity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IBR = table.Column<double>(type: "float", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDC = table.Column<double>(type: "float", nullable: true),
                    GRV = table.Column<double>(type: "float", nullable: true),
                    Increment = table.Column<double>(type: "float", nullable: true),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseDataSP", x => x.LeaseId);
                });

            migrationBuilder.CreateTable(
                name: "LeaseLiability",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    LeaseLiability_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opening = table.Column<double>(type: "float", nullable: false),
                    Interest = table.Column<double>(type: "float", nullable: false),
                    Payment = table.Column<double>(type: "float", nullable: false),
                    Closing = table.Column<double>(type: "float", nullable: false),
                    Exchange_Gain_Loss = table.Column<double>(type: "float", nullable: true),
                    ModificationAdjustment = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseLiability", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LeasesReportSummary",
                columns: table => new
                {
                    OpeningLL = table.Column<double>(type: "float", nullable: true),
                    Interest = table.Column<double>(type: "float", nullable: true),
                    Payment = table.Column<double>(type: "float", nullable: true),
                    ClosingLL = table.Column<double>(type: "float", nullable: true),
                    OpeningROU = table.Column<double>(type: "float", nullable: true),
                    Amortization = table.Column<double>(type: "float", nullable: true),
                    ClosingROU = table.Column<double>(type: "float", nullable: true),
                    Exchange_Gain_Loss = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ROUSchedule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    ROU_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opening = table.Column<double>(type: "float", nullable: false),
                    Amortization = table.Column<double>(type: "float", nullable: false),
                    Closing = table.Column<double>(type: "float", nullable: false),
                    ModificationAdjustment = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUSchedule", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SessionToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionToken", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllLeasesReport");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "DisclouserMaturityAnalysis");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "FC_JournalEntries");

            migrationBuilder.DropTable(
                name: "FC_LeaseLiability");

            migrationBuilder.DropTable(
                name: "FC_ROUSchedule");

            migrationBuilder.DropTable(
                name: "InitialRecognition");

            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "JournalEntryReport");

            migrationBuilder.DropTable(
                name: "LeaseDataContracts");

            migrationBuilder.DropTable(
                name: "LeaseDataSP");

            migrationBuilder.DropTable(
                name: "LeaseLiability");

            migrationBuilder.DropTable(
                name: "LeasesReportSummary");

            migrationBuilder.DropTable(
                name: "ROUSchedule");

            migrationBuilder.DropTable(
                name: "SessionToken");

            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "GRV",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "IDC",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "Increment",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "IncrementalFrequency",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "LeaseData");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "LeaseData");

            migrationBuilder.AlterColumn<long>(
                name: "Rental",
                table: "LeaseData",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "IBR",
                table: "LeaseData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
