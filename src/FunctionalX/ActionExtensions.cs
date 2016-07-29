using System;

namespace FunctionalX
{
    using static Functional;
    public static class ActionExt
    {
        /// <summary>
        /// Converts a parameterless action to a func that returns Unit.
        /// </summary>
        /// <param name="action">Action delegate to be converted</param>
        /// <returns>Func<Unit></returns>
        public static Func<Unit> ToFunc(this Action action)
            => () => { action(); return Unit(); };

        /// <summary>
        /// Converts a one parameter action to a func that takes one parameter and returns Unit.
        /// </summary>
        /// <param name="action">Action delegate to be converted</param>
        /// <returns>Func<T, Unit></returns>
        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
            => t => { action(t); return Unit(); };

        /// <summary>
        /// Converts a two parameter action to a func that takes two parameter and returns Unit.
        /// </summary>
        /// <param name="action">Action delegate to be converted</param>
        /// <returns>Func<T1, T2, Unit></returns>
        public static Func<T1,T2,Unit> ToFunc<T1,T2>(this Action<T1,T2> action)
            => (t1, t2) => { action(t1, t2); return Unit(); };
    }
}
