using System.Diagnostics.CodeAnalysis;

namespace KickTippHistory.Core
{
    public class Result<T>
    {
        private Result()
        {
        }

        public static Result<T> CreateValid(T value)
        {
            return new Result<T>
            {
                Valid = true,
                Value = value
            };
        }

        public static Result<T> CreateError(string errorText)
        {
            return new Result<T>
            {
                Valid = false,
                ErrorMessage = errorText
            };
        }

        public T? Value { get; init; }

        [MemberNotNullWhen(true, nameof(Value))]
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        public bool Valid { get; init; }

        public string? ErrorMessage { get; init; }
    }
}
