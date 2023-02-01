using AutoMapper;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common;

namespace Ecssr.Demo.Application.UseCases
{
    /// <summary>
    /// Base class to be accessed by all the handlers
    /// </summary>
    public abstract class BaseUseCaseHandler
    {
        public BaseUseCaseHandler(IRefId _refId, IMapper _mapper, AppSetting _appSetting)
        {
            RefId = _refId.Id;
            Mapper = _mapper;
            AppSetting = _appSetting;
        }

        public IList<ValidationError> ValidationErrors { get; set; }
        public IMapper Mapper { get; set; }
        public string RefId { get; set; }
        public AppSetting AppSetting { get; set; }
    }
}
