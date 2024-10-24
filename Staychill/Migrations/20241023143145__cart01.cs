using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _cart01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDB_ProductDB_ProductId",
                table: "CartDB");

            migrationBuilder.DropIndex(
                name: "IX_CartDB_ProductId",
                table: "CartDB");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartDB");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CartDB");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDB");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItem_CartDB_CartId",
                        column: x => x.CartId,
                        principalTable: "CartDB",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductId",
                table: "CartItem",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "ShipmentViewModel");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CartDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CartDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "UnitPrice",
                table: "CartDB",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_CartDB_ProductId",
                table: "CartDB",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDB_ProductDB_ProductId",
                table: "CartDB",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
