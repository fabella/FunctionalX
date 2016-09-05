using NUnit.Framework;
using FunctionalX.Monads;
namespace FunctionalX.Tests
{
    using static Functional;
    [TestFixture]
    public class ResultGenericTests
    {

        [Test]
        public void ResultOkIsSuccess()
        {
            var sut = Ok(1);
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultFailIsFailure()
        {
            var sut = Fail<string>("error");
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultImplicitOperatorSuccess()
        {
            Result<int> sut = 23;
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultImplicitOperatorFailure()
        {
            Result<int> sut = "error";
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultSuccessOnMap()
        {
            var sut = Ok(2).Map(x => x * 2);
            Assert.AreEqual(sut.Value, 4);
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultFailureOnMap()
        {
            var sut = Fail<int>("error").Map(x => x * 2);
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultSuccessOnBind()
        {
            var sut = Ok(32).Bind(x => Ok(x*2));
            Assert.AreEqual(sut.Value, 64);
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultFailureOnBind()
        {
            var sut = Fail<int>("error")
                .Bind(x => Ok(x*2));
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultSuccessOnBindAfterError()
        {
            var sut = Ok(42)
                .Map(x => x)
                .Bind(x => Fail<int>("error"));
            Assert.AreEqual("error", sut.Error);
            Assert.IsTrue(sut.Failure);
        }
    }
}
