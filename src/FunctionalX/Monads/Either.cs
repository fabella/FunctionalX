using System;
using System.Collections.Generic;

namespace FunctionalX.Monads
{
    public struct Either<L, R>
    {
        internal L Left { get; }
        internal R Right { get; }

        public bool IsLeft { get; }
        public bool IsRight => !IsLeft;

        internal Either(L left)
        {
            IsLeft = true;
            Left = left;
            Right = default(R);
        }

        internal Either(R right)
        {
            IsLeft = false;
            Right = right;
            Left = default(L);
        } 

        public static implicit operator Either<L, R>(L left)  => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
            => IsLeft
                ? Left(this.Left)
                : Right(this.Right);

        public Unit Match(Action<L> Left, Action<R> Right)
            => Match(Left.ToFunc(), Right.ToFunc());

        public IEnumerator<R> AsEnumerable()
        {
            if (IsRight)
                yield return Right;
        }
    }

    public static class Either
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Either<L, R> Of<L, R>(L left)
            => new Either<L, R>(left); 

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Either<L, R> Of<L, R>(R right)
            => new Either<L, R>(right); 

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="RR"></typeparam>
        /// <param name="this"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Either<L, RR> Map<L, R, RR>(this Either<L, R> @this, Func<R, RR> func)
            => @this.IsRight
                ? func(@this.Right)
                : new Either<L, RR>(@this.Left);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="this"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static Either<L, Unit> ForEach<L, R>(this Either<L, R> @this, Action<R> act)
            => @this.Map(act.ToFunc());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="RR"></typeparam>
        /// <param name="this"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> @this, Func<R, Either<L, RR>> func)
            => @this.IsRight
                ? func(@this.Right)
                : new Either<L, RR>(@this.Left);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="RR"></typeparam>
        /// <param name="this"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Either<L, RR> Select<L, R, RR>(this Either<L, R> @this, Func<R, RR> func)
            => @this.Map(func);
    }
}
