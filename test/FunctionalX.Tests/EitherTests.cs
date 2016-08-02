using System.IO.MemoryMappedFiles;
using NUnit.Framework;
using FunctionalX.Monads;
namespace FunctionalX.Tests
{
    [TestFixture]
    public class EitherTests
    {

        [Test]
        public void EitherLeftMatchTest()
        {
            var sut = Either.Of<string, int>("hello");
            Assert.AreEqual("hello", sut.Match(Left: x => x, Right: null));
        }

        [Test]
        public void EitherRightMatchTest()
        {
            var sut = Either.Of<string, int>(42);
            Assert.AreEqual(42, sut.Match(Left: null, Right: x => x));
        }


        [Test]
        public void EitherLeftMapTest()
        {
            var sut = Either.Of<string, int>("hello");
            var temp = "unmodified";
            sut.Map(x => temp = "modified");
            Assert.AreEqual("unmodified", temp);
        }

        [Test]
        public void EitherRightMapTest()
        {
            var sut = Either.Of<string, int>(42);
            Assert.AreEqual(84, sut.Map(x => x * 2));
        }
    }
}
