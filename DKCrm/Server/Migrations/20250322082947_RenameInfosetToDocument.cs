using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameInfosetToDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "InfoSetsToDocuments",
                newName: "OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                table: "InfoSetsToDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Extension",
                table: "InfoSetsToDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "InfoSetsToDocuments");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "InfoSetsToDocuments");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "InfoSetsToDocuments",
                newName: "OrderId");
        }
    }
}
