using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CurrencyLocal",
                table: "PricesForImportOffers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CurrencyPercent",
                table: "PricesForImportOffers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nds",
                table: "PricesForImportOffers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValueLocal",
                table: "PricesForImportOffers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "CurrencyLocal",
                table: "PricesForImportOffers");

            migrationBuilder.DropColumn(
                name: "CurrencyPercent",
                table: "PricesForImportOffers");

            migrationBuilder.DropColumn(
                name: "Nds",
                table: "PricesForImportOffers");

            migrationBuilder.DropColumn(
                name: "ValueLocal",
                table: "PricesForImportOffers");

        }
    }
}
