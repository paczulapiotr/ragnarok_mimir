using System;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MoveItem = Mimir.API.Commands.MoveKanbanItemCommandHandler;
using AddItem = Mimir.API.Commands.AddKanbanItemCommandHandler;
using RemoveItem = Mimir.API.Commands.RemoveKanbanItemCommandHandler;
using Moq;
using Mimir.Core.CommonExceptions;
using Mimir.Tests.Abstract;

namespace Mimir.Tests
{
    public class KanbanItemTests : KanbanBaseTests
    {
        // Adding Item
        [Test]
        public void Should_Add_Item_To_Column()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new AddItem(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.Where(x => x.Items.Any()).First();
            var itemName = "Item name";
            var itemsCount = column.Items.Count;

            // When
            handler.HandleAsync(
                new AddItem.Command(user.ID, board.ID, itemName, column.ID)).Wait();

            // Then
            var addedItem = column.Items.First(x => x.Name == itemName);
            Assert.IsTrue(column.Items.Any(x => x.Name == itemName));
            Assert.AreEqual(itemsCount + 1, column.Items.Count);
            Assert.AreEqual(itemsCount, addedItem.Index);

        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Item_With_No_Access()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new AddItem(repository, CreateAccessServiceMock(false, false).Object);
            var column = board.Columns.Where(x => x.Items.Any()).First();
            var itemName = "Item name";
            var itemsCount = column.Items.Count;

            // When/Then
            Assert.CatchAsync<ForbiddenException>(
                async () => await handler.HandleAsync(
                   new AddItem.Command(user.ID, board.ID, itemName, column.ID)));
        }

        // Moving Item
        [Test]
        public void Should_Move_Item_To_New_Index()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveItem(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.Where(x => x.Items.Any()).First();
            var items = column.Items.OrderBy(x => x.Index);
            var itemToMove = items.First();
            var lastIndex = items.Count() - 1;
            var expectedIndexes = items.Select(x => x.Index);
            // When
            handler.HandleAsync(
                new MoveItem.Command(user.ID, board.ID, itemToMove.ID, lastIndex, null, board.Timestamp)).Wait();

            // Then
            var actualIndexes = column.Items.OrderBy(x => x.Index).Select(x => x.Index);
            CollectionAssert.AreEqual(expectedIndexes, actualIndexes);
            Assert.AreEqual(lastIndex, itemToMove.Index);
        }

        [Test]
        public void Should_Move_Item_To_Another_Column()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveItem(repository, CreateAccessServiceMock().Object);
            var firstColumn = board.Columns.Where(x => x.Items.Any()).First();
            var lastColumn = board.Columns.Where(x => x.Items.Any()).Last();
            var items = firstColumn.Items.OrderBy(x => x.Index);
            var itemToMove = items.First();
            var lastIndex = lastColumn.Items.Count() - 1;
            var newIndex = 0;
            var expectedIndexes = lastColumn.Items.OrderBy(x => x.Index).Select(x => x.Index);
            expectedIndexes.Append(expectedIndexes.Count());

            // When
            handler.HandleAsync(
                new MoveItem.Command(user.ID, board.ID, itemToMove.ID, newIndex, lastColumn.ID, board.Timestamp)).Wait();

            // Then
            var actualIndexes = lastColumn.Items.OrderBy(x => x.Index).Select(x => x.Index);
            CollectionAssert.AreEqual(expectedIndexes, actualIndexes);
            Assert.AreEqual(newIndex, itemToMove.Index);
            Assert.AreEqual(lastColumn.ID, itemToMove.ColumnID);
            Assert.IsFalse(firstColumn.Items.Any(x => x.ID == itemToMove.ID));
        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Item_To_Column_From_Another_Board()
        {
            // Given
            var srcBoard = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var destBoard = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).Last();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveItem(repository, CreateAccessServiceMock().Object);
            var srcColumn = srcBoard.Columns.Where(x => x.Items.Any()).First();
            var destColumn = destBoard.Columns.Where(x => x.Items.Any()).Last();
            var itemToMove = srcColumn.Items.OrderBy(x => x.Index).First();
            var newIndex = 0;

            // When
            Assert.CatchAsync<ArgumentException>(
               async () => await handler.HandleAsync(
               new MoveItem.Command(user.ID, srcBoard.ID, itemToMove.ID, newIndex, destColumn.ID, srcBoard.Timestamp)));
        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Item_With_No_Access()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveItem(repository, CreateAccessServiceMock(false, false).Object);
            var srcColumn = board.Columns.Where(x => x.Items.Any()).First();
            var itemToMove = srcColumn.Items.OrderBy(x => x.Index).First();
            var newIndex = 0;

            // When
            Assert.CatchAsync<ForbiddenException>(
               async () => await handler.HandleAsync(
               new MoveItem.Command(user.ID, board.ID, itemToMove.ID, newIndex, srcColumn.ID, board.Timestamp)));
        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Item_With_Old_Timestamp()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).ThenInclude(x => x.Items).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveItem(repository, CreateAccessServiceMock().Object);
            var srcColumn = board.Columns.Where(x => x.Items.Any()).First();
            var itemToMove = srcColumn.Items.OrderBy(x => x.Index).First();
            var newIndex = 0;

            // When
            Assert.CatchAsync<ConflictException>(
               async () => await handler.HandleAsync(
               new MoveItem.Command(user.ID, board.ID, itemToMove.ID, newIndex, srcColumn.ID, DateTime.Now)));
        }

        // Removing Item
        [Test]
        public void Should_Remove_Item()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveItem(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.Where(x => x.Items.Any()).OrderBy(x => x.Index).First();
            var itemToRemove = column.Items.First();
            var itemsCount = column.Items.Count;
            var expectedIndexes = column.Items
                .OrderBy(x => x.Index)
                .Select(x => x.Index)
                .SkipLast(1)
                .ToList();
            
            // When
            handler.HandleAsync(
                   new RemoveItem.Command(user.ID, board.ID, itemToRemove.ID)).Wait();

            // Then
            var actualIndexes = column.Items.OrderBy(x => x.Index).Select(x => x.Index);
            Assert.IsFalse(column.Items.Any(x => x.ID == itemToRemove.ID));
            Assert.AreEqual(itemsCount - 1, column.Items.Count);
            CollectionAssert.AreEqual(expectedIndexes, actualIndexes);
        }

        [Test]
        public void Should_Throw_Exception_On_Removing_Item_From_Wrong_Board()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var wrongBoard = context.KanbanBoards.Include(x => x.Columns).Last();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveItem(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.Where(x => x.Items.Any()).OrderBy(x => x.Index).First();
            var itemToRemove = column.Items.First();

            // When
            Assert.CatchAsync<ArgumentException>(
               async () => await handler.HandleAsync(
               new RemoveItem.Command(user.ID, wrongBoard.ID, itemToRemove.ID)));
        }

        [Test]
        public void Should_Throw_Exception_On_Removing_Item_With_No_Access()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveItem(repository, CreateAccessServiceMock(false, false).Object);
            var column = board.Columns.Where(x => x.Items.Any()).OrderBy(x => x.Index).First();
            var itemToRemove = column.Items.First();

            // When
            Assert.CatchAsync<ForbiddenException>(
               async () => await handler.HandleAsync(
               new RemoveItem.Command(user.ID, board.ID, itemToRemove.ID)));
        }
    }
}