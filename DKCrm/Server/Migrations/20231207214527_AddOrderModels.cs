using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddOrderModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExportedOrderStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportedOrderStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportedOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Images = table.Column<string>(type: "text", nullable: true),
                    CurrencyPercent = table.Column<double>(type: "double precision", nullable: true),
                    OurCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyBuyerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OurEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeBuyerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExportedOrderStatusId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportedOrders_Companies_CompanyBuyerId",
                        column: x => x.CompanyBuyerId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExportedOrders_Companies_OurCompanyId",
                        column: x => x.OurCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExportedOrders_Employees_EmployeeBuyerId",
                        column: x => x.EmployeeBuyerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExportedOrders_Employees_OurEmployeeId",
                        column: x => x.OurEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExportedOrders_ExportedOrderStatus_ExportedOrderStatusId",
                        column: x => x.ExportedOrderStatusId,
                        principalTable: "ExportedOrderStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImportedOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Images = table.Column<string>(type: "text", nullable: true),
                    CurrencyPercent = table.Column<double>(type: "double precision", nullable: true),
                    OurCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellersCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    OurEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeSellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ImportedOrderStatusId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedOrders_Companies_OurCompanyId",
                        column: x => x.OurCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ImportedOrders_Companies_SellersCompanyId",
                        column: x => x.SellersCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ImportedOrders_Employees_EmployeeSellerId",
                        column: x => x.EmployeeSellerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ImportedOrders_Employees_OurEmployeeId",
                        column: x => x.OurEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ImportedOrders_ImportedOrderStatus_ImportedOrderStatusId",
                        column: x => x.ImportedOrderStatusId,
                        principalTable: "ImportedOrderStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentOnExportedOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ExportedOrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnExportedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentOnExportedOrders_ExportedOrders_ExportedOrderId",
                        column: x => x.ExportedOrderId,
                        principalTable: "ExportedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExportedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PriceLocal = table.Column<decimal>(type: "numeric", nullable: true),
                    TransactionCurrency = table.Column<string>(type: "text", nullable: true),
                    PriceInTransactionCurrency = table.Column<decimal>(type: "numeric", nullable: true),
                    BuyerCurrency = table.Column<string>(type: "text", nullable: true),
                    PriceInBuyerCurrency = table.Column<decimal>(type: "numeric", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExportedOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportedProducts_ExportedOrders_ExportedOrderId",
                        column: x => x.ExportedOrderId,
                        principalTable: "ExportedOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExportedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentOnImportedOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ImportedOrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnImportedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentOnImportedOrders_ImportedOrders_ImportedOrderId",
                        column: x => x.ImportedOrderId,
                        principalTable: "ImportedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PriceLocal = table.Column<decimal>(type: "numeric", nullable: true),
                    TransactionCurrency = table.Column<string>(type: "text", nullable: true),
                    PriceInTransactionCurrency = table.Column<decimal>(type: "numeric", nullable: true),
                    SupplierCurrency = table.Column<string>(type: "text", nullable: true),
                    PriceInSupplierCurrency = table.Column<decimal>(type: "numeric", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    ImportedOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedProducts_ImportedOrders_ImportedOrderId",
                        column: x => x.ImportedOrderId,
                        principalTable: "ImportedOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImportedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SoldFromStorages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ExportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldFromStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldFromStorages_ExportedProducts_ExportedProductId",
                        column: x => x.ExportedProductId,
                        principalTable: "ExportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldFromStorages_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseAtExports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ExportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportedProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAtExports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseAtExports_ExportedProducts_ExportedProductId",
                        column: x => x.ExportedProductId,
                        principalTable: "ExportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseAtExports_ImportedProducts_ImportedProductId",
                        column: x => x.ImportedProductId,
                        principalTable: "ImportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseAtStorages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    StorageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportedProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAtStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseAtStorages_ImportedProducts_ImportedProductId",
                        column: x => x.ImportedProductId,
                        principalTable: "ImportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseAtStorages_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnExportedOrders_ExportedOrderId",
                table: "CommentOnExportedOrders",
                column: "ExportedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnImportedOrders_ImportedOrderId",
                table: "CommentOnImportedOrders",
                column: "ImportedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrders_CompanyBuyerId",
                table: "ExportedOrders",
                column: "CompanyBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrders_EmployeeBuyerId",
                table: "ExportedOrders",
                column: "EmployeeBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrders_ExportedOrderStatusId",
                table: "ExportedOrders",
                column: "ExportedOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrders_OurCompanyId",
                table: "ExportedOrders",
                column: "OurCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrders_OurEmployeeId",
                table: "ExportedOrders",
                column: "OurEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedProducts_ExportedOrderId",
                table: "ExportedProducts",
                column: "ExportedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedProducts_ProductId",
                table: "ExportedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrders_EmployeeSellerId",
                table: "ImportedOrders",
                column: "EmployeeSellerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrders_ImportedOrderStatusId",
                table: "ImportedOrders",
                column: "ImportedOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrders_OurCompanyId",
                table: "ImportedOrders",
                column: "OurCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrders_OurEmployeeId",
                table: "ImportedOrders",
                column: "OurEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrders_SellersCompanyId",
                table: "ImportedOrders",
                column: "SellersCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedProducts_ImportedOrderId",
                table: "ImportedProducts",
                column: "ImportedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedProducts_ProductId",
                table: "ImportedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAtExports_ExportedProductId",
                table: "PurchaseAtExports",
                column: "ExportedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAtExports_ImportedProductId",
                table: "PurchaseAtExports",
                column: "ImportedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAtStorages_ImportedProductId",
                table: "PurchaseAtStorages",
                column: "ImportedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAtStorages_StorageId",
                table: "PurchaseAtStorages",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldFromStorages_ExportedProductId",
                table: "SoldFromStorages",
                column: "ExportedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldFromStorages_StorageId",
                table: "SoldFromStorages",
                column: "StorageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOnExportedOrders");

            migrationBuilder.DropTable(
                name: "CommentOnImportedOrders");

            migrationBuilder.DropTable(
                name: "PurchaseAtExports");

            migrationBuilder.DropTable(
                name: "PurchaseAtStorages");

            migrationBuilder.DropTable(
                name: "SoldFromStorages");

            migrationBuilder.DropTable(
                name: "ImportedProducts");

            migrationBuilder.DropTable(
                name: "ExportedProducts");

            migrationBuilder.DropTable(
                name: "ImportedOrders");

            migrationBuilder.DropTable(
                name: "ExportedOrders");

            migrationBuilder.DropTable(
                name: "ImportedOrderStatus");

            migrationBuilder.DropTable(
                name: "ExportedOrderStatus");
        }
    }
}
