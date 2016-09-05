using System;
using System.Collections.Generic;

namespace FunctionalX.Monads
{
    public class Result
    {
        public bool Success { get; }
        public bool Failure => !Success;
        public string Error { get; }

        internal Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static implicit operator Result(bool success) => new Result(success, string.Empty);
        public static implicit operator Result(string error) => new Result(false, error);
        public static implicit operator Result(Unit unit) => new Result(true, string.Empty);

        public override string ToString()
            => Success
            ? "Success"
            : $"Error: {Error}";

        public override bool Equals(object obj)
            => Equals(obj as Result);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Success.GetHashCode()*397) ^ (Error?.GetHashCode() ?? 0);
            }
        }

        private bool Equals(Result other)
            => other != null && Success == other.Success
            && Error == other.Error;

        public TR Match<TR>(Func<string, TR> error, Func<TR> success)
            => Success
                ? success()
                : error(Error);

        public Unit Match(Action<string> error, Action success)
            => Match(error.ToFunc(), success.ToFunc());
    }

    public class Result<T>
    {
        public T Value { get; }
        public bool Success { get; }
        public bool Failure => !Success;
        public string Error { get; }

        internal Result(T value, bool success, string error)
        {
            Value = value;
            Success = success;
            Error = error;
        }

        public static implicit operator Result<T>(T value) => new Result<T>(value, true, string.Empty);
        public static implicit operator Result<T>(string error) => new Result<T>(default(T), false, error);

        public override string ToString()
            => Success
            ? $"Success: {Value}"
            : $"Error: {Error}";

        public override bool Equals(object obj)
            => Equals(obj as Result<T>);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(Value);
                hashCode = (hashCode*397) ^ Success.GetHashCode();
                hashCode = (hashCode*397) ^ (Error?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        private bool Equals(Result<T> other)
            => other != null && Success == other.Success
            && Error == other.Error && Value.Equals(other.Value);

        public TR Match<TR>(Func<string, TR> error, Func<T, TR> success)
            => Success
            ? success(Value)
            : error(Error);

        public Unit Match(Action<string> error, Action<T> success)
            => Match(error.ToFunc(), success.ToFunc());

        public IEnumerable<T> AsEnumerable()
        {
            if (Success)
                yield return Value;
        }
    }

    public static class ResultExtensions
    {
        public static Result<T> Of<T>(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Fail<T>(string error) => new Result<T>(default(T), false, error);
        public static Result Ok() => true;
        public static Result Fail(string error) => error;

        public static Result<TR> Map<T, TR>(this Result<T> @this, Func<T, TR> func)
            => @this.Success
            ? func(@this.Value)
            : new Result<TR>(default(TR), false, @this.Error);

        public static Result Map(this Result @this, Action func)
            => @this.Success
                ? func.ToFunc()()
                : @this;

        public static Result<TR> Bind<T, TR>(this Result<T> @this, Func<T, Result<TR>> func)
            => @this.Success
            ? func(@this.Value)
            : new Result<TR>(default(TR), false, @this.Error);

        public static Result Bind(this Result @this, Func<Result> func)
            => @this.Success
                ? func()
                : @this;

        public static Result<Unit> ForEach<T>(this Result<T> @this, Action<T> act)
            => @this.Map(act.ToFunc());

        public static Result<TR> Select<T, TR>(this Result<T> @this, Func<T, TR> func)
            => @this.Map(func);

        public static Result<TR> SelectMany<T, TR>(this Result<T> @this, Func<T, Result<TR>> func)
            => @this.Bind(func);
    }
}
