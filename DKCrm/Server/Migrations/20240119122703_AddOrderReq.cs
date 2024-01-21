using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class AddOrderReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationOrderingProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    CompanyInn = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationOrderingProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationOrderingProductsProduct",
                columns: table => new
                {
                    ApplicationOrderingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductListId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationOrderingProductsProduct", x => new { x.ApplicationOrderingListId, x.ProductListId });
                    table.ForeignKey(
                        name: "FK_ApplicationOrderingProductsProduct_ApplicationOrderingProdu~",
                        column: x => x.ApplicationOrderingListId,
                        principalTable: "ApplicationOrderingProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationOrderingProductsProduct_Products_ProductListId",
                        column: x => x.ProductListId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOrderingProductsProduct_ProductListId",
                table: "ApplicationOrderingProductsProduct",
                column: "ProductListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationOrderingProductsProduct");

            migrationBuilder.DropTable(
                name: "ApplicationOrderingProducts");
        }
    }
}
