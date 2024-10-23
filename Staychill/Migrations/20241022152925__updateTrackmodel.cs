using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _updateTrackmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartId",
                table: "TrackingDB");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "TrackingDB",
                newName: "CartitemsId");

            migrationBuilder.RenameIndex(
                name: "IX_TrackingDB_CartId",
                table: "TrackingDB",
                newName: "IX_TrackingDB_CartitemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_CartDB_CartitemsId",
                table: "TrackingDB",
                column: "CartitemsId",
                principalTable: "CartDB",
                principalColumn: "CartitemsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_CartDB_CartitemsId",
                table: "TrackingDB");

            migrationBuilder.RenameColumn(
                name: "CartitemsId",
                table: "TrackingDB",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_TrackingDB_CartitemsId",
                table: "TrackingDB",
                newName: "IX_TrackingDB_CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_CartDB_CartitemsId",
                table: "TrackingDB",
                column: "CartitemsId",
                principalTable: "CartDB",
                principalColumn: "CartitemsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
