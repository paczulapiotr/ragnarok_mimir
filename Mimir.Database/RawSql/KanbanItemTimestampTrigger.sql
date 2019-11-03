 CREATE TRIGGER KanbanItemUpdated
	ON dbo.KanbanItems
	AFTER INSERT, UPDATE 
	AS
		UPDATE b
		SET b.Timestamp = SYSDATETIME()
		FROM KanbanBoards b 
		INNER JOIN KanbanColumns c ON b.ID = c.KanbanBoardID
		INNER JOIN KanbanItems i ON c.ID = i.ColumnID
		WHERE i.ID IN (SELECT ID FROM Inserted);