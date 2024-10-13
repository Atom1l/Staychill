using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddBankDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductViewModel");

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
                name: "BankAccBD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankPicsData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PaymethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccBD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccBD_PaymethodDB_PaymethodId",
                        column: x => x.PaymethodId,
                        principalTable: "PaymethodDB",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccBD_PaymethodId",
                table: "BankAccBD",
                column: "PaymethodId",
                unique: true,
                filter: "[PaymethodId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccBD");

            migrationBuilder.DropTable(
                name: "PaymethodDB");

            migrationBuilder.CreateTable(
                name: "ProductViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Image1 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image2 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image3 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image4 = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductViewModel_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductViewModel_ProductId",
                table: "ProductViewModel",
                column: "ProductId");
        }
    }
}
