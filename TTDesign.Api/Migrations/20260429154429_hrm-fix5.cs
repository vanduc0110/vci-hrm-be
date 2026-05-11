using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    /// <inheritdoc />
    public partial class hrmfix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Project",
                type: "integer",
                nullable: false,
                comment: "0 Pending, 1 Active, 2 End",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "0 Pending, 1 Active, 2 End");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Project",
                type: "boolean",
                nullable: false,
                comment: "0 Pending, 1 Active, 2 End",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "0 Pending, 1 Active, 2 End");
        }
    }
}
