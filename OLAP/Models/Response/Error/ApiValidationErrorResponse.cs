using System.Net;

namespace OLAP.API.Models.Response.Error
{
    public class ApiValidationErrorResponse : ErrorResponseModel
    {
        public ApiValidationErrorResponse()
        {
        }
        public IEnumerable<string> Errors { get; set; }
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string Message { get; set; }
    }
}
