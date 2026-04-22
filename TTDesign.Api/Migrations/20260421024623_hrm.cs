using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TTDesign.API.Migrations
{
    /// <inheritdoc />
    public partial class hrm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetCategory_AssetCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AssetCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToUserId = table.Column<string>(type: "text", nullable: true),
                    FromUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Supplier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ComponentType = table.Column<string>(type: "text", nullable: false),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    ComponentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Decription = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    IsUsing = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FingerDataMachines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SN = table.Column<string>(type: "text", nullable: false),
                    Table = table.Column<string>(type: "text", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    PunchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status1 = table.Column<int>(type: "integer", nullable: false),
                    VerifyType = table.Column<int>(type: "integer", nullable: false),
                    Status2 = table.Column<int>(type: "integer", nullable: false),
                    Status3 = table.Column<int>(type: "integer", nullable: false),
                    Status4 = table.Column<int>(type: "integer", nullable: false),
                    Status5 = table.Column<int>(type: "integer", nullable: false),
                    Status6 = table.Column<int>(type: "integer", nullable: false),
                    Status7 = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerDataMachines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FingerPrinterLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    DateIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateInUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateOutUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerPrinterLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleTimesheetKey",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KeyName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SpreadsheetId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientEmail = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PrivateKey = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    SheetId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleTimesheetKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "0 holiday, 1 special"),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "0 pending, 1 Apply, 2 Deleting"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leave",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "6 Annual"),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hours = table.Column<double>(type: "double precision", nullable: false),
                    Using = table.Column<double>(type: "double precision", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "0 AddAnnualLeave, 1 TakeBackAnnualLeave, 2 UsingAnnualLeave"),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AnnualLeave = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<double>(type: "double precision", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveInformation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    TypeName = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    LeaveDay = table.Column<string>(type: "text", nullable: false),
                    StartCondition = table.Column<string>(type: "text", nullable: false),
                    End = table.Column<string>(type: "text", nullable: false),
                    Block = table.Column<string>(type: "text", nullable: false),
                    Using = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Annual, 7 Unpaid"),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Hour = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "0 Pending, 1 Approve, 2 Reject"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "0 Information, 1 Approve, 2 Reject"),
                    ObjectType = table.Column<int>(type: "integer", nullable: false, comment: "0 Notification, 1 LeaveRequest, 2 WfhRequest, 3 NoticeInvite, 4 AssetInvite"),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SwapDay",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 TakeBackLeave"),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateStartWork = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<decimal>(type: "numeric(20,0)", nullable: false, comment: "0 Inactive, 1 Active"),
                    State = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "0", comment: "0 Available, 1 WFH, 2 Business, 3 Busy, 4 Unavailable"),
                    StaffId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Position = table.Column<int>(type: "integer", maxLength: 30, nullable: false, comment: "0 System, 1 Director, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership"),
                    Avatar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'male'"),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IssuedTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IssuedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SocialInsuranceBookNo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AboutMe = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FingerId = table.Column<int>(type: "integer", nullable: false),
                    BankName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccountBank = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Dependent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailNotification = table.Column<decimal>(type: "numeric(20,0)", nullable: false, defaultValueSql: "b'1'"),
                    Timezone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "'BangKok, Ha Noi, Jakarta (UTC +7)'"),
                    Language = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "'English'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WfhRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "b'0'", comment: "0 Pending, 1 Approve, 2 Reject"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfhRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PurchasePrice = table.Column<bool>(type: "boolean", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Supplier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AssetCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DetailedSpecs = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_AssetCategory_AssetCategoryId",
                        column: x => x.AssetCategoryId,
                        principalTable: "AssetCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectManagement = table.Column<string>(type: "text", nullable: false, comment: "PM"),
                    ClientId = table.Column<long>(type: "bigint", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "0 Pending, 1 Active, 2 End"),
                    IsPublic = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    ProjectNumber = table.Column<int>(type: "integer", nullable: false),
                    QuotationHour = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "T, I, H"),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FiscalYear = table.Column<int>(type: "integer", nullable: false),
                    WorkingHour = table.Column<double>(type: "double precision", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "fk_projects_client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HolidayApply",
                columns: table => new
                {
                    HolidayId = table.Column<long>(type: "bigint", nullable: false),
                    ApplyId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false, comment: "User, Team")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.HolidayId, x.ApplyId });
                    table.ForeignKey(
                        name: "fk_holiday_user",
                        column: x => x.HolidayId,
                        principalTable: "Holiday",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveHistoryUsing",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveId = table.Column<long>(type: "bigint", nullable: false),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Hours = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveHistoryUsing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveHistoryUsing_Leave_LeaveId",
                        column: x => x.LeaveId,
                        principalTable: "Leave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_leave_history_using_leave_request",
                        column: x => x.LeaveRequestId,
                        principalTable: "LeaveRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequestDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hours = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_leave_request_detail",
                        column: x => x.LeaveRequestId,
                        principalTable: "LeaveRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotificationAssign",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<decimal>(type: "numeric(20,0)", nullable: false, comment: "0 unread, 1 read"),
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
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "SwapDayUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SwapDayId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapDayUser", x => x.Id);
                    table.ForeignKey(
                        name: "fk_swap_day",
                        column: x => x.SwapDayId,
                        principalTable: "SwapDay",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsUsing = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamCategory", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_categories_team",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamUser",
                columns: table => new
                {
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.TeamId });
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
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
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
                });

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
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Timesheet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LockBy = table.Column<long>(type: "bigint", nullable: true),
                    HolidayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SwapDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WfhRequestId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AssetAllocation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EventType = table.Column<int>(type: "integer", nullable: true),
                    OldUserId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentUserId = table.Column<long>(type: "bigint", nullable: true),
                    StatusNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    AssetStatus = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAllocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetAllocation_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetComponents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    ComponentId = table.Column<long>(type: "bigint", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetComponents_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectContract",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectContract", x => x.Id);
                    table.ForeignKey(
                        name: "fk_project_project_contract",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.ProjectId });
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
                });

            migrationBuilder.CreateTable(
                name: "FingerPrinter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    DateIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HourTotal = table.Column<double>(type: "double precision", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerPrinter", x => x.Id);
                    table.ForeignKey(
                        name: "fk_fingerprint_timesheet",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimesheetDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "0 Project, 1 UnpaidLeave, 2 PaidLeave"),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: false, comment: "reference to leave_request, holiday_id"),
                    TimesheetCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Hours = table.Column<double>(type: "double precision", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_detail_timesheet",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimesheetReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimesheetId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    Hours = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetReport", x => x.Id);
                    table.ForeignKey(
                        name: "fk_timesheet_report",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocument", x => x.Id);
                    table.ForeignKey(
                        name: "fk_projectcontract_projectdocument",
                        column: x => x.ContractId,
                        principalTable: "ProjectContract",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SwapDayRefer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HourTotal = table.Column<double>(type: "double precision", nullable: false),
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
                });

            migrationBuilder.CreateIndex(
                name: "asset_code_UNIQUE",
                table: "Asset",
                column: "AssetCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetCategoryId",
                table: "Asset",
                column: "AssetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAllocation_AssetId",
                table: "AssetAllocation",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "asset_category_name_UNIQUE",
                table: "AssetCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetCategory_ParentId",
                table: "AssetCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetComponents_AssetId",
                table: "AssetComponents",
                column: "AssetId");

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
                name: "fk_project_contract_idx",
                table: "ProjectContract",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "fk_projectcontract_document_idx",
                table: "ProjectDocument",
                column: "ContractId");

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
                column: "UserId");

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
                name: "fk_leave_request_created_by_idx1",
                table: "WfhRequest",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetAllocation");

            migrationBuilder.DropTable(
                name: "AssetComponents");

            migrationBuilder.DropTable(
                name: "AssetHistories");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "FingerDataMachines");

            migrationBuilder.DropTable(
                name: "FingerPrinterLogs");

            migrationBuilder.DropTable(
                name: "GoogleTimesheetKey");

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
                name: "ProjectDocument");

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
                name: "UserReports");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSetting");

            migrationBuilder.DropTable(
                name: "UserTask");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "Leave");

            migrationBuilder.DropTable(
                name: "LeaveRequest");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ProjectContract");

            migrationBuilder.DropTable(
                name: "FingerPrinter");

            migrationBuilder.DropTable(
                name: "SwapDay");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "AssetCategory");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Timesheet");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WfhRequest");
        }
    }
}
