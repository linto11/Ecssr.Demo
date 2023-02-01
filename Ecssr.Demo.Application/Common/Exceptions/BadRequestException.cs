using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common.Utility;
using System.Runtime.Serialization;

namespace Ecssr.Demo.Application.Common.Exceptions
{
    /// <summary>
    /// Generic class to handle bad request response that is inherited from Exception class
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void Throw(string message, IList<ValidationError> validationErrors)
        {
            throw new BadRequestException(message, new Exception(validationErrors.ToJsonString()));
        }
    }
}
