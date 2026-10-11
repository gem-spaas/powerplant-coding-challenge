namespace SharedKernel
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result NotFound(string description) => new(false, new Error("NotFound", description));
    }

    public sealed class Result<T> : Result
    {
        private Result(bool isSuccess, T? value, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public T? Value { get; }

        public static Result<T> Success(T value) =>
            new(true, value, Error.None);

        public new static Result<T> Failure(Error error) =>
            new(false, default, error);

        public new static Result<T> NotFound(string description) =>
            new(false, default, new Error("NotFound", description));
    }

}
