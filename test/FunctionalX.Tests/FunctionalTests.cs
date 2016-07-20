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

        [Test]
        public void ListTest()
        {
            var sut = List(1, 2, 3);
            Assert.IsTrue(3 == sut.Count());
            Assert.IsTrue(1 == sut.First());
            Assert.IsTrue(3 == sut.Last());
        }

        [Test]
        public void PartialTestsTwoParameters()
        {
            Func<int, int, int> add = (x, y) => x + y;
            var inc = add.Apply(1);
            Assert.AreEqual(0, inc(-1));
        }

        [Test]
        public void PartialTestsThreeParameters()
        {
            Func<int, int, int, int> add = (x, y, z) => x + y + z;
            var add1 = add.Apply(1);
            var add11 = add.Apply(1).Apply(1);
            Assert.AreEqual(4, add1(1,2));
            Assert.AreEqual(3, add11(1));
        }

        [Test]
        public void PartialTestsFourParameters()
        {
            Func<int, int, int, int, int> add = (x, y, z, w) => x + y + z + w;
            var add1 = add.Apply(1);
            var add11 = add.Apply(1).Apply(2);
            var add111 = add.Apply(1).Apply(2).Apply(3);
            Assert.AreEqual(10, add1(2,3,4));
            Assert.AreEqual(10, add11(3,4));
            Assert.AreEqual(10, add111(4));
        }

        [Test]
        public void CurryTestTwoParameters()
        {
            Func<int, int, int> add = (x, y) => x + y;
            var sut = add.Curry()(5);
            Assert.AreEqual(7, sut(2));
        }

        [Test]
        public void CurryTestThreeParameters()
        {
            Func<int, int, int, int> add = (x, y, z) => x + y + z;
            var add1 = add.Curry()(1);
            var add12 = add.Curry()(1)(2);
            Assert.AreEqual(6, add1(2)(3));
            Assert.AreEqual(6, add12(3));
        }

        [Test]
        public void CurryTestFourParameters()
        {
            Func<int, int, int, int, int> add = (x, y, z, w) => x + y + z + w;
            var add1 = add.Curry()(1);
            var add12 = add.Curry()(1)(2);
            var add123 = add.Curry()(1)(2)(3);
            Assert.AreEqual(10, add1(2)(3)(4));
            Assert.AreEqual(10, add12(3)(4));
            Assert.AreEqual(10, add123(4));
        }

        [Test]
        public void SwapArgsTest()
        {
            Func<int, int, int> add = (x, y) => x + y;
            var dec = add.SwapArgs().Curry()(-1);
            var dec2 = add.SwapArgs().Apply(-1);
            Assert.AreEqual(2, dec(3));
            Assert.AreEqual(2, dec2(3));
        }

        [Test]
        public void ComposeTest()
        {
            Func<int, int> add5 = x => x + 5;
            Func<int, int> sub1 = x => x - 1;
            var sut = add5.Compose(sub1);
            Assert.AreEqual(5, sut(1));
        }

        [Test]
        public void ComposeOnDifferentTypesTest()
        {
            Func<int, string> add5 = x => (x + 5).ToString();
            Func<string, double> sub1 = x => double.Parse(x);
            var sut = add5.Compose(sub1);
            Assert.AreEqual(6.0, sut(1));
        }
        [Test]
        public void ComposeOnStrings()
        {
            Func<string, string> toLower = s => s.ToLowerInvariant();
            Func<string, string> removeWhiteSpace = s => s.Replace(" ", "");
            var sut = toLower.Compose(removeWhiteSpace);
            Assert.AreEqual("functionalx@gmail.com", sut("FunctionalX@gmail.com   "));
        }
    }
}
