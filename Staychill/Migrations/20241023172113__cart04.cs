using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    public partial class _cart04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartItems_RetaincartsDB_ReCartItemId",
                table: "RetainCartItems");

            // Drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_RetainCartItems",
                table: "RetainCartItems");

            // Drop the column
            migrationBuilder.DropColumn(
                name: "ReCartItemId",
                table: "RetainCartItems");

            // Add the new column with the identity property
            migrationBuilder.AddColumn<int>(
                name: "ReCartItemId",
                table: "RetainCartItems",
                type: "int",
                nullable: false
            ).Annotation("SqlServer:Identity", "1, 1");

            // Recreate the primary key constraint on the new column
            migrationBuilder.AddPrimaryKey(
                name: "PK_RetainCartItems",
                table: "RetainCartItems",
                column: "ReCartItemId");

            // Recreate the foreign key constraint
            migrationBuilder.CreateIndex(
                name: "IX_RetainCartItems_RetainCartId",
                table: "RetainCartItems",
                column: "RetainCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartItems_RetaincartsDB_RetainCartId",
                table: "RetainCartItems",
                column: "RetainCartId",
                principalTable: "RetaincartsDB",
                principalColumn: "ReCartId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_RetainCartItems_RetaincartsDB_RetainCartId",
                table: "RetainCartItems");

            // Drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_RetainCartItems",
                table: "RetainCartItems");

            // Drop the new column
            migrationBuilder.DropColumn(
                name: "ReCartItemId",
                table: "RetainCartItems");

            // Re-add the old column without identity
            migrationBuilder.AddColumn<int>(
                name: "ReCartItemId",
                table: "RetainCartItems",
                type: "int",
                nullable: false);

            // Recreate the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_RetainCartItems",
                table: "RetainCartItems",
                column: "ReCartItemId");

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_RetainCartItems_RetaincartsDB_ReCartItemId",
                table: "RetainCartItems",
                column: "ReCartItemId",
                principalTable: "RetaincartsDB",
                principalColumn: "ReCartId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
