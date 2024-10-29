using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _linkrelationwithPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDB_BankTransferDB_BankTransferId",
                table: "PaymentDB");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDB_CreditCardsDB_CreditCardId",
                table: "PaymentDB");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDB_QRDataDB_QRDataId",
                table: "PaymentDB");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDB_BankTransferId",
                table: "PaymentDB");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDB_CreditCardId",
                table: "PaymentDB");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDB_QRDataId",
                table: "PaymentDB");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "QRDataDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "CreditCardsDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "BankTransferDB",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QRDataDB_PaymentMethodId",
                table: "QRDataDB",
                column: "PaymentMethodId",
                unique: true,
                filter: "[PaymentMethodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardsDB_PaymentMethodId",
                table: "CreditCardsDB",
                column: "PaymentMethodId",
                unique: true,
                filter: "[PaymentMethodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransferDB_PaymentMethodId",
                table: "BankTransferDB",
                column: "PaymentMethodId",
                unique: true,
                filter: "[PaymentMethodId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransferDB_PaymentDB_PaymentMethodId",
                table: "BankTransferDB",
                column: "PaymentMethodId",
                principalTable: "PaymentDB",
                principalColumn: "PaymentmethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCardsDB_PaymentDB_PaymentMethodId",
                table: "CreditCardsDB",
                column: "PaymentMethodId",
                principalTable: "PaymentDB",
                principalColumn: "PaymentmethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QRDataDB_PaymentDB_PaymentMethodId",
                table: "QRDataDB",
                column: "PaymentMethodId",
                principalTable: "PaymentDB",
                principalColumn: "PaymentmethodId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransferDB_PaymentDB_PaymentMethodId",
                table: "BankTransferDB");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCardsDB_PaymentDB_PaymentMethodId",
                table: "CreditCardsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_QRDataDB_PaymentDB_PaymentMethodId",
                table: "QRDataDB");

            migrationBuilder.DropIndex(
                name: "IX_QRDataDB_PaymentMethodId",
                table: "QRDataDB");

            migrationBuilder.DropIndex(
                name: "IX_CreditCardsDB_PaymentMethodId",
                table: "CreditCardsDB");

            migrationBuilder.DropIndex(
                name: "IX_BankTransferDB_PaymentMethodId",
                table: "BankTransferDB");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "QRDataDB");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "CreditCardsDB");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "BankTransferDB");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDB_BankTransferId",
                table: "PaymentDB",
                column: "BankTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDB_CreditCardId",
                table: "PaymentDB",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDB_QRDataId",
                table: "PaymentDB",
                column: "QRDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDB_BankTransferDB_BankTransferId",
                table: "PaymentDB",
                column: "BankTransferId",
                principalTable: "BankTransferDB",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDB_CreditCardsDB_CreditCardId",
                table: "PaymentDB",
                column: "CreditCardId",
                principalTable: "CreditCardsDB",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDB_QRDataDB_QRDataId",
                table: "PaymentDB",
                column: "QRDataId",
                principalTable: "QRDataDB",
                principalColumn: "Id");
        }
    }
}
