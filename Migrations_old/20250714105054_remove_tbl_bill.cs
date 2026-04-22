using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class remove_tbl_bill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_item_history",
                table: "ProductHistory");

            migrationBuilder.DropTable(
                name: "BillDetail");

            migrationBuilder.DropTable(
                name: "ProductRequestDetail");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "ProductRequest");

            migrationBuilder.DropIndex(
                name: "fk_product_item_bill_id_idx",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "ProductItem");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ProductItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0 InStock, 1 Using, 2 OutStock");

            migrationBuilder.AddColumn<string>(
                name: "SerialCode",
                table: "ProductItem",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "ProductItem",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Business",
                table: "ProductHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialCode",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "Business",
                table: "ProductHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ProductItem",
                type: "int",
                nullable: false,
                comment: "0 InStock, 1 Using, 2 OutStock",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "BillId",
                table: "ProductItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "ProductItem",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    BillNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CurrencyUnit = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "double", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0 StockIn, 1 StockOut")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Reason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reviewer = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 pending, 1 approverd, 2 rejected, 3 processed"),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRequest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "BillDetail",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.ProductId, x.BillId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_bill_details",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_bill_details_product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductRequestDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductItemId = table.Column<long>(type: "bigint", nullable: false),
                    ProductRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_request_details",
                        column: x => x.ProductRequestId,
                        principalTable: "ProductRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductRequestDetail_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_product_item_bill_id_idx",
                table: "ProductItem",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "fk_bill_no_idx",
                table: "Bill",
                column: "BillNo");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetail_BillId",
                table: "BillDetail",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequest_UserId",
                table: "ProductRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequestDetail_ProductItemId",
                table: "ProductRequestDetail",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequestDetail_ProductRequestId",
                table: "ProductRequestDetail",
                column: "ProductRequestId");

            migrationBuilder.AddForeignKey(
                name: "fk_product_item_history",
                table: "ProductHistory",
                column: "ItemId",
                principalTable: "ProductItem",
                principalColumn: "Id");
        }
    }
}
