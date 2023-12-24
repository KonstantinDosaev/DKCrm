using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddCurrencyStatusCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValueConstant",
                table: "ImportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocalCurrency",
                table: "ImportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierCurrency",
                table: "ImportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionCurrency",
                table: "ImportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsValueConstant",
                table: "ExportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BuyerCurrency",
                table: "ExportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalCurrency",
                table: "ExportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionCurrency",
                table: "ExportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InternalCompanyData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalCurrency = table.Column<string>(type: "text", nullable: true),
                    CurrencyPercent = table.Column<double>(type: "double precision", nullable: false),
                    KeyFns = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalCompanyData", x => x.Id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "InternalCompanyData");

            migrationBuilder.DropColumn(
                name: "IsValueConstant",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "LocalCurrency",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "SupplierCurrency",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "TransactionCurrency",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "IsValueConstant",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "BuyerCurrency",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "LocalCurrency",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "TransactionCurrency",
                table: "ExportedOrders");

        }
    }
}
