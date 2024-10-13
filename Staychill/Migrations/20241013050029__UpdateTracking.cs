using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _UpdateTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCardDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paymethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Instock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QRDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDB_PaymentDB_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "PaymentDB",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImagesDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Image1 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image2 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image3 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image4 = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImagesDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImagesDB_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankDB_PaymentId",
                table: "BankDB",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImagesDB_ProductId",
                table: "ProductImagesDB",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDB");

            migrationBuilder.DropTable(
                name: "CreditCardDB");

            migrationBuilder.DropTable(
                name: "DiscountDB");

            migrationBuilder.DropTable(
                name: "ProductImagesDB");

            migrationBuilder.DropTable(
                name: "QRDB");

            migrationBuilder.DropTable(
                name: "TrackingDB");

            migrationBuilder.DropTable(
                name: "PaymentDB");

            migrationBuilder.DropTable(
                name: "ProductDB");
        }
    }
}
