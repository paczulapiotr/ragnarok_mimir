CREATE TRIGGER KanbanBoardUpdated
	ON dbo.KanbanBoards
	AFTER INSERT, UPDATE 
	AS
		UPDATE KanbanBoards
		SET Timestamp = SYSDATETIME()
		WHERE KanbanBoards.ID IN (SELECT ID FROM Inserted);