using App.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
namespace App.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the necessary infrastructure for the application context.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }
    }
}
