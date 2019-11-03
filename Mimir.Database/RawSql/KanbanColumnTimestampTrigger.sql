 CREATE TRIGGER KanbanColumnUpdated
    ON dbo.KanbanItems
    AFTER INSERT, UPDATE 
    AS
		UPDATE b
		SET b.Timestamp = SYSDATETIME()
		FROM KanbanBoards b 
		INNER JOIN KanbanColumns c ON b.ID = c.KanbanBoardID
		WHERE c.ID IN (SELECT ID FROM Inserted)