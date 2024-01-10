using App.Infrastructure.DbContexts;
using App.Infrastructure.Repositories;
using App.Infrastructure.Shared;
using App.Application.Interfaces.Contexts;
using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Shared;
using Microsoft.Extensions.DependencyInjection;
namespace App.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the infrastructure layer to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICompareObject, CompareObjectService>();
        }
    }
}
