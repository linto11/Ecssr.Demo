using Ecssr.Demo.Common.Utility;
using Ecssr.Demo.Infrastructure.Persistence.Context;
using Ecssr.Demo.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecssr.Demo.Infrastructure.Persistence.Seed
{
    /// <summary>
    /// This class is used to seed default data to Db
    /// </summary>
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider, string newsJson)
        {
            using (var context = new NewsDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<NewsDbContext>>()))
            {
                // Look for any board games already in database.
                if (context.News.Any())
                {
                    return;   // Database has been seeded
                }

                context.News.AddRange(newsJson.FromJsonString<IList<News>>());
                context.SaveChanges();
            }
        }
    }
}
