using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    /// <inheritdoc />
    public partial class salaryapproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApprovedBy",
                table: "Salaries",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "Salaries",
                type: "timestamp with time zone",
                nullable: true);

            // Các cấu hình lương đã tồn tại → tự động duyệt (dùng CreatedBy/CreatedDate)
            migrationBuilder.Sql(
                @"UPDATE ""Salaries"" SET ""ApprovedBy"" = ""CreatedBy"", ""ApprovedDate"" = ""CreatedDate"" WHERE ""IsActive"" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "Salaries");
        }
    }
}
