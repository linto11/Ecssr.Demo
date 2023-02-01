using Ecssr.Demo.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ecssr.Demo.Infrastructure.Persistence.Context
{
    /// <summary>
    /// Interface of the DB Context class
    /// </summary>
    public interface INewsDbContext
    {
        DbSet<News> News { get; set; }
        DbSet<NewsDownload> NewsDownloads { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
