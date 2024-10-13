using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _UpdatePaymentcs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDB_PaymentDB_PaymentId",
                table: "BankDB");

            migrationBuilder.DropIndex(
                name: "IX_BankDB_PaymentId",
                table: "BankDB");

            migrationBuilder.RenameColumn(
                name: "QRImage",
                table: "QRDB",
                newName: "QRImageData");

            migrationBuilder.RenameColumn(
                name: "Paymethod",
                table: "PaymentDB",
                newName: "PayMethod");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "QRDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PayMethod",
                table: "PaymentDB",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ExpiredDate",
                table: "CreditCardDB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "CreditCardDB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CVV",
                table: "CreditCardDB",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CardTypeId",
                table: "CreditCardDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "CreditCardDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "BankDB",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankNumber",
                table: "BankDB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CardOptionDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOptionDB", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QRDB_PaymentId",
                table: "QRDB",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardDB_CardTypeId",
                table: "CreditCardDB",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardDB_PaymentId",
                table: "CreditCardDB",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankDB_PaymentId",
                table: "BankDB",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankDB_PaymentDB_PaymentId",
                table: "BankDB",
                column: "PaymentId",
                principalTable: "PaymentDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCardDB_CardOptionDB_CardTypeId",
                table: "CreditCardDB",
                column: "CardTypeId",
                principalTable: "CardOptionDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCardDB_PaymentDB_PaymentId",
                table: "CreditCardDB",
                column: "PaymentId",
                principalTable: "PaymentDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QRDB_PaymentDB_PaymentId",
                table: "QRDB",
                column: "PaymentId",
                principalTable: "PaymentDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDB_PaymentDB_PaymentId",
                table: "BankDB");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCardDB_CardOptionDB_CardTypeId",
                table: "CreditCardDB");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCardDB_PaymentDB_PaymentId",
                table: "CreditCardDB");

            migrationBuilder.DropForeignKey(
                name: "FK_QRDB_PaymentDB_PaymentId",
                table: "QRDB");

            migrationBuilder.DropTable(
                name: "CardOptionDB");

            migrationBuilder.DropIndex(
                name: "IX_QRDB_PaymentId",
                table: "QRDB");

            migrationBuilder.DropIndex(
                name: "IX_CreditCardDB_CardTypeId",
                table: "CreditCardDB");

            migrationBuilder.DropIndex(
                name: "IX_CreditCardDB_PaymentId",
                table: "CreditCardDB");

            migrationBuilder.DropIndex(
                name: "IX_BankDB_PaymentId",
                table: "BankDB");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "QRDB");

            migrationBuilder.DropColumn(
                name: "CardTypeId",
                table: "CreditCardDB");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "CreditCardDB");

            migrationBuilder.RenameColumn(
                name: "QRImageData",
                table: "QRDB",
                newName: "QRImage");

            migrationBuilder.RenameColumn(
                name: "PayMethod",
                table: "PaymentDB",
                newName: "Paymethod");

            migrationBuilder.AlterColumn<string>(
                name: "Paymethod",
                table: "PaymentDB",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ExpiredDate",
                table: "CreditCardDB",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "CreditCardDB",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CVV",
                table: "CreditCardDB",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "BankDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "BankNumber",
                table: "BankDB",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BankDB_PaymentId",
                table: "BankDB",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankDB_PaymentDB_PaymentId",
                table: "BankDB",
                column: "PaymentId",
                principalTable: "PaymentDB",
                principalColumn: "Id");
        }
    }
}
