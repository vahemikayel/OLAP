namespace OLAP.API.Models.Response.Error
{
    public class ApiException : ErrorResponseModel
    {
        public ApiException(int statusCode, string message = null, string details = null)
        {
            Details = details;
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public string Details { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Error, server side",
                _ => "Status code not implemented"
            };
        }
    }
}
