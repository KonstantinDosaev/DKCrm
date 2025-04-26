using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImpProdImpOfferLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportProductPriceImportOffer_ImportedProducts_ImportedProd~",
                table: "ExportProductPriceImportOffer");

            migrationBuilder.DropIndex(
                name: "IX_ExportProductPriceImportOffer_ImportedProductId",
                table: "ExportProductPriceImportOffer");

            migrationBuilder.DropColumn(
                name: "ImportedProductId",
                table: "ExportProductPriceImportOffer");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "ExportProductPriceImportOffer",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAddImport",
                table: "ExportProductPriceImportOffer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "ExportProductPriceImportOffer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ImportProductPriceImportOffers",
                columns: table => new
                {
                    ImportedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "IX_ImportProductPriceImportOffers_PriceId",
                table: "ImportProductPriceImportOffers",
                column: "PriceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportProductPriceImportOffers");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "ExportProductPriceImportOffer");

            migrationBuilder.DropColumn(
                name: "IsAddImport",
                table: "ExportProductPriceImportOffer");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "ExportProductPriceImportOffer");

            migrationBuilder.AddColumn<Guid>(
                name: "ImportedProductId",
                table: "ExportProductPriceImportOffer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExportProductPriceImportOffer_ImportedProductId",
                table: "ExportProductPriceImportOffer",
                column: "ImportedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportProductPriceImportOffer_ImportedProducts_ImportedProd~",
                table: "ExportProductPriceImportOffer",
                column: "ImportedProductId",
                principalTable: "ImportedProducts",
                principalColumn: "Id");
        }
    }
}
