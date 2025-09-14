using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Backend.Migrations
{
    public partial class RemoveHandleFromPresetAndThemes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Handle",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "Handle",
                table: "Presets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Handle",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Handle",
                table: "Presets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
