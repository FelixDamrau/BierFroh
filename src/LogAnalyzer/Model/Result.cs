using System.Diagnostics.CodeAnalysis;

namespace LogAnalyzer.Model
{
    public class Result<T>
    {
        public T? Value { get; }

        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        [MemberNotNullWhen(true, nameof(Value))]
        public bool Valid { get; }

        public string? ErrorMessage { get; }

        public static Result<T> Create(T value)
        {
            return new Result<T>(value);
        }

        public Result(T value)
        {
            Value = value;
            Valid = true;
        }

        public Result(string errorMessage)
        {
            Value = default;
            Valid = false;
            ErrorMessage = errorMessage;
        }
    }
}
