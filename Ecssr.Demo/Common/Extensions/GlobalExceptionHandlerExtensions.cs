using Ecssr.Demo.Application.Common.Exceptions;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common.ServiceResponse;
using Ecssr.Demo.Common.Utility;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Ecssr.Demo.Common.Extensions
{
    /// <summary>
    /// This is an extension class to handle exception globally. All the exception will hit this class 
    /// before sent in the reponse body. We parse the expection in the required format in this section.
    /// </summary>
    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            //Use ExceptionHandler Pipeline in used to handle
            _ = app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async context =>
                {
                    var _logger = context.RequestServices.GetService<ILogger<ExceptionHandlerOptions>>();
                    var _refId = context.RequestServices.GetService<IRefId>();
                    var _contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (_contextFeature != null)
                    {
                        //Copy  pointer to the original response body stream
                        var originalBodyStream = context.Response.Body;

                        // Set the Http Status Code
                        var statusCode = _contextFeature.Error switch
                        {
                            BadRequestException ex => HttpStatusCode.BadRequest,
                            NotFoundException ex => HttpStatusCode.NotFound,
                            UnprocessableEntityException ex => HttpStatusCode.UnprocessableEntity,
                            InternalServerErrorException ex => HttpStatusCode.InternalServerError
                        };

                        Error apiError = null;
                        IList<ValidationError> validationErrors = null;
                        string handlerStackTrace = null;
                        //parse validation error from exception message
                        try
                        {
                            validationErrors = _contextFeature.Error.InnerException?.Message.FromJsonString<IList<ValidationError>>();
                            handlerStackTrace = _contextFeature.Error.InnerException?.StackTrace;
                            if (validationErrors != null)
                                apiError = new Error(_contextFeature.Error.Message, statusCode, _refId?.Id, validationErrors);
                            else
                                apiError = new Error(_contextFeature.Error.Message, statusCode, _refId?.Id, null);
                        }
                        catch { }

                        // Set Response Details
                        context.Response.StatusCode = (int)statusCode;
                        context.Response.ContentType = "application/json";

                        //Return the Serialized Generic Error
                        await context.Response.WriteAsync(apiError.ToJsonString(true));
                    }
                }
            });

            return app;
        }
    }
}
