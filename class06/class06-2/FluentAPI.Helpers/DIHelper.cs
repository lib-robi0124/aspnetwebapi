using FluentAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAPI.Helpers
{
    public static class DIHelper
    {
        public static void InjectDependencies(IServiceCollection services)
        {
            // Register DbContext with SQL Server
            services.AddDbContext<FluentApiDbContext>(x =>
             x.UseSqlServer("Server=.;Database=FluentAPIDb;Trusted_Connection=True;TrustServerCertificate=true"));


        }
    }
}
