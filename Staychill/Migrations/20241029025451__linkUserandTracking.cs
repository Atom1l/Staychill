using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _linkUserandTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserDB",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TrackingDB",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserDB_Username",
                table: "UserDB",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_UserDB_Username",
                table: "UserDB",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_UserName",
                table: "TrackingDB",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_UserDB_UserName",
                table: "TrackingDB",
                column: "UserName",
                principalTable: "UserDB",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_UserDB_UserName",
                table: "TrackingDB");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserDB_Username",
                table: "UserDB");

            migrationBuilder.DropIndex(
                name: "IX_UserDB_Username",
                table: "UserDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_UserName",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TrackingDB");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserDB",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
