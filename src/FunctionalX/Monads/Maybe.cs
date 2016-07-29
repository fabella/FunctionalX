using System;
using System.Collections.Generic;

namespace FunctionalX.Monads
{
    public struct Maybe<T>
    {
        // Singleton value for None
        public static readonly Maybe<T> Nothing = new Maybe<T>();

        internal T Value { get; }
        public bool IsJust { get; }
        public bool IsNothing => !IsJust;

        internal Maybe(T value, bool isJust)
        {
            Value = value;
            IsJust = isJust;
        }

        public static implicit operator Maybe<T>(T value) => Maybe.Of(value);
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

        public override string ToString() => IsJust ? $"Just {Value}" : "Nothing";

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
        /// <summary>
        /// Return functions that lift the value into a Maybe wrapper
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<T> Of<T>(T value)
            => new Maybe<T>(value, value != null);

        /// <summary>
        /// Gets the inner value if not null otherwise returns default
        /// value passed in.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static T GetOrElse<T>(this Maybe<T> @this, T @default)
            => @this.IsJust
                ? @this.Value
                : @default;

        /// <summary>
        /// Gets the inner value if not null or returns result of the fallback
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static T GetOrElse<T>(this Maybe<T> @this, Func<T> fallback)
            => @this.IsJust
                ? @this.Value
                : fallback();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opt"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static Maybe<TR> Apply<T, TR>(this Maybe<Func<T, TR>> opt, Maybe<T> arg)
            => opt.IsJust && arg.IsJust
                ? Of(opt.Value(arg.Value))
                : NothingType.Default;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Maybe<TR> Map<T, TR>(this Maybe<T> val, Func<T, TR> func)
            => val.IsJust
                ? Of(func(val.Value))
                : NothingType.Default;

        /// <summary>
        /// Takes an elevated value and a world-crossing function f;
        /// it "extracts" from m the inner value t and applies f to it.
        /// </summary>
        /// <param name="val">Value to be projected</param>
        /// <param name="func">Function to project the value</param>
        /// <returns></returns>
        public static Maybe<TR> Bind<T, TR>(this Maybe<T> val, Func<T, Maybe<TR>> func)
            => val.IsJust
                ? func(val.Value)
                : NothingType.Default;

        /// <summary>
        /// Projects the value to a new value returned by the function f
        /// </summary>
        /// <param name="val">Value to be projected</param>
        /// <param name="func">Function that projects to new value</param>
        /// <returns></returns>
        public static Maybe<TR> Select<T, TR>(this Maybe<T> val, Func<T, TR> func)
            => val.Map(func);

        /// <summary>
        /// Applys the action to the wrapped value if there is one. Otherwise it
        /// does nothing
        /// </summary>
        /// <param name="val">Value to apply action to</param>
        /// <param name="action">Action to be applied</param>
        /// <returns></returns>
        public static Maybe<Unit> ForEach<T>(this Maybe<T> val, Action<T> action)
            => val.Map(action.ToFunc());

        /// <summary>
        /// Applies the predicate function the the wrapped value if there is one.
        /// Return the value if the predicate is true, otherwise returns Nothing.
        /// </summary>
        /// <param name="value">Value to apply the predicate to</param>
        /// <param name="predicate">Predicate function</param>
        /// <returns></returns>
        public static Maybe<T> Where<T>(this Maybe<T> value, Predicate<T> predicate)
            => value.IsJust && predicate(value.Value) ? value : NothingType.Default;
    }
}
