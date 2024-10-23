using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _makemultipleselected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartId1",
                table: "TrackingDB");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_RetainCartDB_CartId",
                table: "TrackingDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_CartId1",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "CartId1",
                table: "TrackingDB");

            migrationBuilder.AddColumn<int>(
                name: "TrackingId",
                table: "RetainCartDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RetainCartDB_TrackingId",
                table: "RetainCartDB",
                column: "TrackingId");

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB",
                column: "TrackingId",
                principalTable: "TrackingDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_CartDB_CartId",
                table: "TrackingDB",
                column: "CartId",
                principalTable: "CartDB",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartId",
                table: "TrackingDB");

            migrationBuilder.DropIndex(
                name: "IX_RetainCartDB_TrackingId",
                table: "RetainCartDB");

            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "RetainCartDB");

            migrationBuilder.AddColumn<int>(
                name: "CartId1",
                table: "TrackingDB",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_CartId1",
                table: "TrackingDB",
                column: "CartId1",
                unique: true,
                filter: "[CartId1] IS NOT NULL");

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
    }
}
