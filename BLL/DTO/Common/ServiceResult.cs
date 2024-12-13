namespace BLL.DTO.Common
{
    public class ServiceResult
    {
        public ServiceResult(ServiceResultStatus status)
        {
            Status = status;
        }
        public ServiceResult(ServiceResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public ServiceResultStatus Status { get; }

        public string Message { get; }

    }
    public class ServiceResult<TData> : ServiceResult
    {
        public ServiceResult(ServiceResultStatus status, string message) : base(status, message)
        {
        }
        public TData? Data { get; }
        public static implicit operator ServiceResult<TData>(string errorMessage) => new(ServiceResultStatus.Error, errorMessage);
        public static implicit operator ServiceResult<TData>((ServiceResultStatus Status, string Message) serviceResult) => new(serviceResult.Status, serviceResult.Message);
    }
    public enum ServiceResultStatus
    {
        Error,
        Success,
        NotFound,
        Forbidden,
    }
}
