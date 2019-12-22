using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class RenameCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AppUsers_AuthorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_KanbanItems_ItemId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanItems_AppUsers_CreatedByID",
                table: "KanbanItems");

            migrationBuilder.DropIndex(
                name: "IX_KanbanItems_CreatedByID",
                table: "KanbanItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CreatedByID",
                table: "KanbanItems");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ItemId",
                table: "Comments",
                newName: "IX_Comments_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AuthorId",
                table: "Comments",
                newName: "IX_Comments_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AppUsers_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_KanbanItems_ItemId",
                table: "Comments",
                column: "ItemId",
                principalTable: "KanbanItems",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AppUsers_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_KanbanItems_ItemId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ItemId",
                table: "Comment",
                newName: "IX_Comment_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorId",
                table: "Comment",
                newName: "IX_Comment_AuthorId");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByID",
                table: "KanbanItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_CreatedByID",
                table: "KanbanItems",
                column: "CreatedByID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanItems_AppUsers_CreatedByID",
                table: "KanbanItems",
                column: "CreatedByID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
