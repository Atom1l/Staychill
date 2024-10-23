using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddCarttoTracking01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
