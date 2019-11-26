using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class FixKanbanBoardAccessEntityConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardAccess_AppUsers_BoardID",
                table: "KanbanBoardAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardAccess_KanbanBoards_UserWithAccessID",
                table: "KanbanBoardAccess");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardAccess_KanbanBoards_BoardID",
                table: "KanbanBoardAccess",
                column: "BoardID",
                principalTable: "KanbanBoards",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardAccess_AppUsers_UserWithAccessID",
                table: "KanbanBoardAccess",
                column: "UserWithAccessID",
                principalTable: "AppUsers",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardAccess_KanbanBoards_BoardID",
                table: "KanbanBoardAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardAccess_AppUsers_UserWithAccessID",
                table: "KanbanBoardAccess");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardAccess_AppUsers_BoardID",
                table: "KanbanBoardAccess",
                column: "BoardID",
                principalTable: "AppUsers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardAccess_KanbanBoards_UserWithAccessID",
                table: "KanbanBoardAccess",
                column: "UserWithAccessID",
                principalTable: "KanbanBoards",
                principalColumn: "ID");
        }
    }
}
