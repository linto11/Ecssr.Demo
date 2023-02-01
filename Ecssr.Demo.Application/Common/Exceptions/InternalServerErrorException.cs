using Ecssr.Demo.Application.Common.Enums;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common.Utility;
using System.Runtime.Serialization;

namespace Ecssr.Demo.Application.Common.Exceptions
{
    /// <summary>
    /// Generic class to handle internal server error response that is inherited from Exception class
    /// </summary>
    public class InternalServerErrorException: Exception
    {
        private readonly string _stackTrace;
        public override string? StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }

        public InternalServerErrorException(string message, string stackTrace, Exception innerException): base(message, innerException)
        {
            _stackTrace = stackTrace;
        }

        protected InternalServerErrorException(SerializationInfo info, StreamingContext context): base(info, context) { }

        public static void Throw(string refId, string message, Exception ex)
        {
            var validationErrors = new List<ValidationError>()
            {
                new ValidationError
                {
                    ErrorMessage = string.Format(ErrorNumber.GenericException.GetDescription(), refId),
                    ErrorNumber = ErrorNumber.GenericException
                }
            };
            throw new InternalServerErrorException(message, ex.ParseException(), new Exception(validationErrors.ToJsonString()));
        }
    }
}
