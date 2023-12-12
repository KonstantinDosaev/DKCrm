using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class UpdateCompanyByOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Companies_CompanyBuyerId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Companies_OurCompanyId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Employees_EmployeeBuyerId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Employees_OurEmployeeId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Companies_OurCompanyId",
                table: "ImportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Companies_SellersCompanyId",
                table: "ImportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Employees_EmployeeSellerId",
                table: "ImportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Employees_OurEmployeeId",
                table: "ImportedOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "SellersCompanyId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurEmployeeId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurCompanyId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeSellerId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurEmployeeId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurCompanyId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeBuyerId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyBuyerId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Companies_CompanyBuyerId",
                table: "ExportedOrders",
                column: "CompanyBuyerId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Companies_OurCompanyId",
                table: "ExportedOrders",
                column: "OurCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Employees_EmployeeBuyerId",
                table: "ExportedOrders",
                column: "EmployeeBuyerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Employees_OurEmployeeId",
                table: "ExportedOrders",
                column: "OurEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Companies_OurCompanyId",
                table: "ImportedOrders",
                column: "OurCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Companies_SellersCompanyId",
                table: "ImportedOrders",
                column: "SellersCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Employees_EmployeeSellerId",
                table: "ImportedOrders",
                column: "EmployeeSellerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Employees_OurEmployeeId",
                table: "ImportedOrders",
                column: "OurEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Companies_CompanyBuyerId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Companies_OurCompanyId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Employees_EmployeeBuyerId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportedOrders_Employees_OurEmployeeId",
                table: "ExportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Companies_OurCompanyId",
                table: "ImportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Companies_SellersCompanyId",
                table: "ImportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Employees_EmployeeSellerId",
                table: "ImportedOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportedOrders_Employees_OurEmployeeId",
                table: "ImportedOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "SellersCompanyId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurEmployeeId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurCompanyId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeSellerId",
                table: "ImportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurEmployeeId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "OurCompanyId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeBuyerId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyBuyerId",
                table: "ExportedOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Companies_CompanyBuyerId",
                table: "ExportedOrders",
                column: "CompanyBuyerId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Companies_OurCompanyId",
                table: "ExportedOrders",
                column: "OurCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Employees_EmployeeBuyerId",
                table: "ExportedOrders",
                column: "EmployeeBuyerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedOrders_Employees_OurEmployeeId",
                table: "ExportedOrders",
                column: "OurEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Companies_OurCompanyId",
                table: "ImportedOrders",
                column: "OurCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Companies_SellersCompanyId",
                table: "ImportedOrders",
                column: "SellersCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Employees_EmployeeSellerId",
                table: "ImportedOrders",
                column: "EmployeeSellerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedOrders_Employees_OurEmployeeId",
                table: "ImportedOrders",
                column: "OurEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
