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
        public static void InjectDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MoviesDbContext>(x =>
            x.UseSqlServer(connectionString)); 
        }
        public static void InjectRepositories(IServiceCollection services)
        {
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }
        public static void InjectServices(IServiceCollection services)
        {
            services.AddTransient<IMovieService, MovieServices>();
        }

    }
}
