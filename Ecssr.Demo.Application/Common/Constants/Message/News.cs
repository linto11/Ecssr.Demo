namespace Ecssr.Demo.Application.Common.Constants.Message
{
    /// <summary>
    /// Constant class to maintain error messages for each use case
    /// </summary>
    public struct News
    {
        public struct FetchNews
        {
            public struct Failure
            {
                public const string NotFound = "Unable to fetch the list of news.";
                public const string InternalServerError = "Failed to fetch the list of news due to unkown exception.";
            }

            public const string Success = "Retrieved all the news successfully.";
        }

        public struct FetchNewsDetail
        {
            public struct Failure
            {
                public const string NotFound = "Unable to fetch the selected new detail.";
                public const string InternalServerError = "Failed to fetch the detail due to unkown exception.";
                public const string UnprocessableEntity = "Fetching failed due to incompaitable parameters.";
            }

            public const string Success = "Retrieved the news detail successfully.";
        }

        public struct DownloadNewsDetail
        {
            public struct Failure
            {
                public const string NotFound = "Unable to download the selected new detail.";
                public const string InternalServerError = "Failed to download the detail due to unkown exception.";
                public const string UnprocessableEntity = "Downloading failed due to incompaitable parameters.";
            }

            public const string Success = "Downloaded the news detail successfully.";
        }
    }
}
