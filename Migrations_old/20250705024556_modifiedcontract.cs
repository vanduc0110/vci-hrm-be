using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class modifiedcontract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "ProjectContract",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ProjectContract",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ProjectContract");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ProjectContract");
        }
    }
}
