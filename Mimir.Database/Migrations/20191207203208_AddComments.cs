using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class AddComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanItems_AppUsers_AssigneeID",
                table: "KanbanItems");

            migrationBuilder.RenameColumn(
                name: "AssigneeID",
                table: "KanbanItems",
                newName: "AssigneeId");

            migrationBuilder.RenameIndex(
                name: "IX_KanbanItems_AssigneeID",
                table: "KanbanItems",
                newName: "IX_KanbanItems_AssigneeId");

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedOn = table.Column<DateTime>(nullable: true),
                    KanbanItemID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comment_AppUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_KanbanItems_KanbanItemID",
                        column: x => x.KanbanItemID,
                        principalTable: "KanbanItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_KanbanItemID",
                table: "Comment",
                column: "KanbanItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanItems_AppUsers_AssigneeId",
                table: "KanbanItems",
                column: "AssigneeId",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanItems_AppUsers_AssigneeId",
                table: "KanbanItems");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.RenameColumn(
                name: "AssigneeId",
                table: "KanbanItems",
                newName: "AssigneeID");

            migrationBuilder.RenameIndex(
                name: "IX_KanbanItems_AssigneeId",
                table: "KanbanItems",
                newName: "IX_KanbanItems_AssigneeID");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanItems_AppUsers_AssigneeID",
                table: "KanbanItems",
                column: "AssigneeID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
