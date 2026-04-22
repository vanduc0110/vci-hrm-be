using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class add_col_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TeamCategory",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TeamCategory");
        }
    }
}
