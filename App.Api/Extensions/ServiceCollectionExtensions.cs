using App.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.MSSqlServer;
using Serilog;
namespace App.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            //Log.Logger = new LoggerConfiguration()
            //.WriteTo
            //.MSSqlServer(
            //    connectionString: configuration.GetConnectionString("ApplicationConnection"),
            //    sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true })
            //.CreateLogger();


        }

    }
}
