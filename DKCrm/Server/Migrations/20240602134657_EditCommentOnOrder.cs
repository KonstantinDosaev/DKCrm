using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class EditCommentOnOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOnExportedOrders");

            migrationBuilder.DropTable(
                name: "CommentOnImportedOrders");

            migrationBuilder.CreateTable(
                name: "CommentOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    FromUserId = table.Column<string>(type: "text", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOrders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOrders");

            migrationBuilder.CreateTable(
                name: "CommentOnExportedOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExportedOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FromUserId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnExportedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentOnExportedOrders_ExportedOrders_ExportedOrderId",
                        column: x => x.ExportedOrderId,
                        principalTable: "ExportedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentOnImportedOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportedOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FromUserId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnImportedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentOnImportedOrders_ImportedOrders_ImportedOrderId",
                        column: x => x.ImportedOrderId,
                        principalTable: "ImportedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnExportedOrders_ExportedOrderId",
                table: "CommentOnExportedOrders",
                column: "ExportedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnImportedOrders_ImportedOrderId",
                table: "CommentOnImportedOrders",
                column: "ImportedOrderId");
        }
    }
}
