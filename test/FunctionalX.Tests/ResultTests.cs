using NUnit.Framework;
using FunctionalX.Monads;
namespace FunctionalX.Tests
{
    using static Functional;
    [TestFixture]
    public class ResultTests
    {

        [Test]
        public void ResultOkIsSuccess()
        {
            var sut = Ok();
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultFailIsFailure()
        {
            var sut = Fail("error");
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultImplicitOperatorSuccess()
        {
            Result sut = true;
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultImplicitOperatorFailure()
        {
            Result sut = "error";
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultSuccessOnMap()
        {
            var called = false;
            var sut = Ok().Map(() => called = true);
            Assert.IsTrue(called);
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultFailureOnMap()
        {
            var called = false;
            var sut = Fail("error").Map(() => called = true);
            Assert.IsFalse(called);
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultSuccessOnBind()
        {
            var called = false;
            var sut = Ok().Bind(() =>
            {
                called = true;
                return Ok();
            });
            Assert.IsTrue(called);
            Assert.IsTrue(sut.Success);
        }

        [Test]
        public void ResultFailureOnBind()
        {
            var called = false;
            var sut = Fail("error").Map(() => called = true);
            Assert.IsFalse(called);
            Assert.IsTrue(sut.Failure);
            Assert.AreEqual("error", sut.Error);
        }

        [Test]
        public void ResultSuccessOnBindAfterError()
        {
            var called = false;
            var second = false;
            var sut = Ok()
                .Map(() => called = true)
                .Bind(() => Fail("error"))
                .Map(() => second = true);
            Assert.IsTrue(called);
            Assert.IsFalse(second);
            Assert.AreEqual("error", sut.Error);
            Assert.IsTrue(sut.Failure);
        }
    }
}
