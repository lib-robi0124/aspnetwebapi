using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DataAnnotations.DataAccess;

namespace DataAnnotations.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDependencies(IServiceCollection services)
        {
            // Register DbContext with SQL Server
            services.AddDbContext<DataAnnotationsDbContext>(x =>
             x.UseSqlServer("Server=.;Database=DataAnnotationsDb;Trusted_Connection=True;TrustServerCertificate=true"));
           

        }
    }
}
