using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaskManagement.Api.Entities;

namespace TaskManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Lookup> Lookups => Set<Lookup>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User: unique Email + Username
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.HashedPassword)
                      .IsRequired();
            });

            // TaskItem relationships
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Status)
                      .WithMany(l => l.Tasks)
                      .HasForeignKey(t => t.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Lookup seeding – Task Status
            modelBuilder.Entity<Lookup>().HasData(
                new Lookup { Id = 1, MajorCode = 1, MinorCode = 0, Name = "Task Status" },
                new Lookup { Id = 2, MajorCode = 1, MinorCode = 1, Name = "Initiated" },
                new Lookup { Id = 3, MajorCode = 1, MinorCode = 2, Name = "In Progress" },
                new Lookup { Id = 4, MajorCode = 1, MinorCode = 3, Name = "Completed" },
                new Lookup { Id = 5, MajorCode = 1, MinorCode = 4, Name = "Cancelled" }
            );
        }
    }
}
