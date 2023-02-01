using Ecssr.Demo.Application.Common.Enums;

namespace Ecssr.Demo.Application.Entities
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }
        public ErrorNumber ErrorNumber { get; set; }
    }
}
