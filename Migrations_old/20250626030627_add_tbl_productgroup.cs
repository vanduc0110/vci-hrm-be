using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDesign.API.Migrations
{
  public partial class add_tbl_productgroup : Migration
  {
    protected override void Up( MigrationBuilder migrationBuilder )
    {
      migrationBuilder.AddColumn<string>(
          name: "Code",
          table: "ProductType",
          type: "longtext",
          nullable: false,
          collation: "utf8mb4_0900_ai_ci" )
          .Annotation( "MySql:CharSet", "utf8mb4" );

      migrationBuilder.AddColumn<long>(
          name: "ProductGroupId",
          table: "ProductType",
          type: "bigint",
          nullable: false,
          defaultValue: 0L );

      migrationBuilder.CreateTable(
          name: "ProductGroup",
          columns: table => new
          {
            Id = table.Column<long>( type: "bigint", nullable: false )
                  .Annotation( "MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn ),
            Name = table.Column<string>( type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci" )
                  .Annotation( "MySql:CharSet", "utf8mb4" ),
            Code = table.Column<string>( type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci" )
                  .Annotation( "MySql:CharSet", "utf8mb4" ),
            ParantId = table.Column<long>( type: "bigint", nullable: true ),
            CreatedBy = table.Column<long>( type: "bigint", nullable: false ),
            CreatedDate = table.Column<DateTime>( type: "datetime(6)", nullable: false ),
            ModifiedBy = table.Column<long>( type: "bigint", nullable: false ),
            ModifiedDate = table.Column<DateTime>( type: "datetime(6)", nullable: false )
          },
          constraints: table =>
          {
            table.PrimaryKey( "PK_ProductGroup", x => x.Id );
          } )
          .Annotation( "MySql:CharSet", "utf8mb4" )
          .Annotation( "Relational:Collation", "utf8mb4_0900_ai_ci" );

      migrationBuilder.CreateIndex(
          name: "IX_ProductType_ProductGroupId",
          table: "ProductType",
          column: "ProductGroupId" );

      migrationBuilder.AddForeignKey(
          name: "fk_product_group_product_type",
          table: "ProductType",
          column: "ProductGroupId",
          principalTable: "ProductGroup",
          principalColumn: "Id" );
    }

    protected override void Down( MigrationBuilder migrationBuilder )
    {
      migrationBuilder.DropForeignKey(
          name: "fk_product_group_product_type",
          table: "ProductType" );

      migrationBuilder.DropTable(
          name: "ProductGroup" );

      migrationBuilder.DropIndex(
          name: "IX_ProductType_ProductGroupId",
          table: "ProductType" );

      migrationBuilder.DropColumn(
          name: "Code",
          table: "ProductType" );

      migrationBuilder.DropColumn(
          name: "ProductGroupId",
          table: "ProductType" );
    }
  }
}
