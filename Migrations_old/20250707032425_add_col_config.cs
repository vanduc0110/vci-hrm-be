using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class add_col_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsing",
                table: "Config",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsing",
                table: "Config");
        }
    }
}
