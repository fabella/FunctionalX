using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace FunctionalX.Tests
{
    using static Functional;

    [TestFixture]
    public class FunctionalTests
    {
        [TestCase("hello", ExpectedResult = "hello")]
        public string FromJustOnJustReturnsValue(string maybe)
            => FromJust(Just(maybe));

        [TestCase(null, ExpectedResult = true)]
        public bool FromJustOnNothingThrowsInvalidOperationException(string val)
        {
            try
            {
                FromJust(Just<string>(val));
            }
            catch (InvalidOperationException)
            {
                return true;
            }
            return false;
        }

        [TestCase("x", "y", ExpectedResult = "x")]
        [TestCase(null, "y", ExpectedResult = "y")]
        [TestCase(null, null, ExpectedResult = null)]
        public string FromMaybeTest(string s, string val)
            => FromMaybe(Just(s), val);

        [TestCase("x", "y", ExpectedResult = "x")]
        [TestCase(null, "y", ExpectedResult = "y")]
        [TestCase(null, null, ExpectedResult = null)]
        public string FromMaybeFuncTest(string s, string val)
            => FromMaybe(Just(s), () => val);

        [Test]
        public void ListToMaybeOnEmptyList()
        {
            var empty = new List<int>();

            var sut = empty.ListToMaybe();
            Assert.IsTrue(Nothing == sut);
        }
        [Test]
        public void ListToMaybeOnNonEmptyList()
        {
            var empty = new List<int>() { 1, 2, 3};

            var sut = empty.ListToMaybe();
            Assert.AreEqual(Just(1), sut);
        }

        [Test]
        public void MaybeToListOnNothing()
        {
            var x = Just<string>(null);
            var sut = MaybeToList(x);

            Assert.IsTrue(!sut.Any());
        }

        [Test]
        public void MaybeToListOnJust()
        {
            var x = Just<string>("hi");
            var sut = MaybeToList(x);

            Assert.IsTrue(sut.Any());
        }

        [Test]
        public void CatMaybesTest()
        {
            var xs = new List<Maybe<string>>() {"x", "y", "z", null, null};
            var sut = xs.CatMaybes();
            Assert.AreEqual(3, sut.Count());
        }

        [Test]
        public void CatMaybesTestOnNothings()
        {
            var xs = new List<Maybe<string>>() {null, null, null};
            var sut = xs.CatMaybes();
            Assert.IsFalse(sut.Any());
        }

        [Test]
        public void MapMaybesTest()
        {
            var xs = new List<string>() {"x", "y", "z", null, null};
            var sut = xs.MapMaybe(x => x == null ? Nothing : Just(x.ToUpper()));
            Assert.AreEqual(3, sut.Count());
            Assert.AreEqual(new List<string>() { "X", "Y", "Z"}, sut);
        }
    }
}
