namespace Ecssr.Demo.Common.Constants
{
    /// <summary>
    /// This class is used to set the path of routes in controller
    /// </summary>
    public class Routes
    {
        public const string Base = "api/v{version:apiVersion}/[controller]";
        public struct News
        {
            public const string FetchNews = "";
            public const string FetchNewsDetail = "{id}";
            public const string DownloadNewsDetail = "download";
        }
    }
}
