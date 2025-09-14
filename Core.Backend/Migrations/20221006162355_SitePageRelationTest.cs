using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Backend.Migrations
{
    public partial class SitePageRelationTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pages_ParentPageId",
                table: "Pages",
                column: "ParentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_SiteId",
                table: "Pages",
                column: "SiteId");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Pages_ParentPageId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Sites_SiteId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_ParentPageId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_SiteId",
                table: "Pages");
        }
    }
}
