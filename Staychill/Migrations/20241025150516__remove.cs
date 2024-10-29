using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReCartId",
                table: "RetainCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "RetainCartItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReCartId",
                table: "RetainCartItems");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "RetainCartItems");
        }
    }
}
