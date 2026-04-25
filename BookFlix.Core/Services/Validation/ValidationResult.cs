
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Services.Validation
{
    public record Error(string Key, ErrorType ErrorType)
    {
        public static readonly Error None = new(string.Empty, ErrorType.Failure);
        public static Error Failure(string key) => new(key, ErrorType.Failure);
        public static Error Validation(string key) => new(key, ErrorType.Validation);
        public static Error NotFound(string key) => new(key, ErrorType.NotFound);
        public static Error Conflict(string key) => new(key, ErrorType.Conflict);
        public static Error Unauthorized(string key) => new(key, ErrorType.Unauthorized);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("A successful result cannot have an error.");
            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("A failing result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<T> Success<T>(T value) => new(value, true, Error.None);
        public static Result<T> Failure<T>(Error error) => new(default, false, error);
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        protected internal Result(T value, bool isSuccess, Error error): base(isSuccess, error)
        {
            _value = value;
        }

        public T Value => IsSuccess? _value : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    }
}


