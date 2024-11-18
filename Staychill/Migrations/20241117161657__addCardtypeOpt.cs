using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _addCardtypeOpt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardOptDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditcardOpt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOptDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardOptDB_CreditCardsDB_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "CreditCardsDB",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardOptDB_CreditCardId",
                table: "CardOptDB",
                column: "CreditCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardOptDB");
        }
    }
}
