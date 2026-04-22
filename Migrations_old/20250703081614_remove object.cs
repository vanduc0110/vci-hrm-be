using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class removeobject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamObject");

            migrationBuilder.DropColumn(
                name: "TimesheetObjectId",
                table: "TimesheetDetail");

            migrationBuilder.DropColumn(
                name: "SummerLeave",
                table: "LeaveHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "SystemRequest",
                type: "int",
                nullable: false,
                comment: "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 TakeBackLeave",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 DefineSummerLeave, 5 TakeBackLeave");

            migrationBuilder.AddColumn<string>(
                name: "Warranty",
                table: "Product",
                type: "json",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "LeaveRequest",
                type: "int",
                nullable: false,
                comment: "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Annual, 7 Unpaid",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Compensatory, 7 SummerVacation, 8 Annual, 9 Unpaid");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "LeaveHistory",
                type: "int",
                nullable: false,
                comment: "0 AddAnnualLeave, 1 TakeBackAnnualLeave, 2 UsingAnnualLeave",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 AddAnnualLeave, 1 AddSummerLeave, 2 AddCompensatoryLeave, 3 TakeBackAnnualLeave, 4 TakeBackSummerLeave, 5 TakeBackCompensatoryLeave, 6 UsingAnnualLeave, 7 UsingSummerLeave, 8 UsingCompensatoryLeave");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Leave",
                type: "int",
                nullable: false,
                comment: "6 Annual",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "6 Compensatory, 7 SummerVacation, 8 Annual");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Warranty",
                table: "Product");

            migrationBuilder.AddColumn<long>(
                name: "TimesheetObjectId",
                table: "TimesheetDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "SystemRequest",
                type: "int",
                nullable: false,
                comment: "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 DefineSummerLeave, 5 TakeBackLeave",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 TakeBackLeave");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "LeaveRequest",
                type: "int",
                nullable: false,
                comment: "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Compensatory, 7 SummerVacation, 8 Annual, 9 Unpaid",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Annual, 7 Unpaid");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "LeaveHistory",
                type: "int",
                nullable: false,
                comment: "0 AddAnnualLeave, 1 AddSummerLeave, 2 AddCompensatoryLeave, 3 TakeBackAnnualLeave, 4 TakeBackSummerLeave, 5 TakeBackCompensatoryLeave, 6 UsingAnnualLeave, 7 UsingSummerLeave, 8 UsingCompensatoryLeave",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 AddAnnualLeave, 1 TakeBackAnnualLeave, 2 UsingAnnualLeave");

            migrationBuilder.AddColumn<double>(
                name: "SummerLeave",
                table: "LeaveHistory",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Leave",
                type: "int",
                nullable: false,
                comment: "6 Compensatory, 7 SummerVacation, 8 Annual",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "6 Annual");

            migrationBuilder.CreateTable(
                name: "TeamObject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsUsing = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
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

            migrationBuilder.CreateIndex(
                name: "fk_timesheet_objects_teams_idx",
                table: "TeamObject",
                columns: new[] { "TeamId", "Name" },
                unique: true);
        }
    }
}
