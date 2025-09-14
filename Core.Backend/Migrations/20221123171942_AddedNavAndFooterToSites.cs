using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Backend.Migrations
{
    public partial class AddedNavAndFooterToSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Navigation",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Footer",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Navigation",
                table: "Sites");
        }
    }
}
