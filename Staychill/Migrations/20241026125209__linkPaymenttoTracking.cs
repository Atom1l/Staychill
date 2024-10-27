using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _linkPaymenttoTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentmethodId",
                table: "TrackingDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "UserUploadedData",
                table: "QRDataDB",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_PaymentmethodId",
                table: "TrackingDB",
                column: "PaymentmethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDB_PaymentDB_PaymentmethodId",
                table: "TrackingDB",
                column: "PaymentmethodId",
                principalTable: "PaymentDB",
                principalColumn: "PaymentmethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDB_PaymentDB_PaymentmethodId",
                table: "TrackingDB");

            migrationBuilder.DropIndex(
                name: "IX_TrackingDB_PaymentmethodId",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "PaymentmethodId",
                table: "TrackingDB");

            migrationBuilder.DropColumn(
                name: "UserUploadedData",
                table: "QRDataDB");
        }
    }
}
