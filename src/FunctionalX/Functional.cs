using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FunctionalX.Monads;

namespace FunctionalX
{
    public static class Functional
    {
        // singleton unit struct
        // ReSharper disable once InconsistentNaming
        private static readonly Unit unit = new Unit();

        public static Unit Unit() => unit;
        /// <summary>
        /// This function allows you to create a Maybe struct
        /// </summary>
        /// <param name="value">Value to be wrapped in a Maybe</param>
        /// <returns>Return maybe wrapper</returns>
        public static Maybe<T> Just<T>(T value) => Maybe.Of(value);

        /// <summary>
        /// Nothing maybe wrapper
        /// </summary>
        public static readonly NothingType Nothing = NothingType.Default;

        /// <summary>
        /// Unwraps the value of a maybe. Returns default value if maybe is Nothing
        /// </summary>
        /// <param name="maybe">Maybe to be unwrapped</param>
        /// <param name="val">Default value in case Maybe is Nothing</param>
        /// <returns></returns>
        public static T FromMaybe<T>(Maybe<T> maybe, T val)
            => maybe.GetOrElse(val);

        /// <summary>
        /// Unwraps the value of a maybe. Returns default() value if maybe is Nothing
        /// </summary>
        /// <param name="maybe">Maybe to be unwrapped</param>
        /// <param name="fallback">Callback in case the value is Nothing</param>
        /// <returns></returns>
        public static T FromMaybe<T>(Maybe<T> maybe, Func<T> fallback)
            => maybe.GetOrElse(fallback);

        /// <summary>
        /// Unwraps the value from a maybe type.
        /// Throws InvalidOperationExection if value is Nothing
        /// </summary>
        /// <param name="just">Value to be unwrapped</param>
        /// <returns></returns>
        public static T FromJust<T>(Maybe<T> just)
        {
            if (just.IsJust)
                return just.Value;
            throw new InvalidOperationException("Exception: Maybe.FromJust: Nothing");
        }

        /// <summary>
        /// This function returns Nothing if the list is empty or Just a where a is the first element in the list.
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        public static Maybe<T> ListToMaybe<T>(this IEnumerable<T> xs)
            => xs.Any() ? Just(xs.First()) : Nothing;

        /// <summary>
        /// This function returns an empty list when given Nothing
        /// or singleton list when not given Nothing
        /// </summary>
        /// <param name="maybe"></param>
        /// <returns>Returns an immutable list with either one value or empty</returns>
        public static IEnumerable<T> MaybeToList<T>(Maybe<T> maybe)
            => maybe.IsJust ? List(maybe.Value) : List<T>();

        /// <summary>
        /// This function takes a list of Maybes and returns a list of all Just values.
        /// </summary>
        /// <param name="xs">List of Maybes</param>
        /// <returns></returns>
        public static IEnumerable<T> CatMaybes<T>(this IEnumerable<Maybe<T>> xs)
            => xs.Where(x => x.IsJust).Select(x => x.Value);

        /// <summary>
        /// This function is a version of select which can throw out elements.
        /// In particular, the functional argument returns something of type Mabye R.
        /// If this is Nothing, no element is added to the list.
        /// If it is Just R, then the element is added to the list.
        /// </summary>
        /// <param name="xs">List of values</param>
        /// <param name="map">Map function to map value from T to R</param>
        /// <returns></returns>
        public static IEnumerable<TR> MapMaybe<T, TR>(this IEnumerable<T> xs, Func<T, Maybe<TR>> map)
            => xs.Select(map).Where(x => x.IsJust).Select(x => x.Value);

        /// <summary>
        /// Creates an empty immutable.
        /// </summary>
        /// <returns>ImmutableList</returns>
        public static IEnumerable<T> List<T>()
            => ImmutableList.Create<T>();

        /// <summary>
        /// Creates an immutable list with the given values
        /// </summary>
        /// <param name="items">items to be added to the list</param>
        /// <returns>Immutable list with the items passed in</returns>
        public static IEnumerable<T> List<T>(params T[] items)
            => items.ToImmutableList();

        #region Currying and Partial Function Application

        /// <summary>
        /// Partial function application for functions that take two parameters
        /// </summary>
        /// <param name="func">Function to be partially applied</param>
        /// <param name="t1">First argument to partially apply</param>
        /// <returns></returns>
        public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> func, T1 t1)
            => t2 => func(t1, t2);

        /// <summary>
        /// Partial function application for functions that take three parameters
        /// </summary>
        /// <param name="func">Function to be partially applied the first value</param>
        /// <param name="t1">First value to apply</param>
        /// <returns></returns>
        public static Func<T2, T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 t1)
            => (t2, t3) => func(t1, t2, t3);

        /// <summary>
        /// Partial function application for functions that take four parameters
        /// </summary>
        /// <param name="func">Function to be partially applied the first value</param>
        /// <param name="t1">First value to apply</param>
        /// <returns></returns>
        public static Func<T2, T3, T4, TR> Apply<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func, T1 t1)
            => (t2, t3, t4) => func(t1, t2, t3, t4);

        /// <summary>
        /// Curry for a function that has two parameters. Returns function that takes one value and returns another
        /// function that takes the second parameter and returns the result
        /// </summary>
        /// <param name="func">function to be curried</param>
        /// <returns></returns>
        public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> func)
            => t1 => t2 => func(t1, t2);

        /// <summary>
        /// Curry for a function that has three parameters.
        /// </summary>
        /// <param name="func">function to be curried</param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3, TR>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func)
            => t1 => t2 => t3 => func(t1, t2, t3);

        /// <summary>
        /// Curry for a function that has four parameters.
        /// </summary>
        /// <param name="func">function to be curried</param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> Curry<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func)
            => t1 => t2 => t3 => t4 => func(t1, t2, t3, t4);

        /// <summary>
        /// Swap arguments to a function.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T, T, TR> SwapArgs<T, TR>(this Func<T, T, TR> func)
            => (t1, t2) => func(t2, t1);
        #endregion

        #region Function Composition
        /// <summary>
        /// Composes two functions by applying f1 first and then applying the result of f1 to f2
        /// The types must match.
        /// </summary>
        /// <param name="f1">Function to be applied first</param>
        /// <param name="f2">Function to be applied second</param>
        /// <returns></returns>
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
            => t1 => f2(f1(t1));
        #endregion
    }
}