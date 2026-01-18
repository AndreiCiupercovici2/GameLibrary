using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLibrary.Migrations
{
    /// <inheritdoc />
    public partial class GamePlatformChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameID = table.Column<int>(type: "int", nullable: false),
                    PlatformID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Platform_PlatformID",
                        column: x => x.PlatformID,
                        principalTable: "Platform",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_GameID",
                table: "GamePlatform",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformID",
                table: "GamePlatform",
                column: "PlatformID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePlatform");

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
    }
}
