using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mimir.Core.Models;

namespace Mimir.Database.Configuration
{
    class KanbanItemConfig : IEntityTypeConfiguration<KanbanItem>
    {
        public void Configure(EntityTypeBuilder<KanbanItem> builder)
        {
            builder.HasMany(x => x.Comments).WithOne(x => x.Item).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
