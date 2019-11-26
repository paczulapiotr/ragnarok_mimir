using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Mimir.Core.Models;
using Mimir.Database;
using Mimir.Kanban;
using Moq;
using NUnit.Framework;

namespace Mimir.Tests.Abstract
{
    public abstract class KanbanBaseTests
    {
        protected SqlKanbanRepository repository;
        protected MimirDbContext context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MimirDbContext>()
                .UseInMemoryDatabase("Mimir").Options;
            var indexableHelper = new IndexableHelper();
            context = new MimirDbContext(options);
            repository = new SqlKanbanRepository(context, indexableHelper);

            #region Seed context
            var users = new[]
            {
                new AppUser
                {
                    AuthID ="user1",
                    Name="User Uno",
                },
                    new AppUser
                {
                    AuthID ="user2",
                    Name="User Duo",
                },
            };
            context.AppUsers.AddRange(users);
            context.SaveChanges();

            var boards = new[]
            {
                new KanbanBoard{
                    Name="Board One",
                    Description="Description One",
                    Owner=users[0],
                },
                new KanbanBoard{
                    Name="Board Two",
                    Description="Description Two",
                    Owner=users[1],
                }
            };
            context.KanbanBoards.AddRange(boards);
            context.SaveChanges();


            var columnsOne = new[]
            {
                new KanbanColumn{ Index=0, Name="Board_0_col_0", Board = boards[0] },
                new KanbanColumn{ Index=1, Name="Board_0_col_1", Board = boards[0] },
                new KanbanColumn{ Index=2, Name="Board_0_col_2", Board = boards[0] },
            };
            var columnsTwo = new[]
            {
                new KanbanColumn{ Index=0, Name="Board_1_col_0", Board = boards[1] },
                new KanbanColumn{ Index=1, Name="Board_1_col_1", Board = boards[1] },
                new KanbanColumn{ Index=2, Name="Board_1_col_2", Board = boards[1] },
            };
            context.KanbanColumns.AddRange(columnsOne);
            context.KanbanColumns.AddRange(columnsTwo);
            context.SaveChanges();

            var itemsOne = new[]
            {
                new KanbanItem
                {
                    Index = 0,
                    Name = "B_0_Col_0_item_1",
                    Column = columnsOne[0],
                },
                new KanbanItem
                {
                    Index = 1,
                    Name = "B_0_Col_0_item_2",
                    Column = columnsOne[0],
                },
                new KanbanItem
                {
                    Index = 2,
                    Name = "B_0_Col_0_item_3",
                    Column = columnsOne[0],
                },
                        new KanbanItem
                {
                    Index = 0,
                    Name = "B_0_Col_1_item_1",
                    Column = columnsOne[1],
                },
                new KanbanItem
                {
                    Index = 1,
                    Name = "B_0_Col_1_item_2",
                    Column = columnsOne[1],
                },
                new KanbanItem
                {
                    Index = 2,
                    Name = "B_0_Col_1_item_3",
                    Column = columnsOne[1],
                },
                new KanbanItem
                {
                    Index = 0,
                    Name = "B_1_Col_0_item_1",
                    Column = columnsTwo[0],
                },
                new KanbanItem
                {
                    Index = 1,
                    Name = "B_1_Col_0_item_2",
                    Column = columnsTwo[0],
                },
                new KanbanItem
                {
                    Index = 2,
                    Name = "B_1_Col_0_item_3",
                    Column = columnsTwo[0],
                },
                new KanbanItem
                {
                    Index = 0,
                    Name = "B_1_Col_1_item_1",
                    Column = columnsTwo[1],
                },
                new KanbanItem
                {
                    Index = 1,
                    Name = "B_1_Col_1_item_2",
                    Column = columnsTwo[1],
                },
                new KanbanItem
                {
                    Index = 2,
                    Name = "B_1_Col_1_item_3",
                    Column = columnsTwo[1],
                },
            };
            context.KanbanItems.AddRange(itemsOne);
            context.SaveChanges();
            #endregion
        }
        protected static Mock<IKanbanAccessService> CreateAccessServiceMock(bool hasAccess = true, bool isOwner = true)
        {
            var accessServiceMock = new Mock<IKanbanAccessService>();
            accessServiceMock.Setup(x => x.HasAccess(It.IsAny<int>(), It.IsAny<int>())).Returns(hasAccess);
            accessServiceMock.Setup(x => x.IsOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(isOwner);
            return accessServiceMock;
        }

        protected void RemoveAllItems()
        {
            context.RemoveRange(context.KanbanItems);
            context.SaveChanges();
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

    }
}
