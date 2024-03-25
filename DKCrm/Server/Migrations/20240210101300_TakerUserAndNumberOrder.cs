using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class TakerUserAndNumberOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ImportedOrders",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ExportedOrders",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ApplicationOrderingProducts",
                newName: "Number");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeCreate",
                table: "ImportedOrderStatusImportedOrder",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeCreate",
                table: "ExportedOrderStatusExportedOrder",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TakerUser",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TakerUserId",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeTake",
                table: "ApplicationOrderingProducts",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeTake",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "TakerUser",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "TakerUserId",
                table: "ApplicationOrderingProducts");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "ImportedOrders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "ExportedOrders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "ApplicationOrderingProducts",
                newName: "Name");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeCreate",
                table: "ImportedOrderStatusImportedOrder",
                type: "timestamp without time zone",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeCreate",
                table: "ExportedOrderStatusExportedOrder",
                type: "timestamp without time zone",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
