using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _ADDQR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccBD_BankTransferDB_BankTransferId",
                table: "BankAccBD");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccBD_PaymethodDB_PaymethodId",
                table: "BankAccBD");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccBD",
                table: "BankAccBD");

            migrationBuilder.RenameTable(
                name: "BankAccBD",
                newName: "BankAccDB");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccBD_PaymethodId",
                table: "BankAccDB",
                newName: "IX_BankAccDB_PaymethodId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccBD_BankTransferId",
                table: "BankAccDB",
                newName: "IX_BankAccDB_BankTransferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccDB",
                table: "BankAccDB",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QRDataDB",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRPicData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRDataDB", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccDB_BankTransferDB_BankTransferId",
                table: "BankAccDB",
                column: "BankTransferId",
                principalTable: "BankTransferDB",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccDB_PaymethodDB_PaymethodId",
                table: "BankAccDB",
                column: "PaymethodId",
                principalTable: "PaymethodDB",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccDB_BankTransferDB_BankTransferId",
                table: "BankAccDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccDB_PaymethodDB_PaymethodId",
                table: "BankAccDB");

            migrationBuilder.DropTable(
                name: "QRDataDB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccDB",
                table: "BankAccDB");

            migrationBuilder.RenameTable(
                name: "BankAccDB",
                newName: "BankAccBD");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccDB_PaymethodId",
                table: "BankAccBD",
                newName: "IX_BankAccBD_PaymethodId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccDB_BankTransferId",
                table: "BankAccBD",
                newName: "IX_BankAccBD_BankTransferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccBD",
                table: "BankAccBD",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccBD_BankTransferDB_BankTransferId",
                table: "BankAccBD",
                column: "BankTransferId",
                principalTable: "BankTransferDB",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccBD_PaymethodDB_PaymethodId",
                table: "BankAccBD",
                column: "PaymethodId",
                principalTable: "PaymethodDB",
                principalColumn: "Id");
        }
    }
}
