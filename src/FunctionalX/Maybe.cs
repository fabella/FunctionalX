using System;
using System.Collections.Generic;

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

        public IEnumerable<T> AsEnumerable()
        {
            if(IsJust)
                yield return Value;
        }

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

        public static Maybe<R> Map<T,R>(this Maybe<T> val, Func<T,R> f)
            => val.IsJust
                ? Just(f(val.Value))
                : Nothing;

        public static Maybe<R> Bind<T,R>(this Maybe<T> val, Func<T, Maybe<R>> func)
            => val.IsJust
                ? func(val.Value)
                : Nothing;

        public static Maybe<R> Select<T,R>(this Maybe<T> val, Func<T,R> f)
            => val.Map(f);

        public static Maybe<Unit> ForEach<T>(this Maybe<T> val, Action<T> action)
            => val.Map(action.ToFunc());

        public static Maybe<T> Where<T>(this Maybe<T> value, Func<T, bool> predicate)
            => value.IsJust && predicate(value.Value) ? value : Nothing;
    }
}
