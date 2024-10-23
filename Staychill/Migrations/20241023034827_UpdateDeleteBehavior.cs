using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_ProductDB_ProductId",
                table: "RetainCartDB");

            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB");

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_ProductDB_ProductId",
                table: "RetainCartDB",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB",
                column: "TrackingId",
                principalTable: "TrackingDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_ProductDB_ProductId",
                table: "RetainCartDB");

            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB");

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_ProductDB_ProductId",
                table: "RetainCartDB",
                column: "ProductId",
                principalTable: "ProductDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB",
                column: "TrackingId",
                principalTable: "TrackingDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
