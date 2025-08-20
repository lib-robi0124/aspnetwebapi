using FluentAPI.Domain.Models;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;

namespace FluentAPI.DataAccess
{
    public class FluentApiDbContext : DbContext
    {
        public FluentApiDbContext(DbContextOptions<FluentApiDbContext> options) : base(options)
        {
        }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>()
                .Property(n => n.Text)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Note>()
                .Property(n => n.Priority)
                .IsRequired();
            modelBuilder.Entity<Note>()
                .Property(n => n.Tag)
                .IsRequired();

            //Relationships
            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);
            // User entity configuration
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasMaxLength(50);


        }
    }
}
