using NUnit.Framework;
using System;
using System.Linq;

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

        [Test]
        public void ApplyTestsOnJust()
        {
            Func<string, int> func = (s) => int.Parse(s);
            var val = Just(func);

            var sut = val.Apply(Just("3"));
            Assert.AreEqual(Just(3), sut);
        }

        [Test]
        public void ApplyTestsOnNothing()
        {
            Func<string, int> func = (s) => int.Parse(s);
            var val = Just(func);

            var sut = val.Apply(Nothing);
            Assert.IsTrue(Nothing == sut);
        }

        [Test]
        public void MapTestsOnJust()
        {
            var x = Just(3);

            var sut = x.Map(y => y * 2);

            Assert.AreEqual(Just(6), sut);
        }

        [Test]
        public void AsEnumerableTestOnJust()
        {
            var x = Just(3);

            var sut = x.AsEnumerable();

            Assert.AreEqual(1, sut.Count());
            Assert.IsTrue(Just(3) == sut.First());
        }

        [Test]
        public void AsEnumerableTestOnNothing()
        {
            Maybe<int> x = Nothing;

            var sut = x.AsEnumerable();

            Assert.AreEqual(0, sut.Count());
        }
    }
}
