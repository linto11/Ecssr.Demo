using AutoMapper;
using Ecssr.Demo.Application.Common.Exceptions;
using Ecssr.Demo.Application.Common.Pagination;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common;
using Ecssr.Demo.Infrastructure.Persistence.Context;
using MediatR;

namespace Ecssr.Demo.Application.UseCases.News.FetchNews
{
    #region Request
    public class Request : IRequest<NewsList>
    {
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
    }
    #endregion

    #region Handler
    /// <summary>
    /// This class handles the functionality to fetch all the lis of news
    /// </summary>
    public class Handler : BaseUseCaseHandler, IRequestHandler<Request, NewsList>
    {
        private readonly INewsDbContext _newsDbContext;

        /// <summary>
        ///  Constructor to initlize default objects
        /// </summary>
        /// <param name="refId">Ref Id of the request</param>
        /// <param name="mapper">Mapper object to map from DB to client or vice versa</param>
        /// <param name="appSetting">App Setting object. To be used if required</param>
        /// <param name="newsDbContext">DB Context to access news table</param>
        public Handler(IRefId refId, IMapper mapper, AppSetting appSetting, INewsDbContext newsDbContext) : base(refId, mapper, appSetting)
        {
            _newsDbContext = newsDbContext;
        }

        /// <summary>
        /// This method is the main function which handles the logic to fetch all the news.
        /// </summary>
        /// <param name="request">Paylod required for the function to process.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NewsList> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                //get the pages list of news as required
                var pagedResult = await _newsDbContext.News.FetchByPaginationAsync(request.PageNumber, request.TotalRecords);
                if (pagedResult != null && pagedResult.Results != null && pagedResult.Results.Count > 0)
                    return new NewsList()
                    {
                        News = Mapper.Map<IList<Entities.News>>(pagedResult.Results),
                        Pagination = Mapper.Map<Pagination>(pagedResult)
                    };
                else
                    NotFoundException.Throw(Common.Constants.Message.News.FetchNews.Failure.NotFound,
                        Common.Enums.ErrorNumber.NewsListNotFound);
            }
            catch (NotFoundException ex) { throw ex; }
            catch (Exception ex)
            {
                InternalServerErrorException.Throw(RefId, Common.Constants.Message.News.FetchNews.Failure.InternalServerError, ex);
            }

            return default;
        }
    }
    #endregion
}
