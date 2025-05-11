using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReBuildOfferToImportExportProductsLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExportProductPriceImportOffer");

            migrationBuilder.DropTable(
                name: "ImportProductPriceImportOffers");

            migrationBuilder.AddColumn<Guid>(
                name: "PriceForImportOfferId",
                table: "ImportedProducts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedProducts_PriceForImportOfferId",
                table: "ImportedProducts",
                column: "PriceForImportOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedProducts_PricesForImportOffers_PriceForImportOfferId",
                table: "ImportedProducts",
                column: "PriceForImportOfferId",
                principalTable: "PricesForImportOffers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportedProducts_PricesForImportOffers_PriceForImportOfferId",
                table: "ImportedProducts");

            migrationBuilder.DropIndex(
                name: "IX_ImportedProducts_PriceForImportOfferId",
                table: "ImportedProducts");

            migrationBuilder.DropColumn(
                name: "PriceForImportOfferId",
                table: "ImportedProducts");

            migrationBuilder.CreateTable(
                name: "ExportProductPriceImportOffer",
                columns: table => new
                {
                    ExportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateTimeUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsAddImport = table.Column<bool>(type: "boolean", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0)
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
                        name: "FK_ExportProductPriceImportOffer_PricesForImportOffers_PriceId",
                        column: x => x.PriceId,
                        principalTable: "PricesForImportOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportProductPriceImportOffers",
                columns: table => new
                {
                    ImportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportProductPriceImportOffers", x => new { x.ImportedProductId, x.PriceId });
                    table.ForeignKey(
                        name: "FK_ImportProductPriceImportOffers_ImportedProducts_ImportedPro~",
                        column: x => x.ImportedProductId,
                        principalTable: "ImportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportProductPriceImportOffers_PricesForImportOffers_PriceId",
                        column: x => x.PriceId,
                        principalTable: "PricesForImportOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExportProductPriceImportOffer_PriceId",
                table: "ExportProductPriceImportOffer",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportProductPriceImportOffers_PriceId",
                table: "ImportProductPriceImportOffers",
                column: "PriceId");
        }
    }
}
