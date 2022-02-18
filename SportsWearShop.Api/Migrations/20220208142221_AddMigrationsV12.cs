using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsWearShop.Api.Migrations
{
    public partial class AddMigrationsV12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "ProductSizeId",
                table: "Products");

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ProductSizeId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
