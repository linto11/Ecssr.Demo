using System.ComponentModel.DataAnnotations;

namespace Ecssr.Demo.Infrastructure.Persistence.Models
{
    public class NewsDownload
    {
        [Key]
        public int NewsDownloadId { get; set; }
        public string? DownloadFormat { get; set; }
        public int? Count { get; set; }
        public string? NewsId { get;set; }
        public News? News { get; set; }
    }
}
