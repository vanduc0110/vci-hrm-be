using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class Update_tbl_teamuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserId",
                table: "TeamUser");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "TeamUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserId",
                table: "TeamUser");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "TeamUser",
                column: "UserId",
                unique: true);
        }
    }
}
