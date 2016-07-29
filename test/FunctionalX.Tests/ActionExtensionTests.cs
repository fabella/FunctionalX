using System;
using NUnit.Framework;

namespace FunctionalX.Tests
{
    using static Functional;
    [TestFixture]
    public class ActionExtensionTests
    {
        [Test]
        public void ToFuncTest()
        {
            var name = "Jon";
            Action changeNamge = () => { name = "Mark"; };
            var sut = changeNamge.ToFunc()();
            Assert.AreEqual(sut, Unit());
            Assert.AreEqual(name, "Mark");
        }
        [Test]
        public void ToFuncOneParameterTest()
        {
            Action<Person> changeNamge = (p) => { p.Name = "Mark"; };
            var person = new Person() {Name = "Jon"};
            var sut = changeNamge.ToFunc()(person);
            Assert.AreEqual(sut, Unit());
            Assert.AreEqual(person.Name, "Mark");
        }
        [Test]
        public void ToFuncTwoParameterTest()
        {
            Action<Person, Person> swapNames = (p1, p2) =>
            {
                var name = p1.Name;
                p1.Name = p2.Name;
                p2.Name = name;
            };
            var jon = new Person() {Name = "Jon"};
            var mark = new Person() {Name = "Mark"};
            var sut = swapNames.ToFunc()(jon, mark);
            Assert.AreEqual(sut, Unit());
            Assert.AreEqual(jon.Name, "Mark");
            Assert.AreEqual(mark.Name, "Jon");
        }
    }
}
