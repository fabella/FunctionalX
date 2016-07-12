namespace FunctionalX
{
    public static class Functional
    {
        public static Maybe<T> Just<T>(T value) => Maybe.Of(value);
        public static readonly NothingType Nothing = NothingType.Default;
    }
}
