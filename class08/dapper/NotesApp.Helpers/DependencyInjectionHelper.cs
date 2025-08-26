using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.DataAccess;
using NotesApp.DataAccess.Adoimplementation;
using NotesApp.DataAccess.DapperImplentations;
using NotesApp.DataAccess.Implementations;
using NotesApp.Domain.Models;
using NotesApp.Services.Implementations;
using NotesApp.Services.Interfaces;

namespace NotesApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services, string connectionString)
        {             // Register the DbContext with the dependency injection container
            //services.AddDbContext<NotesAppDbCpntext>(options =>
            //    options.UseSqlServer("Server=.;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True"));
            //// Register repositories
            services.AddDbContext<NotesAppDbCpntext>(options =>
                options.UseSqlServer(connectionString));
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
        public static void InjectDapperRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRepository<Note>>(x => new NoteDapperRepository(connectionString));
        }
        public static void InjectAdoRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRepository<Note>>(x => new NoteAdoRepository(connectionString));
        }
    }
}
