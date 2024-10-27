using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _AddBankAccStringinBankTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                table: "BankTransferDB",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccount",
                table: "BankTransferDB");
        }
    }
}
