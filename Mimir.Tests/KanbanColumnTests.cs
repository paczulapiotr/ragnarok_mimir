using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mimir.Core.CommonExceptions;
using NUnit.Framework;
using MoveColumn = Mimir.API.Commands.MoveKanbanColumnCommandHandler;
using AddColumn = Mimir.API.Commands.AddKanbanColumnCommandHandler;
using RemoveColumn = Mimir.API.Commands.RemoveKanbanColumnCommandHandler;
using Mimir.Tests.Abstract;

namespace Mimir.Tests
{
    public class KanbanColumnTests : KanbanBaseTests
    {
        //Adding Column
        [Test]
        public void Should_Add_Column_To_Board()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).FirstOrDefault();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new AddColumn(repository, CreateAccessServiceMock().Object);
            var columnsPreAdd = board.Columns.ToList();
            // When
            handler.HandleAsync(new AddColumn.Command(user.ID, board.ID, "New column")).Wait();

            // Then
            var indexes = board.Columns.OrderBy(x => x.Index).Select(x => x.Index);
            var addedColumn = board.Columns.First(x => x.Name == "New column");

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, indexes);
            Assert.IsNotNull(addedColumn);
            Assert.AreEqual(columnsPreAdd.Count, addedColumn.Index);
            Assert.IsTrue(board.Columns.Count == columnsPreAdd.Count + 1);

        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Column_With_Name_Duplicate()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).FirstOrDefault();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new AddColumn(repository, CreateAccessServiceMock().Object);
            var existingColumn = board.Columns.First();

            // When/Then
            Assert.CatchAsync<ArgumentException>(
                async () => await handler.HandleAsync(
                   new AddColumn.Command(user.ID, board.ID, existingColumn.Name)));
        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Column_With_No_Access()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).FirstOrDefault();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new AddColumn(repository, CreateAccessServiceMock(false, false).Object);

            // When/Then
            Assert.CatchAsync<ForbiddenException>(
                async () => await handler.HandleAsync(
                   new AddColumn.Command(user.ID, board.ID, "New column")));
        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Column_With_Old_Timestamp()
        {
            // Given
            var board = context.KanbanBoards.FirstOrDefault();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new AddColumn(repository, CreateAccessServiceMock(false, false).Object);

            // When/Then
            Assert.CatchAsync<ForbiddenException>(
                async () => await handler.HandleAsync(
                   new AddColumn.Command(user.ID, board.ID, "New column")));
        }


        // Moving Column
        [Test]
        public void Should_Move_Column_To_New_Index()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).FirstOrDefault();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveColumn(repository, CreateAccessServiceMock().Object);
            var firstColumn = board.Columns.OrderBy(x => x.Index).First();
            var newIndex = 1;

            // When
            handler.HandleAsync(
                   new MoveColumn.Command(user.ID, board.ID, firstColumn.ID, newIndex, board.Timestamp)).Wait();

            // Then
            var newIndexes = board.Columns.OrderBy(x => x.Index).Select(x => x.Index);
            Assert.AreEqual(newIndex, firstColumn.Index);
            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, newIndexes);
        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Column_To_Other_Board()
        {
            // Given
            var boards = context.KanbanBoards.Include(x => x.Columns);
            var firstBoard = boards.First();
            var lastBoard = boards.Last();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveColumn(repository, CreateAccessServiceMock().Object);
            var column = firstBoard.Columns.OrderBy(x => x.Index).First();
            var newIndex = 1;

            // When/Then
            Assert.CatchAsync<ArgumentException>(
                async () => await handler.HandleAsync(
                   new MoveColumn.Command(user.ID, lastBoard.ID, column.ID, newIndex, lastBoard.Timestamp)));
        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Column_With_No_Access()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveColumn(repository, CreateAccessServiceMock(false, false).Object);
            var column = board.Columns.OrderBy(x => x.Index).First();
            var newIndex = 1;

            // When/Then
            Assert.CatchAsync<ForbiddenException>(
                async () => await handler.HandleAsync(
                   new MoveColumn.Command(user.ID, board.ID, column.ID, newIndex, board.Timestamp)));
        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Column_With_Old_Timestamp()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new MoveColumn(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.OrderBy(x => x.Index).First();
            var newIndex = 1;

            // When/Then
            Assert.CatchAsync<ConflictException>(
                async () => await handler.HandleAsync(
                   new MoveColumn.Command(user.ID, board.ID, column.ID, newIndex, DateTime.Now)));
        }

        // Removing Column
        [Test]
        public void Should_Remove_Column()
        {
            // Given
            RemoveAllItems();
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveColumn(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.OrderBy(x => x.Index).Last();
            var columnCount = board.Columns.Count;
            // When
            handler.HandleAsync(
                   new RemoveColumn.Command(user.ID, board.ID, column.ID)).Wait();

            // Then
            Assert.IsFalse(board.Columns.Any(x => x.ID == column.ID));
            Assert.AreEqual(columnCount - 1, board.Columns.Count);
        }

        [Test]
        public void Should_Remap_Indexes_After_Column_Removal()
        {
            // Given
            RemoveAllItems();
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveColumn(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.OrderBy(x => x.Index).First(x => x.Index == 1);
            var columnId = column.ID;
            var columnCount = board.Columns.Count;

            // When
            handler.HandleAsync(
                   new RemoveColumn.Command(user.ID, board.ID, column.ID)).Wait();

            // Then
            var indexes = board.Columns.OrderBy(x => x.Index).Select(x => x.Index);
            Assert.IsFalse(board.Columns.Any(x => x.ID == columnId));
            Assert.AreEqual(new[] { 0, 1 }, indexes);
        }

        [Test]
        public void Should_Throw_Exception_On_Removing_Column_From_Wrong_Board()
        {
            // Given
            RemoveAllItems();
            var firstBoard = context.KanbanBoards.Include(x => x.Columns).First();
            var lastBoard = context.KanbanBoards.Include(x => x.Columns).Last();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveColumn(repository, CreateAccessServiceMock().Object);
            var column = firstBoard.Columns.OrderBy(x => x.Index).First(x => x.Index == 1);
            var columnId = column.ID;

            // When/Then
            Assert.CatchAsync<ArgumentException>(
                async () => await handler.HandleAsync(
                   new RemoveColumn.Command(user.ID, lastBoard.ID, column.ID)));
        }

        [Test]
        public void Should_Throw_Exception_On_Removing_Not_Existing_Column()
        {
            // Given
            RemoveAllItems();
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveColumn(repository, CreateAccessServiceMock().Object);
            var columnId = -1;

            // When/Then
            Assert.CatchAsync<ArgumentException>(
                async () => await handler.HandleAsync(
                   new RemoveColumn.Command(user.ID, board.ID, columnId)));
        }

        [Test]
        public void Should_Throw_Exception_On_Removing_Column_With_No_Access()
        {
            // Given
            RemoveAllItems();
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveColumn(repository, CreateAccessServiceMock(false, false).Object);
            var column = board.Columns.First();

            // When/Then
            Assert.CatchAsync<ForbiddenException>(
                async () => await handler.HandleAsync(
                   new RemoveColumn.Command(user.ID, board.ID, column.ID)));
        }

        [Test]
        public void Should_Throw_Exception_On_Removing_Column_With_Existing_Items()
        {
            // Given
            var board = context.KanbanBoards.Include(x => x.Columns).First();
            var user = context.AppUsers.FirstOrDefault();
            var handler = new RemoveColumn(repository, CreateAccessServiceMock().Object);
            var column = board.Columns.Where(x => x.Items.Any()).First();

            // When/Then
            Assert.CatchAsync<ArgumentException>(
                async () => await handler.HandleAsync(
                   new RemoveColumn.Command(user.ID, board.ID, column.ID)));
        }

    }
}
