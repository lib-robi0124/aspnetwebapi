using DataAnnotations.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAnnotations.DataAccess
{
    public class DataAnnotationsDbContext : DbContext
    {
        public DataAnnotationsDbContext(DbContextOptions options) : base(options) { }
        //reprezant the database tables
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
