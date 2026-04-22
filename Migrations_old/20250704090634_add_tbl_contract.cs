using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
    public partial class add_tbl_contract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_document",
                table: "ProjectDocument");

            migrationBuilder.DropColumn(
                name: "CurrentDay",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "QuotationDay",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectDocument",
                newName: "ContractId");

            migrationBuilder.RenameIndex(
                name: "fk_project_document_idx",
                table: "ProjectDocument",
                newName: "fk_projectcontract_document_idx");

            migrationBuilder.CreateTable(
                name: "ProjectContract",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectContract", x => x.Id);
                    table.ForeignKey(
                        name: "fk_project_project_contract",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_project_contract_idx",
                table: "ProjectContract",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "fk_projectcontract_projectdocument",
                table: "ProjectDocument",
                column: "ContractId",
                principalTable: "ProjectContract",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projectcontract_projectdocument",
                table: "ProjectDocument");

            migrationBuilder.DropTable(
                name: "ProjectContract");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "ProjectDocument",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "fk_projectcontract_document_idx",
                table: "ProjectDocument",
                newName: "fk_project_document_idx");

            migrationBuilder.AddColumn<int>(
                name: "CurrentDay",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuotationDay",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_project_document",
                table: "ProjectDocument",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }
    }
}
