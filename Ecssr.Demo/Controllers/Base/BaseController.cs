using Ecssr.Demo.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecssr.Demo.Controllers.Base
{
    /// <summary>
    /// Base controller of all child controller
    /// </summary>
    public class BaseController : ControllerBase
    {
        #region get Mediatr using DI
        private IMediator _mediator;
        /// <summary>
        /// Mediatr object for handling CQRS design pattern
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        #endregion

        #region get unique RefId for each request and response
        public string RefId => HttpContext.RequestServices.GetService<IRefId>().Id;
        #endregion
    }
}
