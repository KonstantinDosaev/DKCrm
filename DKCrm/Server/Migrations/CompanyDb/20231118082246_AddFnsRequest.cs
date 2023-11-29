using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations.CompanyDb
{
    public partial class AddFnsRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FnsRequests_CompanyId",
                table: "FnsRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "FnsRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "FnsRequestId",
                table: "Companies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FnsRequests_CompanyId",
                table: "FnsRequests",
                column: "CompanyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FnsRequests_CompanyId",
                table: "FnsRequests");

            migrationBuilder.DropColumn(
                name: "FnsRequestId",
                table: "Companies");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "FnsRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FnsRequests_CompanyId",
                table: "FnsRequests",
                column: "CompanyId");
        }
    }
}
