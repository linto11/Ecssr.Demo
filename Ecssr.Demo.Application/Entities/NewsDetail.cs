namespace Ecssr.Demo.Application.Entities
{
    public class NewsDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string ImageUrl { get; set; }
        public IList<NewsDownload> NewsDownloads { get; set; }
    }
}
