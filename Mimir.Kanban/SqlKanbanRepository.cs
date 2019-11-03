using Mimir.Core.Models;
using Mimir.Database;
using System;
using System.Linq;
using System.Threading.Tasks;

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

     

        public async Task AddColumnAsync(int boardId, string name, DateTime timestamp)
        {
            var newColumn = new KanbanColumn
            {
                Name = name,
                Index = GetLastColumnIndex(boardId),
                KanbanBoardID = boardId,
            };

            _dbContext.KanbanColumns.Add(newColumn);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddItemAsync(int boardId, string name, int columnId, DateTime timestamp)
        {
            var newItem = new KanbanItem
            {
                Name = name,
                Index = GetLastItemIndex(columnId),
                ColumnID = columnId,
            };

            _dbContext.KanbanItems.Add(newItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task MoveColumnAsync(int boardId, int columnId, int index, DateTime timestamp)
        {
            var columns = _dbContext.KanbanColumns
                .Where(x => x.KanbanBoardID == boardId)
                .ToList();

            var columnToMove = columns.FirstOrDefault(x => x.ID == columnId);
            var columnsToRemap = columns.Where(x => x.Index >= columnToMove.Index);

            _indexableHelper.ReorderIndexable(columnsToRemap, columnToMove.Index, index);

            await _dbContext.SaveChangesAsync();
        }

        public async Task MoveItemAsync(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp)
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
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int boardId, int itemId, DateTime timestamp)
        {
            var item = _dbContext.KanbanItems.FirstOrDefault(x => x.ID == itemId);
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveColumnAsync(int boardId, int columnId, DateTime timestamp)
        {
            var column = _dbContext.KanbanColumns.FirstOrDefault(x => x.ID == columnId);
            if (column.Items.Any())
                return;
            
            _dbContext.Remove(column);
            await _dbContext.SaveChangesAsync();
        }
    }
}
