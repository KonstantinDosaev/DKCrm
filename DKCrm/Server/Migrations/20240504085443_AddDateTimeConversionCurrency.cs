using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddDateTimeConversionCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeConversionCurrency",
                table: "ImportedProducts",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeConversionCurrency",
                table: "ExportedProducts",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeCreated",
                table: "CommentOnImportedOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "CommentOnImportedOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeCreated",
                table: "CommentOnExportedOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "CommentOnExportedOrders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeConversionCurrency",
                table: "ImportedProducts");

            migrationBuilder.DropColumn(
                name: "DateTimeConversionCurrency",
                table: "ExportedProducts");

            migrationBuilder.DropColumn(
                name: "DateTimeCreated",
                table: "CommentOnImportedOrders");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "CommentOnImportedOrders");

            migrationBuilder.DropColumn(
                name: "DateTimeCreated",
                table: "CommentOnExportedOrders");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "CommentOnExportedOrders");
        }
    }
}
