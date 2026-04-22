using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BillNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 StockIn, 1 StockOut"),
                    CurrencyUnit = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalAmount = table.Column<double>(type: "double", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "CalendarObject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Notice, 1 Meeting Room, 2 HiEnd PC"),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarObject", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "GoogleTimesheetKey",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpreadsheetId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientEmail = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrivateKey = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SheetId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleTimesheetKey", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Type = table.Column<ulong>(type: "bit(1)", nullable: false, defaultValueSql: "b'0'", comment: "0 holiday, 1 special"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 pending, 1 Apply, 2 Deleting"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Leave",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "6 Compensatory, 7 SummerVacation, 8 Annual"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Hours = table.Column<double>(type: "double", nullable: false),
                    Using = table.Column<double>(type: "double", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "LeaveHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LeaveId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 AddAnnualLeave, 1 AddSummerLeave, 2 AddCompensatoryLeave, 3 TakeBackAnnualLeave, 4 TakeBackSummerLeave, 5 TakeBackCompensatoryLeave, 6 UsingAnnualLeave, 7 UsingSummerLeave, 8 UsingCompensatoryLeave"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnnualLeave = table.Column<double>(type: "double", nullable: false),
                    SummerLeave = table.Column<double>(type: "double", nullable: false),
                    CompensatoryLeave = table.Column<double>(type: "double", nullable: false),
                    Unit = table.Column<double>(type: "double", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveHistory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "LeaveInformation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeName = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Detail = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LeaveDay = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartCondition = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    End = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Block = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Using = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveInformation", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "LeaveRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Compensatory, 7 SummerVacation, 8 Annual, 9 Unpaid"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hour = table.Column<double>(type: "double", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 Pending, 1 Approve, 2 Reject"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequest", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Information, 1 Approve, 2 Reject"),
                    ObjectType = table.Column<int>(type: "int", nullable: false, comment: "0 Notification, 1 LeaveRequest, 2 OvertimeRequest, 3 WfhRequest, 4 NoticeInvite, 5 AssetInvite"),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OvertimeRule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Weekday, 1 Weekend, 2 OverNight, 3 Holiday"),
                    Weekday = table.Column<int>(type: "int", nullable: true, comment: "0 = Monday, 1 = Tuesday, 2 = Wednesday, 3 = Thursday, 4 = Friday, 5 = Saturday, 6 = Sunday"),
                    HourStart = table.Column<double>(type: "double", nullable: true),
                    HourEnd = table.Column<double>(type: "double", nullable: true),
                    Multiplier = table.Column<double>(type: "double", nullable: false, comment: "hệ số nhân"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeRule", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OvertimeSummary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Month = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HourWeekday = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourWeekend = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourOvernight = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourHoliday = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourWeekdayToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    HourWeekendToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    HourOvernightToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    HourHolidayToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeSummary", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SwapDay",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Content = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapDay", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SystemRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 DefineSummerLeave, 5 TakeBackLeave"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRequest", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateStartWork = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<ulong>(type: "bit(1)", nullable: false, defaultValueSql: "b'1'", comment: "0 Inactive, 1 Active"),
                    State = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0", comment: "0 Available, 1 WFH, 2 Business, 3 Busy, 4 Unavailable"),
                    StaffId = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Position = table.Column<int>(type: "int", maxLength: 30, nullable: false, comment: "0 System, 1 GM, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership"),
                    Avatar = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "'male'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IssuedTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    IssuedBy = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SocialInsuranceBookNo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AboutMe = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FingerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmailNotification = table.Column<ulong>(type: "bit(1)", nullable: false, defaultValueSql: "b'1'"),
                    Timezone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValueSql: "'BangKok, Ha Noi, Jakarta (UTC +7)'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Language = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValueSql: "'English'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "WfhRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValueSql: "b'0'", comment: "0 Pending, 1 Approve, 2 Reject"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfhRequest", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    RepeatType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, comment: "None, EveryDay, EveryWeek, EveryMonth", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RepeatId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Id);
                    table.ForeignKey(
                        name: "fk_calendar_object",
                        column: x => x.ObjectId,
                        principalTable: "CalendarObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectManagement = table.Column<long>(type: "bigint", nullable: false, comment: "PM"),
                    ClientId = table.Column<long>(type: "bigint", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 Pending, 1 Active, 2 End"),
                    IsPublic = table.Column<ulong>(type: "bit(1)", nullable: true, defaultValueSql: "b'0'"),
                    ProjectNumber = table.Column<int>(type: "int", nullable: false),
                    QuotationHour = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "T, I, H", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    WorkingHour = table.Column<double>(type: "double", nullable: false),
                    OvertimeHour = table.Column<double>(type: "double", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "fk_projects_client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProjectGroup",
                columns: table => new
                {
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.ProjectId, x.GroupId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_project_group_group",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "HolidayApply",
                columns: table => new
                {
                    HolidayId = table.Column<long>(type: "bigint", nullable: false),
                    ApplyId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false, comment: "User, Team", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.HolidayId, x.ApplyId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_holiday_user",
                        column: x => x.HolidayId,
                        principalTable: "Holiday",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "LeaveHistoryUsing",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LeaveId = table.Column<long>(type: "bigint", nullable: false),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Hours = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveHistoryUsing", x => x.Id);
                    table.ForeignKey(
                        name: "fk_leave_history_using_leave_request",
                        column: x => x.LeaveRequestId,
                        principalTable: "LeaveRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaveHistoryUsing_Leave_LeaveId",
                        column: x => x.LeaveId,
                        principalTable: "Leave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "LeaveRequestDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Hours = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_leave_request_detail",
                        column: x => x.LeaveRequestId,
                        principalTable: "LeaveRequest",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "NotificationAssign",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<ulong>(type: "bit(1)", nullable: false, defaultValueSql: "b'0'", comment: "0 unread, 1 read"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAssign", x => x.Id);
                    table.ForeignKey(
                        name: "fk_notification_assign",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Species = table.Column<int>(type: "int", nullable: false, comment: "0 Hardware, 1 Software"),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    InStock = table.Column<int>(type: "int", nullable: false),
                    Using = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 Available, 1 Unavailable"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_type",
                        column: x => x.TypeId,
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SwapDayUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SwapDayId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapDayUser", x => x.Id);
                    table.ForeignKey(
                        name: "fk_swap_day",
                        column: x => x.SwapDayId,
                        principalTable: "SwapDay",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TeamCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUsing = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamCategory", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_categories_team",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TeamObject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUsing = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamObject", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_objects_team",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.GroupId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_group_users_group",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_group_users_user",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 pending, 1 approverd, 2 rejected, 3 processed"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRequest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TeamUser",
                columns: table => new
                {
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.TeamId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_team_users_team",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_team_users_user",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Timesheet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    LockBy = table.Column<long>(type: "bigint", nullable: true),
                    HolidayName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SwapDay = table.Column<DateTime>(type: "datetime", nullable: true),
                    WfhRequestId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheet", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_user",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_timesheet_wfhRequest",
                        column: x => x.WfhRequestId,
                        principalTable: "WfhRequest",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OvertimeRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValueSql: "b'0'", comment: "0 Pending, 1 Approve, 2 Reject, 3 Calculated"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "fk_overtime_project",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProjectDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comment = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocument", x => x.Id);
                    table.ForeignKey(
                        name: "fk_project_document",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.ProjectId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_project_users_project",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_project_users_user",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "BillDetail",
                columns: table => new
                {
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: true),
                    Unit = table.Column<double>(type: "double", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.ProductId, x.BillId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_bill_details",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_bill_details_product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 InStock, 1 Using, 2 OutStock"),
                    Serial = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_items",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "FingerPrinter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    DateIn = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateOut = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HourTotal = table.Column<double>(type: "double", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerPrinter", x => x.Id);
                    table.ForeignKey(
                        name: "fk_fingerprint_timesheet",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheet",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TimesheetDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Project, 1 Overtime, 2 UnpaidLeave, 3 PaidLeave"),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: false, comment: "reference to overtime_request, leave_request, holiday_id"),
                    TimesheetCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    TimesheetObjectId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hours = table.Column<double>(type: "double", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_detail_timesheet",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheet",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TimesheetReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    Hours = table.Column<double>(type: "double", nullable: false),
                    OvertimeHours = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetReport", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_report",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheet",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OvertimeRequestDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvertimeRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Weekday, 1 Weekend, 2 OverNight, 3 Holiday"),
                    Paid = table.Column<int>(type: "int", nullable: false, comment: "0 Salary, 1 Compensatory"),
                    Start = table.Column<DateTime>(type: "datetime", nullable: false),
                    End = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActualHour = table.Column<double>(type: "double", nullable: false),
                    Multiplier = table.Column<double>(type: "double", nullable: false, comment: "hệ số tại thời điểm approve"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_overtime_detail",
                        column: x => x.OvertimeRequestId,
                        principalTable: "OvertimeRequest",
                        principalColumn: "Id");
                },
                comment: "request OT detail")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 New, 1 InStock, 2 Using, 3 OutStock"),
                    Content = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductHistory", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_item_history",
                        column: x => x.ItemId,
                        principalTable: "ProductItem",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductRequestDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ProductItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_request_details",
                        column: x => x.ProductRequestId,
                        principalTable: "ProductRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductRequestDetail_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SwapDayRefer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateIn = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateOut = table.Column<DateTime>(type: "datetime", nullable: false),
                    HourTotal = table.Column<double>(type: "double", nullable: false),
                    FingerPrinterId = table.Column<long>(type: "bigint", nullable: false),
                    SwapDayId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapDayRefer", x => x.Id);
                    table.ForeignKey(
                        name: "fk_swapday_refer_timesheet",
                        column: x => x.FingerPrinterId,
                        principalTable: "FingerPrinter",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_bill_no_idx",
                table: "Bill",
                column: "BillNo");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetail_BillId",
                table: "BillDetail",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "fk_calendar_user_idx",
                table: "Calendar",
                columns: new[] { "CreatedBy", "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_ObjectId",
                table: "Calendar",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_date_idx",
                table: "FingerPrinter",
                columns: new[] { "TimesheetId", "DateIn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FingerPrinter_TimesheetId",
                table: "FingerPrinter",
                column: "TimesheetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "group_code_UNIQUE",
                table: "Group",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_group_users_group",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "fk_group_users_idx",
                table: "GroupUsers",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_holiday_type_date_idx",
                table: "Holiday",
                columns: new[] { "Type", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "fk_holiday_idx",
                table: "HolidayApply",
                column: "HolidayId");

            migrationBuilder.CreateIndex(
                name: "fk_leave_user_idx",
                table: "Leave",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "fk_leave_history_reference_type_idx",
                table: "LeaveHistory",
                columns: new[] { "LeaveId", "LeaveRequestId", "Type" });

            migrationBuilder.CreateIndex(
                name: "fk_leave_history_user_idx",
                table: "LeaveHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "fk_leave_history_using_idx",
                table: "LeaveHistoryUsing",
                columns: new[] { "LeaveId", "LeaveRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_leave_history_using_leave",
                table: "LeaveHistoryUsing",
                column: "LeaveId");

            migrationBuilder.CreateIndex(
                name: "fk_leave_history_using_leave_request",
                table: "LeaveHistoryUsing",
                column: "LeaveRequestId");

            migrationBuilder.CreateIndex(
                name: "fk_leave_request_created_by_idx",
                table: "LeaveRequest",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "fk_leave_history_using_leave_request1",
                table: "LeaveRequestDetail",
                column: "LeaveRequestId");

            migrationBuilder.CreateIndex(
                name: "fk_notification_date_idx",
                table: "Notification",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "fk_notification_object_idx",
                table: "Notification",
                columns: new[] { "CreatedDate", "ObjectType", "ObjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_notification_assign_user_idx",
                table: "NotificationAssign",
                columns: new[] { "UserId", "NotificationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAssign_NotificationId",
                table: "NotificationAssign",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "fk_leave_request_created_by_idx1",
                table: "OvertimeRequest",
                columns: new[] { "CreatedBy", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "fk_overtime_project",
                table: "OvertimeRequest",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "fk_overtime_request_id_detail",
                table: "OvertimeRequestDetail",
                column: "OvertimeRequestId");

            migrationBuilder.CreateIndex(
                name: "fk_overtime_rule_idx",
                table: "OvertimeRule",
                columns: new[] { "Weekday", "HourStart", "HourEnd" });

            migrationBuilder.CreateIndex(
                name: "fk_overtime_summary_idx",
                table: "OvertimeSummary",
                columns: new[] { "UserId", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_TypeId",
                table: "Product",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "fk_product_history_item_idx",
                table: "ProductHistory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "fk_product_item_bill_id_idx",
                table: "ProductItem",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "fk_product_item_id_idx",
                table: "ProductItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequest_UserId",
                table: "ProductRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequestDetail_ProductItemId",
                table: "ProductRequestDetail",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequestDetail_ProductRequestId",
                table: "ProductRequestDetail",
                column: "ProductRequestId");

            migrationBuilder.CreateIndex(
                name: "fk_projects_client",
                table: "Project",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "fk_projects_leader_idx",
                table: "Project",
                column: "ProjectManagement");

            migrationBuilder.CreateIndex(
                name: "fk_projects_teams_idx",
                table: "Project",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "fk_project_document_idx",
                table: "ProjectDocument",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "fk_project_idx",
                table: "ProjectGroup",
                columns: new[] { "GroupId", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_project_user_idx",
                table: "ProjectUsers",
                columns: new[] { "UserId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "fk_project_users_project",
                table: "ProjectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "fk_swap_day_idx",
                table: "SwapDay",
                column: "FromDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SwapDayRefer_FingerPrinterId",
                table: "SwapDayRefer",
                column: "FingerPrinterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_swap_day_user_idx",
                table: "SwapDayUser",
                columns: new[] { "UserId", "SwapDayId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SwapDayUser_SwapDayId",
                table: "SwapDayUser",
                column: "SwapDayId");

            migrationBuilder.CreateIndex(
                name: "fk_system_request_idx",
                table: "SystemRequest",
                columns: new[] { "Type", "Date" });

            migrationBuilder.CreateIndex(
                name: "team_code_UNIQUE",
                table: "Team",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_catogeries_teams_idx",
                table: "TeamCategory",
                columns: new[] { "TeamId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_objects_teams_idx",
                table: "TeamObject",
                columns: new[] { "TeamId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_team_users_idx",
                table: "TeamUser",
                columns: new[] { "UserId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_team_users_team",
                table: "TeamUser",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "TeamUser",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_user_date_idx",
                table: "Timesheet",
                columns: new[] { "UserId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_user_idx",
                table: "Timesheet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_WfhRequestId",
                table: "Timesheet",
                column: "WfhRequestId");

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_detail_project_id_idx",
                table: "TimesheetDetail",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_detail_timesheet_id_idx",
                table: "TimesheetDetail",
                column: "TimesheetId");

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_detail_type_idx",
                table: "TimesheetDetail",
                columns: new[] { "ReferenceId", "Type" });

            migrationBuilder.CreateIndex(
                name: "fk_report_timesheet_id_idx",
                table: "TimesheetReport",
                columns: new[] { "TimesheetId", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "user_id_UNIQUE",
                table: "UserInfo",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "user_id_UNIQUE1",
                table: "UserSetting",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_leave_request_created_by_idx2",
                table: "WfhRequest",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetail");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "GoogleTimesheetKey");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "HolidayApply");

            migrationBuilder.DropTable(
                name: "LeaveHistory");

            migrationBuilder.DropTable(
                name: "LeaveHistoryUsing");

            migrationBuilder.DropTable(
                name: "LeaveInformation");

            migrationBuilder.DropTable(
                name: "LeaveRequestDetail");

            migrationBuilder.DropTable(
                name: "NotificationAssign");

            migrationBuilder.DropTable(
                name: "OvertimeRequestDetail");

            migrationBuilder.DropTable(
                name: "OvertimeRule");

            migrationBuilder.DropTable(
                name: "OvertimeSummary");

            migrationBuilder.DropTable(
                name: "ProductHistory");

            migrationBuilder.DropTable(
                name: "ProductRequestDetail");

            migrationBuilder.DropTable(
                name: "ProjectDocument");

            migrationBuilder.DropTable(
                name: "ProjectGroup");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SwapDayRefer");

            migrationBuilder.DropTable(
                name: "SwapDayUser");

            migrationBuilder.DropTable(
                name: "SystemRequest");

            migrationBuilder.DropTable(
                name: "TeamCategory");

            migrationBuilder.DropTable(
                name: "TeamObject");

            migrationBuilder.DropTable(
                name: "TeamUser");

            migrationBuilder.DropTable(
                name: "TimesheetDetail");

            migrationBuilder.DropTable(
                name: "TimesheetReport");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSetting");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "CalendarObject");

            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "Leave");

            migrationBuilder.DropTable(
                name: "LeaveRequest");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OvertimeRequest");

            migrationBuilder.DropTable(
                name: "ProductRequest");

            migrationBuilder.DropTable(
                name: "ProductItem");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "FingerPrinter");

            migrationBuilder.DropTable(
                name: "SwapDay");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Timesheet");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "ProductType");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WfhRequest");
        }
    }
}
