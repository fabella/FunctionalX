using NUnit.Framework;

namespace FunctionalX.Tests
{
    using static Functional;

    [TestFixture]
    public class MaybeTests
    {
        [TestCase("hello", ExpectedResult = true)]
        [TestCase(null, ExpectedResult = false)]
        public bool WhenNotNull_MaybeIsJust(string s) => Just(s).IsJust;

        [TestCase(ExpectedResult = true)]
        public bool NothingField_IsNothing() => Maybe<string>.Nothing.IsNothing;

        [TestCase("Fidel", ExpectedResult= "hi, Fidel")]
        [TestCase(null, ExpectedResult= "sorry")]
        public string MatchCallsCorrectFunction(string name)
            => Just(name).Match(
                Just: n => $"hi, {n}",
                Nothing: () => "sorry");

        [TestCase("marias", "maria", ExpectedResult = false)]
        [TestCase("marias", "marias", ExpectedResult = true)]
        public bool CompareMaybesForEquality(string left, string right)
            => Just(left).Equals(Just(right));

        [TestCase("marias", "maria", ExpectedResult = false)]
        [TestCase("marias", "marias", ExpectedResult = true)]
        public bool EqualOperatorTest(string left, string right)
            => Just(left) == Just(right);
    }
}
