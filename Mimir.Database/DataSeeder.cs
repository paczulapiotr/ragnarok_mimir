using Microsoft.Extensions.DependencyInjection;
using Mimir.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mimir.Database
{
    public static class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<MimirDbContext>();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
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
                          new AppUser
                        {
                            AuthID ="user3",
                            Name="User Trio",
                        }
                    };
                    if (!context.AppUsers.Any())
                    {
                        context.AppUsers.AddRange(users);
                        context.SaveChanges();
                    }

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
                    if (!context.KanbanBoards.Any())
                    {
                        context.KanbanBoards.AddRange(boards);
                        context.SaveChanges();
                    }


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
                    if (!context.KanbanColumns.Any())
                    {
                        context.KanbanColumns.AddRange(columnsOne.Concat(columnsTwo));
                        context.SaveChanges();
                    }


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

                    if (!context.KanbanItems.Any())
                    {
                        context.KanbanItems.AddRange(itemsOne);
                        context.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

    }
}
