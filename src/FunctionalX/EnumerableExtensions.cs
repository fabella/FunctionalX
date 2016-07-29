using System;
using System.Linq;
using System.Collections.Generic;
using FunctionalX.Monads;

namespace FunctionalX
{
    using static Functional;
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Alternative name to Linq Select operator
        /// </summary>
        /// <param name="this"></param>
        /// <param name="func"> Function to apply to each element in the list</param>
        /// <returns>New list with the result of "func" applied to all the elements</returns>
        public static IEnumerable<TR> Map<T,TR>(this IEnumerable<T> @this, Func<T,TR> func)
            => @this.Select(func);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this">List to be processed</param>
        /// <param name="action"> Function to apply to each element in the list</param>
        /// <returns>New list with the result of "func" applied to all the elements</returns>
        public static Unit ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach(var t in @this)
                action(t);
            return Unit();
        }

        /// <summary>
        /// Alternative name to Linq SelectMany operator
        /// </summary>
        /// <param name="this">List to be processed</param>
        /// <param name="func"> Function to apply to each element in the list</param>
        /// <returns>New list with the result of "func" applied to all the elements</returns>
        public static IEnumerable<TR> Bind<T,TR>(this IEnumerable<T> @this
                , Func<T,IEnumerable<TR>> func)
                => @this.SelectMany(func);

        /// <summary>
        /// Alternative name to Linq SelectMany operator but with a func that returns a Maybe monad
        /// So we flatten out the result
        /// </summary>
        /// <param name="@this">List to be processed</param>
        /// <param name="func"> Function to apply to each element in the list</param>
        /// <returns>New list with the result of "func" applied to all the elements</returns>
        public static IEnumerable<TR> Bind<T,TR>(this IEnumerable<T> @this, Func<T, Maybe<TR>> func)
            => @this.Bind(t => func(t).AsEnumerable());

        /// <summary>
        /// Flatten out the result of applying a function to a monad that returns a list.
        /// </summary>
        /// <param name="maybe">Maybe monad to apply the function to</param>
        /// <param name="func">Function to apply to the monad</param>
        /// <returns>New list with the result of "func" applied to the maybe monad</returns>
        public static IEnumerable<TR> Bind<T,TR>(this Maybe<T> maybe, Func<T, IEnumerable<TR>> func)
            => maybe.AsEnumerable().Bind(func);
    }
}
