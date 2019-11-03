using Mimir.Kanban;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Mimir.Database;
using Mimir.Core.Models;
using System.Linq;

namespace Mimir.Tests
{
    [TestFixture]
    public class KanbanBoardTests
    {
        private SqlKanbanRepository SUT;
        private MimirDbContext context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MimirDbContext>().UseInMemoryDatabase("Mimir").Options;
            var indexableHelper = new IndexableHelper();
            context = new MimirDbContext(options);
            SUT = new SqlKanbanRepository(context, indexableHelper);

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
            }.ToList();
            var columnsTwo = new[]
            {
                new KanbanColumn{ Index=0, Name="Board_1_col_0", Board = boards[1] },
                new KanbanColumn{ Index=1, Name="Board_1_col_1", Board = boards[1] },
            }.ToList();
            context.KanbanColumns.AddRange(columnsOne);
            context.KanbanColumns.AddRange(columnsTwo);
            context.SaveChanges();


            var itemsOne = new[]
            {
                new KanbanItem
                {
                    Index = 0,
                    Name = "Col_0_item_1",
                    Column = columnsOne[0],
                },
                new KanbanItem
                {
                    Index = 1,
                    Name = "Col_0_item_2",
                    Column = columnsOne[0],
                },
                new KanbanItem
                {
                    Index = 2,
                    Name = "Col_0_item_3",
                    Column = columnsOne[0],
                },
                        new KanbanItem
                {
                    Index = 0,
                    Name = "Col_1_item_1",
                    Column = columnsOne[1],
                },
                new KanbanItem
                {
                    Index = 1,
                    Name = "Col_1_item_2",
                    Column = columnsOne[1],
                },
                new KanbanItem
                {
                    Index = 2,
                    Name = "Col_1_item_3",
                    Column = columnsOne[1],
                },
            };

            context.KanbanItems.AddRange(itemsOne);
            context.SaveChanges();
        }

        //Adding Column
        [Test]
        public void Should_Add_Column_To_Board()
        {
        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Column_With_Name_Duplicate()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Column_With_No_Access()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Column_With_Old_Timestamp()
        {

        }


        // Moving Column
        [Test]
        public void Should_Move_Column_To_New_Index()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Column_To_Other_Board()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Column_With_No_Access()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Column_With_Old_Timestamp()
        {

        }

        // Adding Item

        [Test]
        public void Should_Add_Item_To_Column()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Item_With_No_Access()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Adding_Item_With_Old_Timestamp()
        {

        }

        // Moving Item
        [Test]
        public void Should_Move_Item_To_New_Index()
        {

        }

        [Test]
        public void Should_Move_Item_To_Another_Column()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Item_With_No_Access()
        {

        }

        [Test]
        public void Should_Throw_Exception_On_Moving_Item_With_Old_Timestamp()
        {

        }
    }
}