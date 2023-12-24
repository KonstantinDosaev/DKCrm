using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class CorrectionManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExportedOrders",
                type: "text",
                nullable: true);
            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldFromStorages",
                table: "SoldFromStorages");

            migrationBuilder.DropIndex(
                name: "IX_SoldFromStorages_StorageId",
                table: "SoldFromStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseAtStorages",
                table: "PurchaseAtStorages");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseAtStorages_StorageId",
                table: "PurchaseAtStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseAtExports",
                table: "PurchaseAtExports");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseAtExports_ExportedProductId",
                table: "PurchaseAtExports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsInStorages",
                table: "ProductsInStorages");

            migrationBuilder.DropIndex(
                name: "IX_ProductsInStorages_StorageId",
                table: "ProductsInStorages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SoldFromStorages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PurchaseAtStorages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PurchaseAtExports");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductsInStorages");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "SoldFromStorages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "PurchaseAtStorages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "PurchaseAtExports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProductsInStorages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldFromStorages",
                table: "SoldFromStorages",
                columns: new[] { "StorageId", "ExportedProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseAtStorages",
                table: "PurchaseAtStorages",
                columns: new[] { "StorageId", "ImportedProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseAtExports",
                table: "PurchaseAtExports",
                columns: new[] { "ExportedProductId", "ImportedProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsInStorages",
                table: "ProductsInStorages",
                columns: new[] { "StorageId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExportedOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldFromStorages",
                table: "SoldFromStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseAtStorages",
                table: "PurchaseAtStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseAtExports",
                table: "PurchaseAtExports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsInStorages",
                table: "ProductsInStorages");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "SoldFromStorages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SoldFromStorages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "PurchaseAtStorages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PurchaseAtStorages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "PurchaseAtExports",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PurchaseAtExports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProductsInStorages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductsInStorages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldFromStorages",
                table: "SoldFromStorages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseAtStorages",
                table: "PurchaseAtStorages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseAtExports",
                table: "PurchaseAtExports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsInStorages",
                table: "ProductsInStorages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SoldFromStorages_StorageId",
                table: "SoldFromStorages",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAtStorages_StorageId",
                table: "PurchaseAtStorages",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAtExports_ExportedProductId",
                table: "PurchaseAtExports",
                column: "ExportedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInStorages_StorageId",
                table: "ProductsInStorages",
                column: "StorageId");
        }
    }
}
