namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, T value, string error)
        {
            if (isSuccess && value == null)
                throw new ArgumentNullException(nameof(value));
            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new ArgumentException("Error message is required", nameof(error));

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        public static Result<T> Failure(string error) => new Result<T>(false, default, error);
        public static Result<T> NotFound(string error) => new Result<T>(false, default, error);
        public static Result<T> Unauthorized(string error) => new Result<T>(false, default, error); 
    }

    // Versión no genérica para operaciones sin valor de retorno
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new ArgumentException("Error message is required", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
        public static Result NotFound(string error) => new Result(false, error);
        public static Result Unauthorized(string error) => new Result(false, error);
    }
}
