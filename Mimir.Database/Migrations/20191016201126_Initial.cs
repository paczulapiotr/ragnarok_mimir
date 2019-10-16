using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthID = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KanbanBoards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OwnerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanBoards", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KanbanBoards_AppUsers_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KanbanBoardAccess",
                columns: table => new
                {
                    UserWithAccessID = table.Column<int>(nullable: false),
                    BoardID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanBoardAccess", x => new { x.BoardID, x.UserWithAccessID });
                    table.ForeignKey(
                        name: "FK_KanbanBoardAccess_AppUsers_BoardID",
                        column: x => x.BoardID,
                        principalTable: "AppUsers",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_KanbanBoardAccess_KanbanBoards_UserWithAccessID",
                        column: x => x.UserWithAccessID,
                        principalTable: "KanbanBoards",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "KanbanColumns",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Index = table.Column<int>(nullable: false),
                    KanbanBoardID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanColumns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KanbanColumns_KanbanBoards_KanbanBoardID",
                        column: x => x.KanbanBoardID,
                        principalTable: "KanbanBoards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KanbanItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Index = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AssigneeID = table.Column<int>(nullable: true),
                    CreatedByID = table.Column<int>(nullable: true),
                    KanbanColumnID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KanbanItems_AppUsers_AssigneeID",
                        column: x => x.AssigneeID,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KanbanItems_AppUsers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KanbanItems_KanbanColumns_KanbanColumnID",
                        column: x => x.KanbanColumnID,
                        principalTable: "KanbanColumns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardAccess_UserWithAccessID",
                table: "KanbanBoardAccess",
                column: "UserWithAccessID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoards_OwnerID",
                table: "KanbanBoards",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanColumns_KanbanBoardID",
                table: "KanbanColumns",
                column: "KanbanBoardID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_AssigneeID",
                table: "KanbanItems",
                column: "AssigneeID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_CreatedByID",
                table: "KanbanItems",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanItems_KanbanColumnID",
                table: "KanbanItems",
                column: "KanbanColumnID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KanbanBoardAccess");

            migrationBuilder.DropTable(
                name: "KanbanItems");

            migrationBuilder.DropTable(
                name: "KanbanColumns");

            migrationBuilder.DropTable(
                name: "KanbanBoards");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
