using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class AddBoardNavProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanColumns_KanbanBoards_KanbanBoardID",
                table: "KanbanColumns");

            migrationBuilder.AlterColumn<int>(
                name: "KanbanBoardID",
                table: "KanbanColumns",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanColumns_KanbanBoards_KanbanBoardID",
                table: "KanbanColumns",
                column: "KanbanBoardID",
                principalTable: "KanbanBoards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanColumns_KanbanBoards_KanbanBoardID",
                table: "KanbanColumns");

            migrationBuilder.AlterColumn<int>(
                name: "KanbanBoardID",
                table: "KanbanColumns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanColumns_KanbanBoards_KanbanBoardID",
                table: "KanbanColumns",
                column: "KanbanBoardID",
                principalTable: "KanbanBoards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
