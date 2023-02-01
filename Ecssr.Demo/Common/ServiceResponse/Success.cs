using System.Net;

namespace Ecssr.Demo.Common.ServiceResponse
{
    /// <summary>
    /// Entity of success response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Success<T>
    {
        public Success(string _message, HttpStatusCode _httpStatusCode, string _refId, T _data)
        {
            Message = _message;
            HttpStatusCode = (int)_httpStatusCode;
            RefId = _refId;
            ResponseData = _data;
        }

        public string Message { get; private set; }
        public int HttpStatusCode { get; private set; }
        public string RefId { get; private set; }
        public T ResponseData { get; private set; }
    }
}
