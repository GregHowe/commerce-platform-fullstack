using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Backend.Migrations
{
    public partial class ContentLibrary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Pages_ParentPageId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Sites_SiteId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Brands_BrandId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Brands_BrandId",
                table: "Sites");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Pages_HomepageId",
                table: "Sites");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Themes_ThemeId",
                table: "Sites");

            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Brands_BrandId",
                table: "Themes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Brands_BrandId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "BrandPermission");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "PermissionSeat");

            migrationBuilder.DropTable(
                name: "RoleSeat");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Users_BrandId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Themes_BrandId",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_Sites_BrandId",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_HomepageId",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_ThemeId",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Roles_BrandId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Pages_ParentPageId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_SiteId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ThemeId",
                table: "Sites",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Pages",
                newName: "Handle");

            migrationBuilder.RenameColumn(
                name: "Redirect",
                table: "Pages",
                newName: "RedirectUrl");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Style",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Handle",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Themes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Themes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HomepageId",
                table: "Sites",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeoIsPrivate",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Presets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Handle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Handle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Handle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ParentTopicId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Presets");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropColumn(
                name: "Handle",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "Settings",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "Settings",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "SeoIsPrivate",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Sites",
                newName: "ThemeId");

            migrationBuilder.RenameColumn(
                name: "RedirectUrl",
                table: "Pages",
                newName: "Redirect");

            migrationBuilder.RenameColumn(
                name: "Handle",
                table: "Pages",
                newName: "Slug");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Style",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HomepageId",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomepageForId",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BrandPermission",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    PermissionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandPermission", x => new { x.BrandId, x.PermissionsId });
                    table.ForeignKey(
                        name: "FK_BrandPermission_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BrandPermission_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissionSeat",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    SeatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionSeat", x => new { x.PermissionsId, x.SeatsId });
                    table.ForeignKey(
                        name: "FK_PermissionSeat_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissionSeat_Seats_SeatsId",
                        column: x => x.SeatsId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleSeat",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    SeatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSeat", x => new { x.RolesId, x.SeatsId });
                    table.ForeignKey(
                        name: "FK_RoleSeat_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleSeat_Seats_SeatsId",
                        column: x => x.SeatsId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BrandId",
                table: "Users",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_BrandId",
                table: "Themes",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_BrandId",
                table: "Sites",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_HomepageId",
                table: "Sites",
                column: "HomepageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_ThemeId",
                table: "Sites",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_BrandId",
                table: "Roles",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ParentPageId",
                table: "Pages",
                column: "ParentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_SiteId",
                table: "Pages",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandPermission_PermissionsId",
                table: "BrandPermission",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesId",
                table: "PermissionRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSeat_SeatsId",
                table: "PermissionSeat",
                column: "SeatsId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSeat_SeatsId",
                table: "RoleSeat",
                column: "SeatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SiteId",
                table: "Seats",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_UserId",
                table: "Seats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Pages_ParentPageId",
                table: "Pages",
                column: "ParentPageId",
                principalTable: "Pages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Sites_SiteId",
                table: "Pages",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Brands_BrandId",
                table: "Roles",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Brands_BrandId",
                table: "Sites",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Pages_HomepageId",
                table: "Sites",
                column: "HomepageId",
                principalTable: "Pages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Themes_ThemeId",
                table: "Sites",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Brands_BrandId",
                table: "Themes",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Brands_BrandId",
                table: "Users",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }
    }
}
