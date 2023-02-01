using AutoMapper;
using Ecssr.Demo.Application.Common.Pagination;
using Ecssr.Demo.Application.Entities;

namespace Ecssr.Demo.Application.Common.Mapper
{
    /// <summary>
    /// Inherited from AutoMapper base class to map DB objects to Client objects
    /// </summary>
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {
            CreateMap<News, Infrastructure.Persistence.Models.News>().ReverseMap();
            CreateMap<NewsDetail, Infrastructure.Persistence.Models.News>().ReverseMap();
            CreateMap<NewsDownload, Infrastructure.Persistence.Models.NewsDownload>().ReverseMap();
            CreateMap<Entities.Pagination, PagedResultBase>().ReverseMap();
        }
    }
}
