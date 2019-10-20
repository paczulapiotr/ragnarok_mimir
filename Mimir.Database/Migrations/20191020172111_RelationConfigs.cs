using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class RelationConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanItems_KanbanColumns_KanbanColumnID",
                table: "KanbanItems");

            migrationBuilder.DropIndex(
                name: "IX_KanbanItems_KanbanColumnID",
                table: "KanbanItems");

            migrationBuilder.DropColumn(
                name: "KanbanColumnID",
                table: "KanbanItems");

            migrationBuilder.AddColumn<int>(
                name: "ColumnID",
                table: "KanbanItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "KanbanColumns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_ColumnID",
                table: "KanbanItems",
                column: "ColumnID");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanItems_KanbanColumns_ColumnID",
                table: "KanbanItems",
                column: "ColumnID",
                principalTable: "KanbanColumns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanItems_KanbanColumns_ColumnID",
                table: "KanbanItems");

            migrationBuilder.DropIndex(
                name: "IX_KanbanItems_ColumnID",
                table: "KanbanItems");

            migrationBuilder.DropColumn(
                name: "ColumnID",
                table: "KanbanItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "KanbanColumns");

            migrationBuilder.AddColumn<int>(
                name: "KanbanColumnID",
                table: "KanbanItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_KanbanColumnID",
                table: "KanbanItems",
                column: "KanbanColumnID");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanItems_KanbanColumns_KanbanColumnID",
                table: "KanbanItems",
                column: "KanbanColumnID",
                principalTable: "KanbanColumns",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
