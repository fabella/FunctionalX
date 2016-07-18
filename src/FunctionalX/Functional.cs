using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FunctionalX
{
    public static class Functional
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Unit unit = new Unit();

        public static Unit Unit() => unit;
        /// <summary>
        /// This function allows you to create a Maybe struct
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="maybe">Maybe to be unwrapped</param>
        /// <param name="val">Default value in case Maybe is Nothing</param>
        /// <returns></returns>
        public static T FromMaybe<T>(Maybe<T> maybe, T val)
            => maybe.IsJust ? maybe.Value : val;

        /// <summary>
        /// Unwraps the value of a maybe. Returns default value if maybe is Nothing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maybe">Maybe to be unwrapped</param>
        /// <param name="fallBack">Callback in case the value is Nothing</param>
        /// <returns></returns>
        public static T FromMaybe<T>(Maybe<T> maybe, Func<T> fallBack)
            => maybe.IsJust ? maybe.Value : fallBack();

        /// <summary>
        /// Unwraps the value from a maybe type.
        /// Throws InvalidOperationExection if value is Nothing
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="xs"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static Maybe<T> ListToMaybe<T>(this IEnumerable<T> xs)
            => xs.Any() ? Just(xs.First()) : Nothing;

        /// <summary>
        /// This function returns an empty list when given Nothing
        /// or singleton list when not given Nothing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maybe"></param>
        /// <returns></returns>
        public static IEnumerable<T> MaybeToList<T>(Maybe<T> maybe)
            => maybe.IsJust ? new List<T>() { maybe.Value } : new List<T>();

        /// <summary>
        /// This function takes a list of Maybes and returns a list of all Just values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="xs">List of values</param>
        /// <param name="map">Map function to map value from T to R</param>
        /// <returns></returns>
        public static IEnumerable<TR> MapMaybe<T, TR>(this IEnumerable<T> xs, Func<T, Maybe<TR>> map)
            => xs.Select(map).Where(x => x.IsJust).Select(x => x.Value);

        /// <summary>
        /// Creates an immutable list with the given values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">items to be added to the list</param>
        /// <returns>Immutable list with the items passed in</returns>
        public static IEnumerable<T> List<T>(params T[] items)
            => items.ToImmutableList();
    }
}