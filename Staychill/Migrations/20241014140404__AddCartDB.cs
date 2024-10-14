using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddCartDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccDB_PaymethodDB_PaymethodId",
                table: "BankAccDB");

            migrationBuilder.DropTable(
                name: "BankDB");

            migrationBuilder.DropTable(
                name: "CreditCardDB");

            migrationBuilder.DropTable(
                name: "PaymethodDB");

            migrationBuilder.DropTable(
                name: "QRDB");

            migrationBuilder.DropTable(
                name: "PaymentDB");

            migrationBuilder.DropIndex(
                name: "IX_BankAccDB_PaymethodId",
                table: "BankAccDB");

            migrationBuilder.DropColumn(
                name: "PaymethodId",
                table: "BankAccDB");

            migrationBuilder.CreateTable(
                name: "CartDB",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDB", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_CartDB_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDB_ProductId",
                table: "CartDB",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDB");

            migrationBuilder.AddColumn<int>(
                name: "PaymethodId",
                table: "BankAccDB",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymethodDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymethodDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    BankImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDB_PaymentDB_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "PaymentDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiredDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCardDB_PaymentDB_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "PaymentDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    QRImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QRDB_PaymentDB_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "PaymentDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccDB_PaymethodId",
                table: "BankAccDB",
                column: "PaymethodId",
                unique: true,
                filter: "[PaymethodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BankDB_PaymentId",
                table: "BankDB",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardDB_PaymentId",
                table: "CreditCardDB",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QRDB_PaymentId",
                table: "QRDB",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccDB_PaymethodDB_PaymethodId",
                table: "BankAccDB",
                column: "PaymethodId",
                principalTable: "PaymethodDB",
                principalColumn: "Id");
        }
    }
}
