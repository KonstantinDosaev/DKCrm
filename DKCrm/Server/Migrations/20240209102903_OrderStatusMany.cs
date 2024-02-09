using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class OrderStatusMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_ExportedOrderStatus_ExportedOrderStatusId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_ImportedOrderStatus_ImportedOrderStatusId",
                table: "ImportedOrders");

            migrationBuilder.DropIndex(
                name: "IX_ImportedOrders_ImportedOrderStatusId",
                table: "ImportedOrders");

            migrationBuilder.DropIndex(
                name: "IX_ExportedOrders_ExportedOrderStatusId",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "ImportedOrderStatusId",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "ExportedOrderStatusId",
                table: "ExportedOrders");

            migrationBuilder.RenameColumn(
                name: "DateTimeUpdated",
                table: "ImportedOrders",
                newName: "DateTimeUpdate");

            migrationBuilder.RenameColumn(
                name: "DateTimeUpdated",
                table: "ExportedOrders",
                newName: "DateTimeUpdate");

            migrationBuilder.AddColumn<double>(
                name: "Nds",
                table: "InternalCompanyData",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "ImportedProducts",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ImportedProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ImportedProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ImportedProducts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "ImportedOrderStatus",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ImportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ImportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ImportedOrderStatus",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ImportedOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ImportedOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Nds",
                table: "ImportedOrders",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ImportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "ExportedProducts",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExportedProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ExportedProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ExportedProducts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUpdate",
                table: "ExportedOrderStatus",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ExportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ExportedOrderStatus",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExportedOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDeleted",
                table: "ExportedOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Nds",
                table: "ExportedOrders",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ExportedOrders",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExportedOrderStatusExportedOrder",
                columns: table => new
                {
                    ExportedOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExportedOrderStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    DateTimeUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedOrderStatusExportedOrder", x => new { x.ExportedOrderId, x.ExportedOrderStatusId });
                    table.ForeignKey(
                        name: "FK_ExportedOrderStatusExportedOrder_ExportedOrders_ExportedOrd~",
                        column: x => x.ExportedOrderId,
                        principalTable: "ExportedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportedOrderStatusExportedOrder_ExportedOrderStatus_Export~",
                        column: x => x.ExportedOrderStatusId,
                        principalTable: "ExportedOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedOrderStatusImportedOrder",
                columns: table => new
                {
                    ImportedOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportedOrderStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    DateTimeUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedOrderStatusImportedOrder", x => new { x.ImportedOrderId, x.ImportedOrderStatusId });
                    table.ForeignKey(
                        name: "FK_ImportedOrderStatusImportedOrder_ImportedOrders_ImportedOrd~",
                        column: x => x.ImportedOrderId,
                        principalTable: "ImportedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportedOrderStatusImportedOrder_ImportedOrderStatus_Import~",
                        column: x => x.ImportedOrderStatusId,
                        principalTable: "ImportedOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrderStatusExportedOrder_ExportedOrderStatusId",
                table: "ExportedOrderStatusExportedOrder",
                column: "ExportedOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrderStatusImportedOrder_ImportedOrderStatusId",
                table: "ImportedOrderStatusImportedOrder",
                column: "ImportedOrderStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExportedOrderStatusExportedOrder");

            migrationBuilder.DropTable(
                name: "ImportedOrderStatusImportedOrder");

            migrationBuilder.DropColumn(
                name: "Nds",
                table: "InternalCompanyData");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "ImportedProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ImportedProducts");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ImportedProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ImportedProducts");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "Nds",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "ExportedProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExportedProducts");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ExportedProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ExportedProducts");

            migrationBuilder.DropColumn(
                name: "DateTimeUpdate",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "IsFullDeleted",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "Nds",
                table: "ExportedOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ExportedOrders");

            migrationBuilder.RenameColumn(
                name: "DateTimeUpdate",
                table: "ImportedOrders",
                newName: "DateTimeUpdated");

            migrationBuilder.RenameColumn(
                name: "DateTimeUpdate",
                table: "ExportedOrders",
                newName: "DateTimeUpdated");

            migrationBuilder.AddColumn<Guid>(
                name: "ImportedOrderStatusId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExportedOrderStatusId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrders_ImportedOrderStatusId",
                table: "ImportedOrders",
                column: "ImportedOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrders_ExportedOrderStatusId",
                table: "ExportedOrders",
                column: "ExportedOrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_ExportedOrderStatus_ExportedOrderStatusId",
                table: "ExportedOrders",
                column: "ExportedOrderStatusId",
                principalTable: "ExportedOrderStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_ImportedOrderStatus_ImportedOrderStatusId",
                table: "ImportedOrders",
                column: "ImportedOrderStatusId",
                principalTable: "ImportedOrderStatus",
                principalColumn: "Id");
        }
    }
}
