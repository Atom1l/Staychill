using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _updateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorStockDB_ProductDB_ProductId",
                table: "ColorStockDB");

            migrationBuilder.DropTable(
                name: "ColorStockViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ColorStockDB",
                table: "ColorStockDB");

            migrationBuilder.RenameTable(
                name: "ColorStockDB",
                newName: "ColorStock");

            migrationBuilder.RenameIndex(
                name: "IX_ColorStockDB_ProductId",
                table: "ColorStock",
                newName: "IX_ColorStock_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "ProductViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductViewModelId",
                table: "ColorStock",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ColorStock",
                table: "ColorStock",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ColorStock_ProductViewModelId",
                table: "ColorStock",
                column: "ProductViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorStock_ProductDB_ProductId",
                table: "ColorStock",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ColorStock_ProductViewModel_ProductViewModelId",
                table: "ColorStock",
                column: "ProductViewModelId",
                principalTable: "ProductViewModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorStock_ProductDB_ProductId",
                table: "ColorStock");

            migrationBuilder.DropForeignKey(
                name: "FK_ColorStock_ProductViewModel_ProductViewModelId",
                table: "ColorStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ColorStock",
                table: "ColorStock");

            migrationBuilder.DropIndex(
                name: "IX_ColorStock_ProductViewModelId",
                table: "ColorStock");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "ProductViewModel");

            migrationBuilder.DropColumn(
                name: "ProductViewModelId",
                table: "ColorStock");

            migrationBuilder.RenameTable(
                name: "ColorStock",
                newName: "ColorStockDB");

            migrationBuilder.RenameIndex(
                name: "IX_ColorStock_ProductId",
                table: "ColorStockDB",
                newName: "IX_ColorStockDB_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ColorStockDB",
                table: "ColorStockDB",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ColorStockViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductViewModelId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorStockViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColorStockViewModel_ProductViewModel_ProductViewModelId",
                        column: x => x.ProductViewModelId,
                        principalTable: "ProductViewModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorStockViewModel_ProductViewModelId",
                table: "ColorStockViewModel",
                column: "ProductViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorStockDB_ProductDB_ProductId",
                table: "ColorStockDB",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
