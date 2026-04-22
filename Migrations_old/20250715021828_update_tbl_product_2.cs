using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class update_tbl_product_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBuy",
                table: "Product");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Product",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Product");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateBuy",
                table: "Product",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
