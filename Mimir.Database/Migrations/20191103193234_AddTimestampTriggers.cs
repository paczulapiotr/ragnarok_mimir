using Microsoft.EntityFrameworkCore.Migrations;

namespace Mimir.Database.Migrations
{
    public partial class AddTimestampTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TRIGGER KanbanBoardUpdated
	            ON dbo.KanbanBoards
	            AFTER INSERT, UPDATE 
	            AS
                BEGIN
                    SET NOCOUNT ON;
		            UPDATE KanbanBoards
		            SET Timestamp = SYSDATETIME()
		            WHERE KanbanBoards.ID IN (SELECT ID FROM Inserted);
                END
            
            GO

            CREATE TRIGGER KanbanColumnUpdated
                ON dbo.KanbanColumns
                AFTER INSERT, UPDATE 
                AS
                BEGIN
                    SET NOCOUNT ON;
		            UPDATE b
		            SET b.Timestamp = SYSDATETIME()
		            FROM KanbanBoards b 
		            INNER JOIN KanbanColumns c ON b.ID = c.KanbanBoardID
		            WHERE c.ID IN (SELECT ID FROM Inserted)
                END
            GO

            CREATE TRIGGER KanbanItemUpdated
	            ON dbo.KanbanItems
	            AFTER INSERT, UPDATE 
	            AS
                BEGIN
                    SET NOCOUNT ON;
		            UPDATE b
		            SET b.Timestamp = SYSDATETIME()
		            FROM KanbanBoards b 
		            INNER JOIN KanbanColumns c ON b.ID = c.KanbanBoardID
		            INNER JOIN KanbanItems i ON c.ID = i.ColumnID
		            WHERE i.ID IN (SELECT ID FROM Inserted);
                END
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
