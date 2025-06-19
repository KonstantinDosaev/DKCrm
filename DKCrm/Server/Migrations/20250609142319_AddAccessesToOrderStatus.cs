using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessesToOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowMoveBack",
                table: "ImportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LimitPositionToEditOrder",
                table: "ImportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UsersWitchAccess",
                table: "ImportedOrderStatus",
                type: "character varying(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowMoveBack",
                table: "ExportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LimitPositionToEditOrder",
                table: "ExportedOrderStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UsersWitchAccess",
                table: "ExportedOrderStatus",
                type: "character varying(400)",
                maxLength: 400,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowMoveBack",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "LimitPositionToEditOrder",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "UsersWitchAccess",
                table: "ImportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "AllowMoveBack",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "LimitPositionToEditOrder",
                table: "ExportedOrderStatus");

            migrationBuilder.DropColumn(
                name: "UsersWitchAccess",
                table: "ExportedOrderStatus");
        }
    }
}
