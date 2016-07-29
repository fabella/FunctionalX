using NUnit.Framework;
using FunctionalX.Monads;

namespace FunctionalX.Tests
{
    using static Functional;
    using static Maybe;
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase("42", ExpectedResult = 42)]
        [TestCase("not_an_int", ExpectedResult = null)]
        public int? TestParseInt(string s)
            => s.ParseInt().IsJust ? FromJust(s.ParseInt()) : (int?)null;

        [TestCase("42", 0, ExpectedResult = 42)]
        [TestCase("not_an_int", 42, ExpectedResult = 42)]
        public int TestParseInt(string s, int @default)
            => s.ParseInt(@default);

        [TestCase("42.0", ExpectedResult = 42.0)]
        [TestCase("not_a_double", ExpectedResult = null)]
        public double? TestParseDouble(string s)
            => s.ParseDouble().IsJust ? FromJust(s.ParseDouble()) : (double?)null;

        [TestCase("42.0", 0, ExpectedResult = 42.0)]
        [TestCase("not_a_double", 42.0, ExpectedResult = 42.0)]
        public double TestParseDouble(string s, double @default)
            => s.ParseDouble(@default);

        [TestCase("42.0", ExpectedResult = 42.0)]
        [TestCase("not_a_float", ExpectedResult = null)]
        public float? TestParseFloat(string s)
            => s.ParseFloat().IsJust ? FromJust(s.ParseFloat()) : (float?)null;

        [TestCase("42.0", 0, ExpectedResult = 42.0)]
        [TestCase("not_a_float", (float)42.0, ExpectedResult = 42.0)]
        public float TestParseFloat(string s, float @default)
            => s.ParseFloat(@default);

        [TestCase("42.0", ExpectedResult = 42.0)]
        [TestCase("not_a_decimal", ExpectedResult = null)]
        public decimal? TestParseDecimal(string s)
            => s.ParseDecimal().IsJust ? FromJust(s.ParseDecimal()) : (decimal?)null;

        [TestCase("42.0", 0, ExpectedResult = 42.0)]
        [TestCase("not_a_decimal", 42.0, ExpectedResult = 42.0)]
        public decimal TestParseDecimal(string s, decimal @default)
            => s.ParseDecimal(@default);

        [TestCase("not_a_date", ExpectedResult = false)]
        [TestCase("01/01/2016", ExpectedResult = true)]
        public bool TestParseDate(string s)
            => s.ParseDate().IsJust;
    }
}
