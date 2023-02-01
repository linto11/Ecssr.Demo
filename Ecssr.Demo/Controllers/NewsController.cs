using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common.Constants;
using Ecssr.Demo.Common.ServiceResponse;
using Ecssr.Demo.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecssr.Demo.Controllers
{
    /// <summary>
    /// News controller for fetching and downloading the news details
    /// </summary>
    [Route(Routes.Base)]
    [ApiController]
    
    //Attribute to validate anti forgery token
    [ValidateAntiForgeryToken]
    public class NewsController : BaseController
    {
        /// <summary>
        /// This method is used to fetch the list of news based on page number
        /// </summary>
        /// <param name="totalRecords">total record to be displayed in response</param>
        /// <param name="pageNumber">the data of the required page number</param>
        /// <returns>Returns the list of news from the requested page number or else returns the different error responses based on the http status code</returns>
        [HttpGet] 
        [Route(Routes.News.FetchNews)]
        [ProducesResponseType(typeof(Success<NewsList>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> FetchNews(int totalRecords, int pageNumber)
        {
            var _response = await Mediator.Send(new Application.UseCases.News.FetchNews.Request
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber
            });

            return StatusCode((int)HttpStatusCode.OK, new Success<NewsList>(
                Application.Common.Constants.Message.News.FetchNews.Success, HttpStatusCode.OK, RefId, _response));
        }

        /// <summary>
        /// This methid is used to fetch the details of the specifc news
        /// </summary>
        /// <param name="id">The id of the news</param>
        /// <returns>Return the details of the news or else returns the different error responses based on the http status code</returns>
        [HttpGet]
        [Route(Routes.News.FetchNewsDetail)]
        [ProducesResponseType(typeof(Success<NewsDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> FetchNewsDetail(string id)
        {
            var _response = await Mediator.Send(new Application.UseCases.News.FetchNewsDetail.Request { Id = id });

            return StatusCode((int)HttpStatusCode.OK, new Success<NewsDetail>(
                Application.Common.Constants.Message.News.FetchNewsDetail.Success, HttpStatusCode.OK, RefId, _response));
        }

        /// <summary>
        /// This method is used to download the news detail based on the different formats
        /// </summary>
        /// <param name="request">The detail of the news that needs to be downloaded</param>
        /// <returns>Returns the donload details of the selected news along with the base64 of the file in the rerquired format or or else returns the different error responses based on the http status code</returns>
        [HttpPost]
        [Route(Routes.News.DownloadNewsDetail)]
        [ProducesResponseType(typeof(Success<DownloadDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> DownloadNewsDetail(Application.UseCases.News.DownloadNewsDetail.Request request)
        {
            var _response = await Mediator.Send(request);

            return StatusCode((int)HttpStatusCode.OK, new Success<DownloadDetail>(
                Application.Common.Constants.Message.News.FetchNewsDetail.Success, HttpStatusCode.OK, RefId, _response));
        }
    }
}
