using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the application layer to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
