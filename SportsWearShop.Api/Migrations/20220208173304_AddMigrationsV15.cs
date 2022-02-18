using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsWearShop.Api.Migrations
{
    public partial class AddMigrationsV15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Basket",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Basket");
        }
    }
}
