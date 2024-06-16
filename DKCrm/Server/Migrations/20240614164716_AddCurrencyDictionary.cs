using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddCurrencyDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyDictionaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Male = table.Column<bool>(type: "boolean", nullable: false),
                    MaleLoverNominal = table.Column<bool>(type: "boolean", nullable: false),
                    CharCode = table.Column<string>(type: "text", nullable: false),
                    One = table.Column<string>(type: "text", nullable: false),
                    Two = table.Column<string>(type: "text", nullable: false),
                    Five = table.Column<string>(type: "text", nullable: false),
                    OneLoverNominal = table.Column<string>(type: "text", nullable: false),
                    TwoLoverNominal = table.Column<string>(type: "text", nullable: false),
                    FiveLoverNominal = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyDictionaries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyDictionaries");
        }
    }
}
