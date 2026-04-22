using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    /// <inheritdoc />
    public partial class fixhrm2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "boolean",
                nullable: false,
                comment: "0 Inactive, 1 Active",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "0 Inactive, 1 Active");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "NotificationAssign",
                type: "boolean",
                nullable: false,
                comment: "0 unread, 1 read",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "0 unread, 1 read");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsActive",
                table: "User",
                type: "integer",
                nullable: false,
                comment: "0 Inactive, 1 Active",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "0 Inactive, 1 Active");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "NotificationAssign",
                type: "integer",
                nullable: false,
                comment: "0 unread, 1 read",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "0 unread, 1 read");
        }
    }
}
