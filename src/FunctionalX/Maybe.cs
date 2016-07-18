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
            if (IsJust)
                yield return Value;
        }

        // ReSharper disable InconsistentNaming
        // ReSharper disable once ParameterHidesMember
        public TR Match<TR>(Func<TR> Nothing, Func<T, TR> Just)
            => IsJust ? Just(Value) : Nothing();
        // ReSharper restore InconsistentNaming

        public override bool Equals(object obj)
            => ReferenceEquals(null, obj) == false && (obj is Maybe<T> && Equals((Maybe<T>)obj));

        public bool Equals(Maybe<T> other)
            => IsJust == other.IsJust
               && (IsNothing || Value.Equals(other.Value));

        public bool Equals(NothingType other) => IsNothing;

        public static bool operator ==(Maybe<T> @this, Maybe<T> other) => @this.Equals(other);
        public static bool operator !=(Maybe<T> @this, Maybe<T> other) => !(@this == other);

        public override string ToString() => IsJust ? $"Just {Value}" : "nothingFunc";

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Value) * 397) ^ Value.GetHashCode();
            }
        }
    }

    public static class Maybe
    {
        public static Maybe<T> Of<T>(T value)
            => new Maybe<T>(value, value != null);

        public static Maybe<TR> Apply<T, TR>(this Maybe<Func<T, TR>> opt, Maybe<T> arg)
            => opt.IsJust && arg.IsJust
                ? Just(opt.Value(arg.Value))
                : Nothing;

        public static Maybe<TR> Map<T, TR>(this Maybe<T> val, Func<T, TR> f)
            => val.IsJust
                ? Just(f(val.Value))
                : Nothing;

        public static Maybe<TR> Bind<T, TR>(this Maybe<T> val, Func<T, Maybe<TR>> func)
            => val.IsJust
                ? func(val.Value)
                : Nothing;

        public static Maybe<TR> Select<T, TR>(this Maybe<T> val, Func<T, TR> f)
            => val.Map(f);

        public static Maybe<Unit> ForEach<T>(this Maybe<T> val, Action<T> action)
            => val.Map(action.ToFunc());

        public static Maybe<T> Where<T>(this Maybe<T> value, Func<T, bool> predicate)
            => value.IsJust && predicate(value.Value) ? value : Nothing;
    }
}
