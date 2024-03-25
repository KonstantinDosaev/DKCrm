using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class StatusChild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ImportedOrderStatus",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ExportedOrderStatus",
                type: "uuid",
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_ImportedOrderStatus_ParentId",
                table: "ImportedOrderStatus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedOrderStatus_ParentId",
                table: "ExportedOrderStatus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrderStatus_ExportedOrderStatus_ParentId",
                table: "ExportedOrderStatus",
                column: "ParentId",
                principalTable: "ExportedOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrderStatus_ImportedOrderStatus_ParentId",
                table: "ImportedOrderStatus",
                column: "ParentId",
                principalTable: "ImportedOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AlterColumn<double>(
                name: "Position",
                table: "ImportedOrderStatus",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Position",
                table: "ExportedOrderStatus",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrderStatus_ExportedOrderStatus_ParentId",
                table: "ExportedOrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrderStatus_ImportedOrderStatus_ParentId",
                table: "ImportedOrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_ImportedOrderStatus_ParentId",
                table: "ImportedOrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_ExportedOrderStatus_ParentId",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ExportedOrderStatus");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "ImportedOrderStatus",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "ExportedOrderStatus",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

        }
    }
}
