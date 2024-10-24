using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _DiscountwithCart01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "CartitemsDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartitemsDB_DiscountId",
                table: "CartitemsDB",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartitemsDB_DiscountDB_DiscountId",
                table: "CartitemsDB",
                column: "DiscountId",
                principalTable: "DiscountDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartitemsDB_DiscountDB_DiscountId",
                table: "CartitemsDB");

            migrationBuilder.DropIndex(
                name: "IX_CartitemsDB_DiscountId",
                table: "CartitemsDB");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "CartitemsDB");
        }
    }
}
