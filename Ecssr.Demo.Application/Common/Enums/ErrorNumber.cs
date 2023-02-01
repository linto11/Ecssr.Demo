using System.ComponentModel;

namespace Ecssr.Demo.Application.Common.Enums
{
    public enum ErrorNumber
    {
        [Description("News detail not found")]
        NewsDeailNotFound = 5,

        [Description("News list not found")]
        NewsListNotFound = 4,

        [Description("News Id is invalid.")]
        InvalidNewsId = 3,

        [Description("News Id is required.")]
        NewsIdIsRequired = 2,

        [Description("Process failed due to technical error. Please try again after sometime. [{0}]")]
        GenericException = 1,
        
        [Description("Success")]
        Success = 0
    }
}
