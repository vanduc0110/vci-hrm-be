using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class updatetblclient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Client",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Client",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Client",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Client",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Client");
        }
    }
}
