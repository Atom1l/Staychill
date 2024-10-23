using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddRetainCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartId",
                table: "TrackingDB");

            migrationBuilder.AddColumn<int>(
                name: "CartId1",
                table: "TrackingDB",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RetainCartDB",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetainCartDB", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_RetainCartDB_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_CartId1",
                table: "TrackingDB",
                column: "CartId1",
                unique: true,
                filter: "[CartId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RetainCartDB_ProductId",
                table: "RetainCartDB",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_CartDB_CartId1",
                table: "TrackingDB",
                column: "CartId1",
                principalTable: "CartDB",
                principalColumn: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_RetainCartDB_CartId",
                table: "TrackingDB",
                column: "CartId",
                principalTable: "RetainCartDB",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartId1",
                table: "TrackingDB");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_RetainCartDB_CartId",
                table: "TrackingDB");

            migrationBuilder.DropTable(
                name: "RetainCartDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_CartId1",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "CartId1",
                table: "TrackingDB");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_CartDB_CartId",
                table: "TrackingDB",
                column: "CartId",
                principalTable: "CartDB",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
