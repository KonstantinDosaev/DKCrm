using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReplacementConnExpOrderImportOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExportedProductImportOffer");

            migrationBuilder.DropTable(
                name: "ImportOfferImportedProduct");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "PricesForImportOffers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedUser",
                table: "ImportOffers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExportProductPriceImportOffer",
                columns: table => new
                {
                    ExportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    ImportedProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportProductPriceImportOffer", x => new { x.ExportedProductId, x.PriceId });
                    table.ForeignKey(
                        name: "FK_ExportProductPriceImportOffer_ExportedProducts_ExportedProd~",
                        column: x => x.ExportedProductId,
                        principalTable: "ExportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportProductPriceImportOffer_ImportedProducts_ImportedProd~",
                        column: x => x.ImportedProductId,
                        principalTable: "ImportedProducts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExportProductPriceImportOffer_PricesForImportOffers_PriceId",
                        column: x => x.PriceId,
                        principalTable: "PricesForImportOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExportProductPriceImportOffer_ImportedProductId",
                table: "ExportProductPriceImportOffer",
                column: "ImportedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportProductPriceImportOffer_PriceId",
                table: "ExportProductPriceImportOffer",
                column: "PriceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExportProductPriceImportOffer");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PricesForImportOffers");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedUser",
                table: "ImportOffers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExportedProductImportOffer",
                columns: table => new
                {
                    ExportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportOffersId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedProductImportOffer", x => new { x.ExportedProductId, x.ImportOffersId });
                    table.ForeignKey(
                        name: "FK_ExportedProductImportOffer_ExportedProducts_ExportedProduct~",
                        column: x => x.ExportedProductId,
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

            migrationBuilder.CreateIndex(
                name: "IX_ExportedProductImportOffer_ImportOffersId",
                table: "ExportedProductImportOffer",
                column: "ImportOffersId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOfferImportedProduct_ImportedProductsId",
                table: "ImportOfferImportedProduct",
                column: "ImportedProductsId");
        }
    }
}
