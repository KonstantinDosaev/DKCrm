using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddCheckOverToOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
               migrationBuilder.AddColumn<bool>(
                name: "OrderIsOver",
                table: "ImportedOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
               
            migrationBuilder.AddColumn<bool>(
                name: "OrderIsOver",
                table: "ExportedOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIsOver",
                table: "ImportedOrders");

            migrationBuilder.DropColumn(
                name: "OrderIsOver",
                table: "ExportedOrders");
        }
    }
}
