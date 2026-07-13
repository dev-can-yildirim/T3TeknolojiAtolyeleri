namespace T3TeknolojiAtolyeleriApplication.Responses
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        public static ServiceResponse<T> Ok(T data, string? message = null)
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        public static ServiceResponse<T> Fail(string message)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = null
            };
        }

        public static ServiceResponse<T> Fail(string message, List<string> errors)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}