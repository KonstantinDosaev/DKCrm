using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddSoftDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "Storages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Storages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "Storages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "Storages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "ApplicationOrderingProducts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationOrderingProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ApplicationOrderingProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ApplicationOrderingProducts");
        }
    }
}
