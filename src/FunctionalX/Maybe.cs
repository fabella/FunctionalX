using System;

namespace FunctionalX
{
    using static Functional;
    public struct Maybe<T>
    {
        public static readonly Maybe<T> Nothing = new Maybe<T>(); 

        internal T Value { get; }
        public bool IsJust { get; }
        public bool IsNothing => !IsJust;

        internal Maybe(T value, bool isJust)
        {
            Value = value;
            IsJust = isJust;
        }

        public static implicit operator Maybe<T>(T value) => Just(value);
        public static implicit operator Maybe<T>(NothingType _) => Nothing;

        public R Match<R>(Func<R> Nothing, Func<T, R> Just)
            => IsJust ? Just(Value) : Nothing();

        public bool Equals(Maybe<T> other)
            => IsJust == other.IsJust
               && (IsNothing || Value.Equals(other.Value));

        public bool Equals(NothingType other) => IsNothing;

        public static bool operator ==(Maybe<T> @this, Maybe<T> other) => @this.Equals(other);
        public static bool operator !=(Maybe<T> @this, Maybe<T> other) => !(@this == other);

        public override string ToString() => IsJust ? $"Just {Value}" : "Nothing";
    }

    public static class Maybe
    {
        public static Maybe<T> Of<T>(T value)
            => new Maybe<T>(value, value != null);

        public static Maybe<R> Apply<T, R>(this Maybe<Func<T, R>> opt, Maybe<T> arg)
            => opt.IsJust && arg.IsJust
                ? Just(opt.Value(arg.Value))
                : Nothing;
    }
}
