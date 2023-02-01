namespace Ecssr.Demo.Application.Entities
{
    public class DownloadDetail
    {
        public IList<NewsDownload> NewsDownloads { get; set; }
        public string FileBase64 { get; set; }
    }
}
