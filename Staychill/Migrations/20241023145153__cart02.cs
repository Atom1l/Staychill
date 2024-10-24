using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _cart02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_CartDB_CartId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_ProductDB_ProductId",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "CartitemsDB");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_ProductId",
                table: "CartitemsDB",
                newName: "IX_CartitemsDB_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_CartId",
                table: "CartitemsDB",
                newName: "IX_CartitemsDB_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartitemsDB",
                table: "CartitemsDB",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartitemsDB_CartDB_CartId",
                table: "CartitemsDB",
                column: "CartId",
                principalTable: "CartDB",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartitemsDB_ProductDB_ProductId",
                table: "CartitemsDB",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartitemsDB_CartDB_CartId",
                table: "CartitemsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_CartitemsDB_ProductDB_ProductId",
                table: "CartitemsDB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartitemsDB",
                table: "CartitemsDB");

            migrationBuilder.RenameTable(
                name: "CartitemsDB",
                newName: "CartItem");

            migrationBuilder.RenameIndex(
                name: "IX_CartitemsDB_ProductId",
                table: "CartItem",
                newName: "IX_CartItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartitemsDB_CartId",
                table: "CartItem",
                newName: "IX_CartItem_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_CartDB_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "CartDB",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_ProductDB_ProductId",
                table: "CartItem",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
