using Ecssr.Demo.Common;
using Ecssr.Demo.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ecssr.Demo.Infrastructure.Persistence.Context
{
    /// <summary>
    /// DB context class
    /// </summary>
    public class NewsDbContext : DbContext, INewsDbContext
    {        
        public NewsDbContext(DbContextOptions<NewsDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<News> News { get; set; }
        public DbSet<NewsDownload> NewsDownloads { get; set; }

        public new DatabaseFacade Database => base.Database;

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                .HasMany(n => n.NewsDownloads)
                .WithOne(nd => nd.News)
                .HasForeignKey(nd => nd.NewsId);
 
        }
    }
}
