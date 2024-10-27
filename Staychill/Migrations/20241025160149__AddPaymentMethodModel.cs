using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddPaymentMethodModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentDB",
                columns: table => new
                {
                    PaymentmethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankTransferId = table.Column<int>(type: "int", nullable: true),
                    CreditCardId = table.Column<int>(type: "int", nullable: true),
                    QRDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDB", x => x.PaymentmethodId);
                    table.ForeignKey(
                        name: "FK_PaymentDB_BankTransferDB_BankTransferId",
                        column: x => x.BankTransferId,
                        principalTable: "BankTransferDB",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentDB_CreditCardsDB_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "CreditCardsDB",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentDB_QRDataDB_QRDataId",
                        column: x => x.QRDataId,
                        principalTable: "QRDataDB",
                        principalColumn: "Id");
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDB");
        }
    }
}
