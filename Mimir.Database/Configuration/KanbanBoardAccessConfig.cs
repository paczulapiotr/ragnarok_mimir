using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mimir.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mimir.Database.Configuration
{
    public class KanbanBoardAccessConfig : IEntityTypeConfiguration<KanbanBoardAccess>
    {
        public void Configure(EntityTypeBuilder<KanbanBoardAccess> builder)
        {
            builder.HasKey(x => new { x.BoardID, x.UserWithAccessID });

            builder.HasOne(x => x.Board)
                .WithMany(x => x.UsersWithAccess)
                .HasForeignKey(x => x.BoardID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UserWithAccess)
                .WithMany(x => x.BoardsWithAccess)
                .HasForeignKey(x => x.UserWithAccessID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
