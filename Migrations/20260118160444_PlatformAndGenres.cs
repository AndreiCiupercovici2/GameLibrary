using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLibrary.Migrations
{
    /// <inheritdoc />
    public partial class PlatformAndGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlatformID",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_PlatformID",
                table: "Game",
                column: "PlatformID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Platform_PlatformID",
                table: "Game",
                column: "PlatformID",
                principalTable: "Platform",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Platform_PlatformID",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_PlatformID",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "PlatformID",
                table: "Game");
        }
    }
}
