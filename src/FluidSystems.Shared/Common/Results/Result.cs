namespace FluidSystems.Shared.Common.Results
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }

        private Result(T value, bool isSuccess, string errorMessage, Exception ex)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null, null);
        public static Result<T> Failure(string errorMessage, Exception ex = null) => new Result<T>(default, false, errorMessage, ex);
    }
}