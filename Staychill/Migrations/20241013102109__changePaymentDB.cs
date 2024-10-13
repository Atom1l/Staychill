using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _changePaymentDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCardDB_CardOptionDB_CardTypeId",
                table: "CreditCardDB");

            migrationBuilder.DropTable(
                name: "CardOptionDB");

            migrationBuilder.DropIndex(
                name: "IX_CreditCardDB_CardTypeId",
                table: "CreditCardDB");

            migrationBuilder.DropColumn(
                name: "CardTypeId",
                table: "CreditCardDB");

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "CreditCardDB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "CreditCardDB");

            migrationBuilder.AddColumn<int>(
                name: "CardTypeId",
                table: "CreditCardDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CardOptionDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOptionDB", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardDB_CardTypeId",
                table: "CreditCardDB",
                column: "CardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCardDB_CardOptionDB_CardTypeId",
                table: "CreditCardDB",
                column: "CardTypeId",
                principalTable: "CardOptionDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
