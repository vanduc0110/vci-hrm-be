using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class removetypecompensatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompensatoryLeave",
                table: "LeaveHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CompensatoryLeave",
                table: "LeaveHistory",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
