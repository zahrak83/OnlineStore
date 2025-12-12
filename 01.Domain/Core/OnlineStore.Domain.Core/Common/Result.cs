namespace OnlineStore.Domain.Core.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static Result<T> Success(string message, T? data = default) =>
             new() { IsSuccess = true, Message = message, Data = data };

        public static Result<T> Failure(string message, T? data = default) =>
            new() { IsSuccess = false, Message = message, Data = data };
    }
}
