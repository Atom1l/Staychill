using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _RemoveForeignKeyfromPaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankTransferId",
                table: "PaymentDB");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "PaymentDB");

            migrationBuilder.DropColumn(
                name: "QRDataId",
                table: "PaymentDB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankTransferId",
                table: "PaymentDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditCardId",
                table: "PaymentDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QRDataId",
                table: "PaymentDB",
                type: "int",
                nullable: true);
        }
    }
}
