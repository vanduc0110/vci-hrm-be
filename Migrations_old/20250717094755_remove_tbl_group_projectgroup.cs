using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class remove_tbl_group_projectgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "ProjectGroup");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.CreateTable(
                name: "Allocation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductItemId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocation_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Allocation_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Allocation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_allcation_product_item",
                table: "Allocation",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "fk_allocation_product",
                table: "Allocation",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_UserId",
                table: "Allocation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allocation");

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.GroupId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_group_users_group",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_group_users_user",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProjectGroup",
                columns: table => new
                {
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.ProjectId, x.GroupId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_project_group_group",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "group_code_UNIQUE",
                table: "Group",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_group_users_group",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "fk_group_users_idx",
                table: "GroupUsers",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_project_idx",
                table: "ProjectGroup",
                columns: new[] { "GroupId", "ProjectId" },
                unique: true);
        }
    }
}
