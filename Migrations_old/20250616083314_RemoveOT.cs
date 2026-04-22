using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class RemoveOT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OvertimeRequestDetail");

            migrationBuilder.DropTable(
                name: "OvertimeRule");

            migrationBuilder.DropTable(
                name: "OvertimeSummary");

            migrationBuilder.DropTable(
                name: "OvertimeRequest");

            migrationBuilder.DropColumn(
                name: "OvertimeHours",
                table: "TimesheetReport");

            migrationBuilder.DropColumn(
                name: "OvertimeHour",
                table: "Project");

            migrationBuilder.RenameIndex(
                name: "fk_leave_request_created_by_idx2",
                table: "WfhRequest",
                newName: "fk_leave_request_created_by_idx1");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "TimesheetDetail",
                type: "int",
                nullable: false,
                comment: "0 Project, 1 UnpaidLeave, 2 PaidLeave",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 Project, 1 Overtime, 2 UnpaidLeave, 3 PaidLeave");

            migrationBuilder.AlterColumn<long>(
                name: "ReferenceId",
                table: "TimesheetDetail",
                type: "bigint",
                nullable: false,
                comment: "reference to leave_request, holiday_id",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "reference to overtime_request, leave_request, holiday_id");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectType",
                table: "Notification",
                type: "int",
                nullable: false,
                comment: "0 Notification, 1 LeaveRequest, 2 WfhRequest, 3 NoticeInvite, 4 AssetInvite",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 Notification, 1 LeaveRequest, 2 OvertimeRequest, 3 WfhRequest, 4 NoticeInvite, 5 AssetInvite");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "fk_leave_request_created_by_idx1",
                table: "WfhRequest",
                newName: "fk_leave_request_created_by_idx2");

            migrationBuilder.AddColumn<double>(
                name: "OvertimeHours",
                table: "TimesheetReport",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "TimesheetDetail",
                type: "int",
                nullable: false,
                comment: "0 Project, 1 Overtime, 2 UnpaidLeave, 3 PaidLeave",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 Project, 1 UnpaidLeave, 2 PaidLeave");

            migrationBuilder.AlterColumn<long>(
                name: "ReferenceId",
                table: "TimesheetDetail",
                type: "bigint",
                nullable: false,
                comment: "reference to overtime_request, leave_request, holiday_id",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "reference to leave_request, holiday_id");

            migrationBuilder.AddColumn<double>(
                name: "OvertimeHour",
                table: "Project",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "ObjectType",
                table: "Notification",
                type: "int",
                nullable: false,
                comment: "0 Notification, 1 LeaveRequest, 2 OvertimeRequest, 3 WfhRequest, 4 NoticeInvite, 5 AssetInvite",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 Notification, 1 LeaveRequest, 2 WfhRequest, 3 NoticeInvite, 4 AssetInvite");

            migrationBuilder.CreateTable(
                name: "OvertimeRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValueSql: "b'0'", comment: "0 Pending, 1 Approve, 2 Reject, 3 Calculated")
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
                name: "OvertimeRule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HourEnd = table.Column<double>(type: "double", nullable: true),
                    HourStart = table.Column<double>(type: "double", nullable: true),
                    Multiplier = table.Column<double>(type: "double", nullable: false, comment: "hệ số nhân"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Weekday, 1 Weekend, 2 OverNight, 3 Holiday"),
                    Weekday = table.Column<int>(type: "int", nullable: true, comment: "0 = Monday, 1 = Tuesday, 2 = Wednesday, 3 = Thursday, 4 = Friday, 5 = Saturday, 6 = Sunday")
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
                    HourHoliday = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourHolidayToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    HourOvernight = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourOvernightToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    HourWeekday = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourWeekdayToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    HourWeekend = table.Column<double>(type: "double", nullable: false, comment: "tổng hợp giờ overtime theo type, chưa nhân hệ số"),
                    HourWeekendToCompensatory = table.Column<double>(type: "double", nullable: false, comment: "tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số"),
                    Month = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeSummary", x => x.Id);
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
                    ActualHour = table.Column<double>(type: "double", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    End = table.Column<DateTime>(type: "datetime", nullable: false),
                    Multiplier = table.Column<double>(type: "double", nullable: false, comment: "hệ số tại thời điểm approve"),
                    Paid = table.Column<int>(type: "int", nullable: false, comment: "0 Salary, 1 Compensatory"),
                    Start = table.Column<DateTime>(type: "datetime", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Weekday, 1 Weekend, 2 OverNight, 3 Holiday")
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
        }
    }
}
