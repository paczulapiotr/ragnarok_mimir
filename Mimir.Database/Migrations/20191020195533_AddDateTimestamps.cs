using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class AddDateTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Timestamp", table: "KanbanItems");
            migrationBuilder.DropColumn(name: "Timestamp", table: "KanbanColumns");
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "KanbanItems", 
                defaultValueSql: "SYSDATETIME()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "KanbanColumns",
                defaultValueSql: "SYSDATETIME()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "KanbanBoards",
                defaultValueSql: "SYSDATETIME()");

            migrationBuilder.Sql(@"
            CREATE TRIGGER KanbanItemModified
            ON dbo.KanbanItems
            AFTER UPDATE 
            AS
               UPDATE dbo.KanbanItems
               SET Timestamp = SYSDATETIME()
               FROM Inserted i
               WHERE dbo.KanbanItems.ID = i.ID");

            migrationBuilder.Sql(@"
            CREATE TRIGGER KanbanColumnModified
            ON dbo.KanbanColumns
            AFTER UPDATE 
            AS
               UPDATE dbo.KanbanColumns
               SET Timestamp = SYSDATETIME()
               FROM Inserted i
               WHERE dbo.KanbanColumns.ID = i.ID");

            migrationBuilder.Sql(@"
            CREATE TRIGGER KanbanBoardModified
            ON dbo.KanbanBoards
            AFTER UPDATE 
            AS
               UPDATE dbo.KanbanBoards
               SET Timestamp = SYSDATETIME()
               FROM Inserted i
               WHERE dbo.KanbanBoards.ID = i.ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "KanbanBoards");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Timestamp",
                table: "KanbanItems",
                type: "rowversion",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<byte[]>(
                name: "Timestamp",
                table: "KanbanColumns",
                type: "rowversion",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
