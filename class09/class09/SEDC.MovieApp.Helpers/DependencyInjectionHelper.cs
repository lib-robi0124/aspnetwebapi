using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SEDC.MovieApp.DataAccess;
using SEDC.MovieApp.DataAccess.Implementation;
using SEDC.MovieApp.Services.Implementations;
using SEDC.MovieApp.Services.Interfaces;

namespace SEDC.MovieApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<MoviesDbContext>(x => x.UseSqlServer("Server=.;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True"));
        }
        public static void InjectRepositories(IServiceCollection services)
        {
            services.AddTransient<IMovieRepository, MovieRepository>();
        }
        public static void InjectServices(IServiceCollection services)
        {
            services.AddTransient<IMovieService, MovieServices>();
        }

    }
}
