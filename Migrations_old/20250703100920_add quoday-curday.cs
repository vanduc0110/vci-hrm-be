using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class addquodaycurday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "User",
                type: "int",
                maxLength: 30,
                nullable: false,
                comment: "0 System, 1 Director, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 30,
                oldComment: "0 System, 1 GM, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership");

            migrationBuilder.AddColumn<int>(
                name: "CurrentDay",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuotationDay",
                table: "Project",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentDay",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "QuotationDay",
                table: "Project");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "User",
                type: "int",
                maxLength: 30,
                nullable: false,
                comment: "0 System, 1 GM, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 30,
                oldComment: "0 System, 1 Director, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership");
        }
    }
}
