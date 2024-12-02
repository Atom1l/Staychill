using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staychill.Migrations
{
    /// <inheritdoc />
    public partial class _addmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressDB",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Housenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alley = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Road = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subdistrict = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDB", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "CartDB",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDB", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "DiscountDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDB",
                columns: table => new
                {
                    PaymentmethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentmethodType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDB", x => x.PaymentmethodId);
                });

            migrationBuilder.CreateTable(
                name: "ProductDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Instock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentViewModel",
                columns: table => new
                {
                    ShipmentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    SelectedCartItemIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StaychillQRDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreQRData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaychillQRDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alley = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Road = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subdistrict = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDB_AddressDB_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AddressDB",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankTransferDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransferDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransferDB_PaymentDB_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentDB",
                        principalColumn: "PaymentmethodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardsDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiredDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardsDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCardsDB_PaymentDB_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentDB",
                        principalColumn: "PaymentmethodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRDataDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRPicData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserUploadedData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRDataDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QRDataDB_PaymentDB_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentDB",
                        principalColumn: "PaymentmethodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartitemsDB",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartitemsDB", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartitemsDB_CartDB_CartId",
                        column: x => x.CartId,
                        principalTable: "CartDB",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartitemsDB_DiscountDB_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "DiscountDB",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartitemsDB_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImagesDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Image1 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image2 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image3 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image4 = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImagesDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImagesDB_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ShipmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Invoice = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PaymentmethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingDB_PaymentDB_PaymentmethodId",
                        column: x => x.PaymentmethodId,
                        principalTable: "PaymentDB",
                        principalColumn: "PaymentmethodId");
                    table.ForeignKey(
                        name: "FK_TrackingDB_UserDB_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankPicsData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BankTransferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccDB_BankTransferDB_BankTransferId",
                        column: x => x.BankTransferId,
                        principalTable: "BankTransferDB",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "RetaincartsDB",
                columns: table => new
                {
                    ReCartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetaincartsDB", x => x.ReCartId);
                    table.ForeignKey(
                        name: "FK_RetaincartsDB_TrackingDB_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "TrackingDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetainCartItems",
                columns: table => new
                {
                    ReCartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductIMG = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: false),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<float>(type: "real", nullable: false),
                    TotalDiscountedPrice = table.Column<float>(type: "real", nullable: false),
                    RetainCartId = table.Column<int>(type: "int", nullable: false),
                    ReCartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetainCartItems", x => x.ReCartItemId);
                    table.ForeignKey(
                        name: "FK_RetainCartItems_ProductDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RetainCartItems_RetaincartsDB_RetainCartId",
                        column: x => x.RetainCartId,
                        principalTable: "RetaincartsDB",
                        principalColumn: "ReCartId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccDB_BankTransferId",
                table: "BankAccDB",
                column: "BankTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransferDB_PaymentMethodId",
                table: "BankTransferDB",
                column: "PaymentMethodId",
                unique: true,
                filter: "[PaymentMethodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardOptDB_CreditCardId",
                table: "CardOptDB",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CartitemsDB_CartId",
                table: "CartitemsDB",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartitemsDB_DiscountId",
                table: "CartitemsDB",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_CartitemsDB_ProductId",
                table: "CartitemsDB",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardsDB_PaymentMethodId",
                table: "CreditCardsDB",
                column: "PaymentMethodId",
                unique: true,
                filter: "[PaymentMethodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImagesDB_ProductId",
                table: "ProductImagesDB",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QRDataDB_PaymentMethodId",
                table: "QRDataDB",
                column: "PaymentMethodId",
                unique: true,
                filter: "[PaymentMethodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RetainCartItems_ProductId",
                table: "RetainCartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RetainCartItems_RetainCartId",
                table: "RetainCartItems",
                column: "RetainCartId");

            migrationBuilder.CreateIndex(
                name: "IX_RetaincartsDB_TrackingId",
                table: "RetaincartsDB",
                column: "TrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_PaymentmethodId",
                table: "TrackingDB",
                column: "PaymentmethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDB_UserId",
                table: "TrackingDB",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDB_AddressId",
                table: "UserDB",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDB_Username",
                table: "UserDB",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccDB");

            migrationBuilder.DropTable(
                name: "CardOptDB");

            migrationBuilder.DropTable(
                name: "CartitemsDB");

            migrationBuilder.DropTable(
                name: "FeedbackDB");

            migrationBuilder.DropTable(
                name: "ProductImagesDB");

            migrationBuilder.DropTable(
                name: "QRDataDB");

            migrationBuilder.DropTable(
                name: "RetainCartItems");

            migrationBuilder.DropTable(
                name: "ShipmentViewModel");

            migrationBuilder.DropTable(
                name: "StaychillQRDB");

            migrationBuilder.DropTable(
                name: "UserViewModel");

            migrationBuilder.DropTable(
                name: "BankTransferDB");

            migrationBuilder.DropTable(
                name: "CreditCardsDB");

            migrationBuilder.DropTable(
                name: "CartDB");

            migrationBuilder.DropTable(
                name: "DiscountDB");

            migrationBuilder.DropTable(
                name: "ProductDB");

            migrationBuilder.DropTable(
                name: "RetaincartsDB");

            migrationBuilder.DropTable(
                name: "TrackingDB");

            migrationBuilder.DropTable(
                name: "PaymentDB");

            migrationBuilder.DropTable(
                name: "UserDB");

            migrationBuilder.DropTable(
                name: "AddressDB");
        }
    }
}
