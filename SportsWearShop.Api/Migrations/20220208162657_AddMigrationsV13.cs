using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsWearShop.Api.Migrations
{
    public partial class AddMigrationsV13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_ProductSize_ProductSizeId",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_ProductSizeId",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "ProductSizeId",
                table: "CategoryProducts");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductSize",
                type: "bigint",
                nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Size",
            //    table: "Products",
            //    type: "text",
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_ProductId",
                table: "ProductSize",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Products_ProductId",
                table: "ProductSize",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Products_ProductId",
                table: "ProductSize");

            migrationBuilder.DropIndex(
                name: "IX_ProductSize_ProductId",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductSize");

            //migrationBuilder.DropColumn(
            //    name: "Size",
            //    table: "Products");

            migrationBuilder.AddColumn<long>(
                name: "ProductSizeId",
                table: "CategoryProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_ProductSizeId",
                table: "CategoryProducts",
                column: "ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_ProductSize_ProductSizeId",
                table: "CategoryProducts",
                column: "ProductSizeId",
                principalTable: "ProductSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
