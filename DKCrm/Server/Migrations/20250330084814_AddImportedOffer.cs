using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImportedOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
      
            migrationBuilder.CreateTable(
                name: "ImportOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateTimeUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUser = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportOffers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImportOffers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExportedProductImportOffer",
                columns: table => new
                {
                    ExportedProductsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportOffersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedProductImportOffer", x => new { x.ExportedProductsId, x.ImportOffersId });
                    table.ForeignKey(
                        name: "FK_ExportedProductImportOffer_ExportedProducts_ExportedProduct~",
                        column: x => x.ExportedProductsId,
                        principalTable: "ExportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportedProductImportOffer_ImportOffers_ImportOffersId",
                        column: x => x.ImportOffersId,
                        principalTable: "ImportOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportOfferImportedProduct",
                columns: table => new
                {
                    ImportOffersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportedProductsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportOfferImportedProduct", x => new { x.ImportOffersId, x.ImportedProductsId });
                    table.ForeignKey(
                        name: "FK_ImportOfferImportedProduct_ImportOffers_ImportOffersId",
                        column: x => x.ImportOffersId,
                        principalTable: "ImportOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportOfferImportedProduct_ImportedProducts_ImportedProduct~",
                        column: x => x.ImportedProductsId,
                        principalTable: "ImportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricesForImportOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    ImportOfferId = table.Column<Guid>(type: "uuid", nullable: true),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateTimeOver = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateTimeUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesForImportOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricesForImportOffers_ImportOffers_ImportOfferId",
                        column: x => x.ImportOfferId,
                        principalTable: "ImportOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExportedProductImportOffer_ImportOffersId",
                table: "ExportedProductImportOffer",
                column: "ImportOffersId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOfferImportedProduct_ImportedProductsId",
                table: "ImportOfferImportedProduct",
                column: "ImportedProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOffers_CompanyId",
                table: "ImportOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOffers_ProductId",
                table: "ImportOffers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PricesForImportOffers_ImportOfferId",
                table: "PricesForImportOffers",
                column: "ImportOfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExportedProductImportOffer");

            migrationBuilder.DropTable(
                name: "ImportOfferImportedProduct");

            migrationBuilder.DropTable(
                name: "PricesForImportOffers");

            migrationBuilder.DropTable(
                name: "ImportOffers");

        }
    }
}
