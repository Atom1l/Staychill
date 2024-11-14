using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _maketrackaccessproIMG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProductIMG",
                table: "RetainCartItems",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductIMG",
                table: "RetainCartItems");
        }
    }
}
