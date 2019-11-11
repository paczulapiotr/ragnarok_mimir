﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mimir.Database;

namespace Mimir.Database.Migrations
{
    [DbContext(typeof(MimirDbContext))]
    partial class MimirDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mimir.Core.Models.AppUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanBoard", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OwnerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.ToTable("KanbanBoards");
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanBoardAccess", b =>
                {
                    b.Property<int>("BoardID")
                        .HasColumnType("int");

                    b.Property<int>("UserWithAccessID")
                        .HasColumnType("int");

                    b.HasKey("BoardID", "UserWithAccessID");

                    b.HasIndex("UserWithAccessID");

                    b.ToTable("KanbanBoardAccess");
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanColumn", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("KanbanBoardID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("KanbanBoardID");

                    b.ToTable("KanbanColumns");
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssigneeID")
                        .HasColumnType("int");

                    b.Property<int>("ColumnID")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AssigneeID");

                    b.HasIndex("ColumnID");

                    b.HasIndex("CreatedByID");

                    b.ToTable("KanbanItems");
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanBoard", b =>
                {
                    b.HasOne("Mimir.Core.Models.AppUser", "Owner")
                        .WithMany("OwnedBoards")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanBoardAccess", b =>
                {
                    b.HasOne("Mimir.Core.Models.KanbanBoard", "Board")
                        .WithMany("UsersWithAccess")
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Mimir.Core.Models.AppUser", "UserWithAccess")
                        .WithMany("BoardsWithAccess")
                        .HasForeignKey("UserWithAccessID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanColumn", b =>
                {
                    b.HasOne("Mimir.Core.Models.KanbanBoard", "Board")
                        .WithMany("Columns")
                        .HasForeignKey("KanbanBoardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mimir.Core.Models.KanbanItem", b =>
                {
                    b.HasOne("Mimir.Core.Models.AppUser", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeID");

                    b.HasOne("Mimir.Core.Models.KanbanColumn", "Column")
                        .WithMany("Items")
                        .HasForeignKey("ColumnID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mimir.Core.Models.AppUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByID");
                });
#pragma warning restore 612, 618
        }
    }
}
