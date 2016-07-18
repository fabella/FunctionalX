using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FunctionalX
{
    using static Functional;
    public static class EnumerableExtensions
    {
        public static IEnumerable<R> Map<T,R>(this IEnumerable<T> @this, Func<T,R> func)
            => @this.Select(func);

        public static Unit Foreach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach(var t in @this)
                action(t);
            return Unit();
        }

        public static IEnumerable<R> Bind<T,R>(this IEnumerable<T> source
                , Func<T,IEnumerable<R>> func)
        {
            foreach(T t in source)
                foreach(R r in func(t))
                    yield return r;
        }

        public static IEnumerable<T> List<T>(params T[] items)
            => items.ToImmutableList();

        public static IEnumerable<R> Bind<T,R>(this IEnumerable<T> list, Func<T, Maybe<R>> func)
            => list.Bind(t => func(t).AsEnumerable());

        public static IEnumerable<R> Bind<T,R>(this Maybe<T> maybe, Func<T, IEnumerable<R>> func)
            => maybe.AsEnumerable().Bind(func);
    }
}
