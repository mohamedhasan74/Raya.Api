using Raya.Api.Errors;

namespace Raya.Api.Dtos
{
    public class ApiFailResponse
    {
        public bool Success { get; set; }
        public ApiErrorResponse Error { get; set; }
    }
}
