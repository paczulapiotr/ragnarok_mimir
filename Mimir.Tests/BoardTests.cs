using System.Linq;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mimir.API.Commands;
using Mimir.Core.Models;
using Mimir.Database;
using NUnit.Framework;

namespace Mimir.Tests
{
    public class BoardTests
    {
        private MimirDbContext context;
        private AppUser[] users;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MimirDbContext>()
               .UseInMemoryDatabase("Mimir")
               .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;
            context = new MimirDbContext(options);

            //Seed users
            users = new[]
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
                new AppUser
                {
                    AuthID ="user3",
                    Name="User Trio",
                },
            };
            context.AppUsers.AddRange(users);
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
            users = null;
        }

        [Test]
        public void Should_Create_Board_With_Participants()
        {
            // Given
            var boardName = "test board";
            var participantsIds = users.Skip(1).Select(x => x.ID);
            var userId = users.First().ID;
            var handler = new CreateBoardCommandHandler(context);
            var command = new CreateBoardCommandHandler.Command
            {
                Name = boardName,
                ParticipantIds = participantsIds,
                UserId = userId
            };
            
            // When
            handler.HandleAsync(command).Wait();

            // Then
            var board = context.KanbanBoards.First();
            Assert.AreEqual(boardName, board.Name);
            Assert.AreEqual(userId, board.OwnerID);
            CollectionAssert.AreEquivalent(participantsIds, board.UsersWithAccess.Select(x => x.UserWithAccessID));
        }
    }
}
