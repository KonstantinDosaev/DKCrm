using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    public partial class UpdateModelsAndContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_GrandCategory_GrandCategoryId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryOption_Category_CategoryId",
                table: "CategoryOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOption_CategoryOption_CategoryOptionId",
                table: "ProductOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOption_Products_ProductId",
                table: "ProductOption");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Category_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOption",
                table: "ProductOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GrandCategory",
                table: "GrandCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryOption",
                table: "CategoryOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.RenameTable(
                name: "ProductOption",
                newName: "ProductOptions");

            migrationBuilder.RenameTable(
                name: "GrandCategory",
                newName: "GrandCategories");

            migrationBuilder.RenameTable(
                name: "CategoryOption",
                newName: "CategoryOptions");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOption_ProductId",
                table: "ProductOptions",
                newName: "IX_ProductOptions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOption_CategoryOptionId",
                table: "ProductOptions",
                newName: "IX_ProductOptions_CategoryOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryOption_CategoryId",
                table: "CategoryOptions",
                newName: "IX_CategoryOptions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_GrandCategoryId",
                table: "Categories",
                newName: "IX_Categories_GrandCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOptions",
                table: "ProductOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GrandCategories",
                table: "GrandCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryOptions",
                table: "CategoryOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_GrandCategories_GrandCategoryId",
                table: "Categories",
                column: "GrandCategoryId",
                principalTable: "GrandCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryOptions_Categories_CategoryId",
                table: "CategoryOptions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptions_CategoryOptions_CategoryOptionId",
                table: "ProductOptions",
                column: "CategoryOptionId",
                principalTable: "CategoryOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptions_Products_ProductId",
                table: "ProductOptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_GrandCategories_GrandCategoryId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryOptions_Categories_CategoryId",
                table: "CategoryOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptions_CategoryOptions_CategoryOptionId",
                table: "ProductOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptions_Products_ProductId",
                table: "ProductOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOptions",
                table: "ProductOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GrandCategories",
                table: "GrandCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryOptions",
                table: "CategoryOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "ProductOptions",
                newName: "ProductOption");

            migrationBuilder.RenameTable(
                name: "GrandCategories",
                newName: "GrandCategory");

            migrationBuilder.RenameTable(
                name: "CategoryOptions",
                newName: "CategoryOption");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOptions_ProductId",
                table: "ProductOption",
                newName: "IX_ProductOption_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOptions_CategoryOptionId",
                table: "ProductOption",
                newName: "IX_ProductOption_CategoryOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryOptions_CategoryId",
                table: "CategoryOption",
                newName: "IX_CategoryOption_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_GrandCategoryId",
                table: "Category",
                newName: "IX_Category_GrandCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOption",
                table: "ProductOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GrandCategory",
                table: "GrandCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryOption",
                table: "CategoryOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_GrandCategory_GrandCategoryId",
                table: "Category",
                column: "GrandCategoryId",
                principalTable: "GrandCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryOption_Category_CategoryId",
                table: "CategoryOption",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOption_CategoryOption_CategoryOptionId",
                table: "ProductOption",
                column: "CategoryOptionId",
                principalTable: "CategoryOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOption_Products_ProductId",
                table: "ProductOption",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Category_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
