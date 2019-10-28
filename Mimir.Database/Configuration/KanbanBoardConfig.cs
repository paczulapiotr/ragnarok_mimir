using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mimir.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mimir.Database.Configuration
{
    public class KanbanBoardConfig : IEntityTypeConfiguration<KanbanBoard>
    {
        public void Configure(EntityTypeBuilder<KanbanBoard> builder)
        {
            builder.HasOne(x => x.Owner).WithMany(x => x.OwnedBoards);
            builder.HasMany(x => x.Columns).WithOne(x => x.Board).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
