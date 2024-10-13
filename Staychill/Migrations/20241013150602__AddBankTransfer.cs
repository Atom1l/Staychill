using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddBankTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankTransferId",
                table: "BankAccBD",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankTransferDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransferDB", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccBD_BankTransferId",
                table: "BankAccBD",
                column: "BankTransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccBD_BankTransferDB_BankTransferId",
                table: "BankAccBD",
                column: "BankTransferId",
                principalTable: "BankTransferDB",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccBD_BankTransferDB_BankTransferId",
                table: "BankAccBD");

            migrationBuilder.DropTable(
                name: "BankTransferDB");

            migrationBuilder.DropIndex(
                name: "IX_BankAccBD_BankTransferId",
                table: "BankAccBD");

            migrationBuilder.DropColumn(
                name: "BankTransferId",
                table: "BankAccBD");
        }
    }
}
