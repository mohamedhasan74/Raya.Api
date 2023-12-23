namespace Raya.Api.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int StatusCode, string Message = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? DefaultStatusCodeResponse(StatusCode);
        }

        private string? DefaultStatusCodeResponse(int StatusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request, You Have Made",
                401 => "An Authorized, You Are Not",
                404 => "Resourse Was Not Found",
                500 => "Error Are Very Bad",
                _ => null
            };
        }
    }
}
