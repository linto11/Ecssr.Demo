using Ecssr.Demo.Common;
using Ecssr.Demo.Infrastructure.Persistence.Context;
using Ecssr.Demo.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecssr.Demo.Infrastructure
{
    /// <summary>
    /// This class is part of the DI. This has been implemented seperately so that it is part of SOC. 
    /// Also it becomes to handle all Infrastructure related DIs in one place.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppSetting appSetting, string newsJson)
        {
            //connecting and using to MS SQL Server
            if (!appSetting.DatabaseSetting.IsInMemory)
                services.AddDbContext<NewsDbContext>(options =>
                options.UseSqlServer(appSetting.DatabaseSetting.ConnectionString,
                b => b.MigrationsAssembly(typeof(NewsDbContext).Assembly.FullName)), ServiceLifetime.Transient);
            else
                //using inMemory
                services.AddDbContext<NewsDbContext>(options => options.UseInMemoryDatabase("NewsDb"));
            services.AddTransient<INewsDbContext>(provider => provider.GetService<NewsDbContext>());

            //seed dummy data
            if (appSetting.DatabaseSetting.IsInMemory || 
                appSetting.DatabaseSetting.SeedData)
            {
                var serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetRequiredService<NewsDbContext>();
                DataGenerator.Initialize(serviceProvider, newsJson);
            }

            return services;
        }
    }
}