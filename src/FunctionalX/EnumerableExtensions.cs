using System;
using System.Linq;
using System.Collections.Generic;

namespace FunctionalX
{
    using static Functional;
    public static class EnumerableExtensions
    {
        public static IEnumerable<TR> Map<T,TR>(this IEnumerable<T> @this, Func<T,TR> func)
            => @this.Select(func);

        public static Unit Foreach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach(var t in @this)
                action(t);
            return Unit();
        }

        public static IEnumerable<TR> Bind<T,TR>(this IEnumerable<T> source
                , Func<T,IEnumerable<TR>> func)
                => source.SelectMany(func);

        public static IEnumerable<TR> Bind<T,TR>(this IEnumerable<T> list, Func<T, Maybe<TR>> func)
            => list.Bind(t => func(t).AsEnumerable());

        public static IEnumerable<TR> Bind<T,TR>(this Maybe<T> maybe, Func<T, IEnumerable<TR>> func)
            => maybe.AsEnumerable().Bind(func);
    }
}
