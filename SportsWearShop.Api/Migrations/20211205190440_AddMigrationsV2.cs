using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsWearShop.Api.Migrations
{
    public partial class AddMigrationsV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PictureId",
                table: "CategoryProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_PictureId",
                table: "CategoryProducts",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_Pictures_PictureId",
                table: "CategoryProducts",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_Pictures_PictureId",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_PictureId",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "CategoryProducts");
        }
    }
}
