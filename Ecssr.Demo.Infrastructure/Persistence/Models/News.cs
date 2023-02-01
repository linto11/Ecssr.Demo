using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecssr.Demo.Infrastructure.Persistence.Models
{
    public class News
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Brief { get; set; }
        public string? ImageUrl { get; set; }
        public string? Detail { get; set; }
        public List<NewsDownload>? NewsDownloads { get; set; }
    }
}
