using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class add_col_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Species",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Product",
                newName: "ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_TypeId",
                table: "Product",
                newName: "IX_Product_ProductTypeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsMangeCode",
                table: "ProductType",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWarranty",
                table: "ProductType",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Product",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateBuy",
                table: "Product",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Product",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "ManageId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Product",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Product",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Product",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMangeCode",
                table: "ProductType");

            migrationBuilder.DropColumn(
                name: "IsWarranty",
                table: "ProductType");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DateBuy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ManageId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                table: "Product",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProductTypeId",
                table: "Product",
                newName: "IX_Product_TypeId");

            migrationBuilder.AddColumn<int>(
                name: "Species",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "0 Hardware, 1 Software");
        }
    }
}
