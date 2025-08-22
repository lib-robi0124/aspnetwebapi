using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.DataAccess;
using NotesApp.DataAccess.Implementations;
using NotesApp.Domain.Models;
using NotesApp.Services.Implementations;
using NotesApp.Services.Interfaces;

namespace NotesApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services)
        {             // Register the DbContext with the dependency injection container
            services.AddDbContext<NotesAppDbCpntext>(options =>
                options.UseSqlServer("Server=.;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True"));
            // Register repositories
            services.AddScoped<IRepository<Note>, NoteRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
        }
        public static void InjectRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository<Note>, NoteRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
        }
        public static void InjectServices(IServiceCollection services)
        {
            // Register services
            services.AddTransient<INoteService, NoteService>();
        }
    }
}
