using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class add_col_userinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountBank",
                table: "UserInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "UserInfo",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Dependent",
                table: "UserInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBank",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "Dependent",
                table: "UserInfo");
        }
    }
}
