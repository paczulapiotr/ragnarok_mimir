using Mimir.Core.Models;
using Mimir.Database;
using System;
using System.Linq;

namespace Mimir.Kanban
{
    public class SqlKanbanRepository : IKanbanRepository
    {
        private readonly MimirDbContext _dbContext;
        private readonly IIndexableHelper _indexableHelper;

        public SqlKanbanRepository(MimirDbContext dbContext, IIndexableHelper indexableHelper)
        {
            _dbContext = dbContext;
            _indexableHelper = indexableHelper;
        }

        private int GetLastColumnIndex(int boardId)
        {
            return _dbContext.KanbanColumns
                .Where(x => x.KanbanBoardID == boardId)
                .Max(x => x.Index) + 1;
        }

        private int GetLastItemIndex(int columndId)
        {
            return _dbContext.KanbanItems
                .Where(x => x.ColumnID == columndId)
                .Max(x=>x.Index) + 1;
        }

     

        public bool AddColumn(int boardId, string name, DateTime timestamp)
        {
            var newColumn = new KanbanColumn
            {
                Name = name,
                Index = GetLastColumnIndex(boardId),
                KanbanBoardID = boardId,
            };

            _dbContext.KanbanColumns.Add(newColumn);
            _dbContext.SaveChanges();
            return true;
        }

        public bool AddItem(int boardId, string name, int columnId, DateTime timestamp)
        {
            var newItem = new KanbanItem
            {
                Name = name,
                Index = GetLastItemIndex(columnId),
                ColumnID = columnId,
            };

            _dbContext.KanbanItems.Add(newItem);
            _dbContext.SaveChanges();
            return true;
        }

        public bool MoveColumn(int boardId, int columnId, int index, DateTime timestamp)
        {
            var columns = _dbContext.KanbanColumns
                .Where(x => x.KanbanBoardID == boardId)
                .ToList();

            var columnToMove = columns.FirstOrDefault(x => x.ID == columnId);
            var columnsToRemap = columns.Where(x => x.Index >= columnToMove.Index);

            _indexableHelper.ReorderIndexable(columnsToRemap, columnToMove.Index, index);
            return true;
        }

        public bool MoveItem(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp)
        {
            var items = _dbContext.KanbanItems
                .Where(x => x.ColumnID == destColumnId || x.ID == itemId)
                .ToList();
            var item = items.FirstOrDefault(x => x.ID == itemId);

            if (!destColumnId.HasValue || item.ColumnID == destColumnId)
            {
                _indexableHelper.ReorderIndexable(items, item.Index, index);
            }
            else
            {
                item.ColumnID = destColumnId.Value;
                _indexableHelper.MoveIndexable(items, item, index);
            }
            _dbContext.SaveChanges();
            return true; 
        }

        public void RemoveItem(int boardId, int itemId, DateTime timestamp)
        {
            var item = _dbContext.KanbanItems.FirstOrDefault(x => x.ID == itemId);
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }

        public void RemoveColumn(int boardId, int columnId, DateTime timestamp)
        {
            var column = _dbContext.KanbanColumns.FirstOrDefault(x => x.ID == columnId);
            if (column.Items.Any())
                return;
            
            _dbContext.Remove(column);
            _dbContext.SaveChanges();
        }
    }
}
