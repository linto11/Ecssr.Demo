using AutoMapper;
using Ecssr.Demo.Application.Common.Exceptions;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common;
using Ecssr.Demo.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecssr.Demo.Application.UseCases.News.FetchNewsDetail
{
    #region Request
    public class Request : IRequest<NewsDetail>
    {
        public string Id { get; set; }

    }
    #endregion

    #region Handler
    /// <summary>
    /// This class handles the functionality to fetch one specific news
    /// </summary>
    public class Handler : BaseUseCaseHandler, IRequestHandler<Request, NewsDetail>
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
        /// This method is the main function which handles the logic to fetch the specific news based on the id
        /// </summary>
        /// <param name="request">Paylod required for the function to process.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NewsDetail> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var dbNews = await _newsDbContext.News.Include(n => n.NewsDownloads).FirstOrDefaultAsync(n => n.Id == request.Id);
                if (dbNews != null)
                    return Mapper.Map<NewsDetail>(dbNews);
                else
                    NotFoundException.Throw(Common.Constants.Message.News.FetchNewsDetail.Failure.NotFound,
                        Common.Enums.ErrorNumber.NewsDeailNotFound);
            }
            catch (NotFoundException ex) { throw ex; }
            catch (UnprocessableEntityException ex) { throw ex; }
            catch (Exception ex)
            {
                InternalServerErrorException.Throw(RefId, Common.Constants.Message.News.FetchNewsDetail.Failure.InternalServerError, ex);
            }

            return default;
        }
    }
    #endregion
}

