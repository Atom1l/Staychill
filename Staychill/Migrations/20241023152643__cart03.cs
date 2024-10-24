using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _cart03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RetaincartsDB",
                columns: table => new
                {
                    ReCartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetaincartsDB", x => x.ReCartId);
                    table.ForeignKey(
                        name: "FK_RetaincartsDB_TrackingDB_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "TrackingDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetainCartItems",
                columns: table => new
                {
                    ReCartItemId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    RetainCartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetainCartItems", x => x.ReCartItemId);
                    table.ForeignKey(
                        name: "FK_RetainCartItems_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RetainCartItems_RetaincartsDB_ReCartItemId",
                        column: x => x.ReCartItemId,
                        principalTable: "RetaincartsDB",
                        principalColumn: "ReCartId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RetainCartItems_ProductId",
                table: "RetainCartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RetaincartsDB_TrackingId",
                table: "RetaincartsDB",
                column: "TrackingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetainCartItems");

            migrationBuilder.DropTable(
                name: "RetaincartsDB");
        }
    }
}
