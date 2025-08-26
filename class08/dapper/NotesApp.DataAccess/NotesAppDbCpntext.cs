using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess
{
    public class NotesAppDbCpntext : DbContext
    {
        public NotesAppDbCpntext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region NoteConfig
            // Note
            //Constaints za text propertito
            modelBuilder.Entity<Note>()
                .Property(e => e.Text)
                .HasMaxLength(100)
                .IsRequired(); // not null

            modelBuilder.Entity<Note>()
                .Property(x => x.Priority)
                .IsRequired();

            modelBuilder.Entity<Note>()
                .Property(x => x.Tag)
                .IsRequired();

            //Relation
            modelBuilder.Entity<Note>()
                .HasOne(x => x.User)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.UserId);

            #endregion

            #region UserConfig

            //USER
            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .HasMaxLength(30)
                .IsRequired();

            //modelBuilder.Entity<User>()
            //    .Ignore(x => x.Age);

            #endregion

            #region SeedData
            // SEED if needed
            #endregion

        }
    }
 }
