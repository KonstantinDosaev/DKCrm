using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsInCompanyAndBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KPP",
                table: "FnsRequests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "FnsRequests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "FnsRequests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Kbk",
                table: "Companies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kpp",
                table: "Companies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KorBeneficiaryAccount",
                table: "BankDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Kpp",
                table: "BankDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KPP",
                table: "FnsRequests");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "FnsRequests");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "FnsRequests");

            migrationBuilder.DropColumn(
                name: "Kbk",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Kpp",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "KorBeneficiaryAccount",
                table: "BankDetails");

            migrationBuilder.DropColumn(
                name: "Kpp",
                table: "BankDetails");
        }
    }
}
