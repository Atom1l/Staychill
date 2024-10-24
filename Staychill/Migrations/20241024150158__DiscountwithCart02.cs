using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _DiscountwithCart02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartitemsDB_DiscountDB_DiscountId",
                table: "CartitemsDB");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "CartitemsDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CartitemsDB_DiscountDB_DiscountId",
                table: "CartitemsDB",
                column: "DiscountId",
                principalTable: "DiscountDB",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartitemsDB_DiscountDB_DiscountId",
                table: "CartitemsDB");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "CartitemsDB",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartitemsDB_DiscountDB_DiscountId",
                table: "CartitemsDB",
                column: "DiscountId",
                principalTable: "DiscountDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
