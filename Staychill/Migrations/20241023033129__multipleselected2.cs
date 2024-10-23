using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _multipleselected2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartId",
                table: "TrackingDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_CartId",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "TrackingDB");

            migrationBuilder.AddColumn<int>(
                name: "TrackingId",
                table: "CartDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartDB_TrackingId",
                table: "CartDB",
                column: "TrackingId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDB_TrackingDB_TrackingId",
                table: "CartDB",
                column: "TrackingId",
                principalTable: "TrackingDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDB_TrackingDB_TrackingId",
                table: "CartDB");

            migrationBuilder.DropIndex(
                name: "IX_CartDB_TrackingId",
                table: "CartDB");

            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "CartDB");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "TrackingDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_CartId",
                table: "TrackingDB",
                column: "CartId",
                unique: true);

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
