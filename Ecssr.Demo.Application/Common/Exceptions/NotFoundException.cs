using Ecssr.Demo.Application.Common.Enums;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common.Utility;
using System.Runtime.Serialization;

namespace Ecssr.Demo.Application.Common.Exceptions
{
    /// <summary>
    /// Generic class to handle not found error response that is inherited from Exception class
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, Exception innerException): base(message, innerException) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context): base(info, context) { }

        public static void Throw(string message, ErrorNumber errorNumber)
        {
            var validationErrors = new List<ValidationError>()
            {
                new ValidationError
                {
                    ErrorMessage = errorNumber.GetDescription(),
                    ErrorNumber = errorNumber
                }
            };
            throw new NotFoundException(message, new Exception(validationErrors.ToJsonString()));
        }
    }
}
