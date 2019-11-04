using Microsoft.EntityFrameworkCore;
using Mimir.Core.CommonExceptions;
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

        private void VerifyTimestamp(int boardId, DateTime timestamp)
        {
            var board = _dbContext.KanbanBoards.AsNoTracking().FirstOrDefault(x => x.ID == boardId);
            if (board == null)
                throw new ArgumentException("Board doesn't exist");
            if (!board.CompareTimestamp(timestamp))
                throw new ConflictException("Board version is different");
        }

        private void VerifyColumn(int boardId, int columnId)
        {
            var validColumnMove = _dbContext.KanbanColumns.Any(x => x.ID == columnId && x.KanbanBoardID == boardId);
            if (!validColumnMove)
                throw new ArgumentException("Column, Board arguments conflict");
        }

        private void VerifyItem(int boardId, int itemId, int? destColumnId = null)
        {
            var validItem = _dbContext.KanbanItems.Include(x=>x.Column)
                .Any(x => x.ID == itemId && x.Column.KanbanBoardID == boardId);
            if (!validItem)
                throw new ArgumentException("Item, Board arguments conflict");

            if (!destColumnId.HasValue)
                return;

            var validDestColumn = _dbContext.KanbanColumns
                .Any(x => x.ID == destColumnId.Value && x.KanbanBoardID == boardId);

            if(!validDestColumn)
                throw new ArgumentException("Destination Column, Board arguments conflict");


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
                .Max(x => x.Index) + 1;
        }

        public async Task AddColumnAsync(int boardId, string name, DateTime timestamp)
        {
            VerifyTimestamp(boardId, timestamp);

            var newColumn = new KanbanColumn
            {
                Name = name,
                Index = GetLastColumnIndex(boardId),
                KanbanBoardID = boardId,
            };

            var existingDuplicate = _dbContext.KanbanColumns
                .Where(x => x.KanbanBoardID == boardId)
                .Any(x => x.Name == name);

            if (existingDuplicate)
                throw new ArgumentException("Column name duplicate");

            _dbContext.KanbanColumns.Add(newColumn);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddItemAsync(int boardId, string name, int columnId, DateTime timestamp)
        {
            VerifyTimestamp(boardId, timestamp);

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
            VerifyTimestamp(boardId, timestamp);
            VerifyColumn(boardId, columnId);

            var columns = _dbContext.KanbanColumns
                .Where(x => x.KanbanBoardID == boardId)
                .ToList();

            var columnToMove = columns.FirstOrDefault(x => x.ID == columnId);

            _indexableHelper.MoveIndexable(columns, columnToMove.Index, index);

            await _dbContext.SaveChangesAsync();
        }

        public async Task MoveItemAsync(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp)
        {
            VerifyTimestamp(boardId, timestamp);
            VerifyItem(boardId, itemId, destColumnId);

            var itemToMove = _dbContext.KanbanItems
                .Select(x => new { x.ID, x.ColumnID })
                .FirstOrDefault(x => x.ID == itemId);
            if (itemToMove == null)
                throw new ArgumentException("Item does not exist");
            
            var destColumn = destColumnId ?? itemToMove.ColumnID;

            if (destColumn != itemToMove.ColumnID)
            {
                MoveItemToAnotherColumn(itemToMove.ColumnID, destColumn, itemId, index);
            }
            else
            {
                MoveItemToIndex(itemToMove.ColumnID, itemId, index);
            }
            await _dbContext.SaveChangesAsync();
        }

        private void MoveItemToAnotherColumn(int sourceColumnId, int destColumnId, int itemId, int index)
        {
            var items = _dbContext.KanbanItems
                   .Where(x => x.ColumnID == destColumnId || x.ColumnID == sourceColumnId)
                   .ToList();
            var toMove = items.FirstOrDefault(x => x.ID == itemId);
            if (toMove == null)
                throw new ArgumentException("Item does not exist");

            var destinationItems = items.Where(x => x.ColumnID == destColumnId).ToList();
            var sourceItems = items.Where(x => x.ColumnID == toMove.ColumnID).ToList();
            sourceItems.Remove(toMove);
            toMove.ColumnID = destColumnId;

            _indexableHelper.RemapIndexes(sourceItems);
            _indexableHelper.AddIndexable(destinationItems, toMove, index);
        }

        private void MoveItemToIndex(int columnId, int itemId, int index)
        {
            var items = _dbContext.KanbanItems
                  .Where(x => x.ColumnID == columnId)
                  .ToList();
            var toMove = items.FirstOrDefault(x => x.ID == itemId);
            if (toMove == null)
                throw new ArgumentException("Item does not exist");

            _indexableHelper.MoveIndexable(items, toMove.Index, index);
        }
        public async Task RemoveItemAsync(int boardId, int itemId, DateTime timestamp)
        {
            VerifyTimestamp(boardId, timestamp);
            VerifyItem(boardId, itemId);

            var item = _dbContext.KanbanItems.FirstOrDefault(x => x.ID == itemId);
            if (item == null)
                throw new ArgumentException("Item, Board arguments conflict");

            var itemsToRemap = _dbContext.KanbanItems
                .Where(x => x.ColumnID == item.ColumnID)
                .ToList();
            itemsToRemap.Remove(item);
            _dbContext.Remove(item);
            _indexableHelper.RemapIndexes(itemsToRemap);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveColumnAsync(int boardId, int columnId, DateTime timestamp)
        {
            VerifyTimestamp(boardId, timestamp);
            VerifyColumn(boardId, columnId);

            var columns = _dbContext.KanbanColumns.Where(x => x.KanbanBoardID == boardId).ToList();
            var column = columns.FirstOrDefault(x => x.ID == columnId);
            if (column.Items.Any())
                throw new ArgumentException("Cannot remove column with existing items");

            columns.Remove(column);
            _dbContext.Remove(column);

            _indexableHelper.RemapIndexes(columns);
            await _dbContext.SaveChangesAsync();
        }
    }
}
