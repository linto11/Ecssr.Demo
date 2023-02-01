using Ecssr.Demo.Application.Entities;
using System.Net;

namespace Ecssr.Demo.Common.ServiceResponse
{
    /// <summary>
    /// Entity of error response
    /// </summary>
    public class Error
    {
        public Error(string _message, HttpStatusCode _httpStatusCode, string _refId, IList<ValidationError> _validationErrors)
        {
            Message = _message;
            HttpStatusCode = (int)_httpStatusCode;
            RefId = _refId;
            ValidationErrors = _validationErrors;
        }

        public string Message { get; private set; }
        public int HttpStatusCode { get; private set; }
        public string RefId { get; private set; }
        public IList<ValidationError> ValidationErrors { get; private set; }
    }
}
