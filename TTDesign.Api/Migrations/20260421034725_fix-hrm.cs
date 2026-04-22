using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    /// <inheritdoc />
    public partial class fixhrm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmailNotification",
                table: "UserSetting",
                type: "integer",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<int>(
                name: "IsActive",
                table: "User",
                type: "integer",
                nullable: false,
                comment: "0 Inactive, 1 Active",
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldComment: "0 Inactive, 1 Active");

            migrationBuilder.AlterColumn<int>(
                name: "IsPublic",
                table: "Project",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "NotificationAssign",
                type: "integer",
                nullable: false,
                comment: "0 unread, 1 read",
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldComment: "0 unread, 1 read");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EmailNotification",
                table: "UserSetting",
                type: "numeric(20,0)",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<decimal>(
                name: "IsActive",
                table: "User",
                type: "numeric(20,0)",
                nullable: false,
                comment: "0 Inactive, 1 Active",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "0 Inactive, 1 Active");

            migrationBuilder.AlterColumn<decimal>(
                name: "IsPublic",
                table: "Project",
                type: "numeric(20,0)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Status",
                table: "NotificationAssign",
                type: "numeric(20,0)",
                nullable: false,
                comment: "0 unread, 1 read",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "0 unread, 1 read");
        }
    }
}
