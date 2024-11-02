using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _changeusernametouserIdontracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_UserDB_UserName",
                table: "TrackingDB");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserDB_Username",
                table: "UserDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_UserName",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TrackingDB");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TrackingDB",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_UserId",
                table: "TrackingDB",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_UserDB_UserId",
                table: "TrackingDB",
                column: "UserId",
                principalTable: "UserDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_UserDB_UserId",
                table: "TrackingDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_UserId",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TrackingDB");

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
    }
}
