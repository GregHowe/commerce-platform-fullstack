using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Backend.Migrations
{
    public partial class ContentLibraryForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Handle",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Handle",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "Handle",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Handle",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Handle",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Handle",
                table: "Permissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Handle",
                table: "Roles",
                column: "Handle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Handle",
                table: "Permissions",
                column: "Handle",
                unique: true);
        }
    }
}
