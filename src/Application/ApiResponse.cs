namespace Application
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public Status Status { get; set; }
        public string ResultMessage { get; set; }
        public string ErrorCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public ApiResponse(T data, Status status, string resultMessage = null, string errorCode = null, List<string> errors = null)
        {
            Data = data;
            Status = status;
            ResultMessage = resultMessage;
            ErrorCode = errorCode;
            Errors = errors;
        }
    }
    public enum Status
    {
        Failed = 0,
        Success = 1
    }
}
