using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _seperatethecartandretain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB");

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB",
                column: "TrackingId",
                principalTable: "TrackingDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB");

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartDB_TrackingDB_TrackingId",
                table: "RetainCartDB",
                column: "TrackingId",
                principalTable: "TrackingDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
