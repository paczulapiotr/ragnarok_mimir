using Microsoft.EntityFrameworkCore;
using Mimir.Core.Models;
using System;
using System.Reflection;

namespace Mimir.Database
{
    public class MimirDbContext : DbContext
    {
        public MimirDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<KanbanBoard> KanbanBoards { get; set; }
        public DbSet<KanbanColumn> KanbanColumns { get; set; }
        public DbSet<KanbanItem> KanbanItems{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
