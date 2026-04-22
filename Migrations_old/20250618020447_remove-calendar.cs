using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class removecalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "CalendarObject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarObject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 Notice, 1 Meeting Room, 2 HiEnd PC")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarObject", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    RepeatId = table.Column<long>(type: "bigint", nullable: false),
                    RepeatType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, comment: "None, EveryDay, EveryWeek, EveryMonth", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
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

            migrationBuilder.CreateIndex(
                name: "fk_calendar_user_idx",
                table: "Calendar",
                columns: new[] { "CreatedBy", "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_ObjectId",
                table: "Calendar",
                column: "ObjectId");
        }
    }
}
