using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class CommentCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AppUsers_AuthorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_KanbanItems_KanbanItemID",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_KanbanItemID",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "KanbanItemID",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ItemId",
                table: "Comment",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AppUsers_AuthorId",
                table: "Comment",
                column: "AuthorId",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_KanbanItems_ItemId",
                table: "Comment",
                column: "ItemId",
                principalTable: "KanbanItems",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AppUsers_AuthorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_KanbanItems_ItemId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ItemId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Comment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KanbanItemID",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_KanbanItemID",
                table: "Comment",
                column: "KanbanItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AppUsers_AuthorId",
                table: "Comment",
                column: "AuthorId",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_KanbanItems_KanbanItemID",
                table: "Comment",
                column: "KanbanItemID",
                principalTable: "KanbanItems",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
