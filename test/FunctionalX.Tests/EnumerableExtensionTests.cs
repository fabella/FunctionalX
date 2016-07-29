using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace FunctionalX.Tests
{
    using static Functional;
    [TestFixture]
    public class EnumerableExtensionTests
    {
        [Test]
        public void MapTest()
        {
            var sut = List(1, 2).Map(x => x*2);
            Assert.AreEqual(2, sut.Count());
            Assert.AreEqual(2, sut.First());
            Assert.AreEqual(4, sut.Last());
        }

        [Test]
        public void BindTest()
        {
            var sut = List("bulbasaur Ivysaur Venasaur", "charmander charmeleao squirtle")
                .Bind(s => s.Split(' '));
            Assert.AreEqual(6, sut.Count());
            Assert.AreEqual("bulbasaur", sut.First());
            Assert.AreEqual("squirtle", sut.Last());
        }

        [Test]
        public void BindTestWithMaybe()
        {
            var sut = List("44", "pikachu", "42")
                .Bind(s => s.ParseInt());
            Assert.AreEqual(2, sut.Count());
            Assert.AreEqual(44, sut.First());
            Assert.AreEqual(42, sut.Last());
        }

        [Test]
        public void BindTestWithMaybeAndEnumerable()
        {
            var sut = Just("pikachu raichu")
                .Bind(s => s.Split(' '));

            Assert.AreEqual(2, sut.Count());
            Assert.AreEqual("pikachu", sut.First());
            Assert.AreEqual("raichu", sut.Last());
        }

        [Test]
        public void ForEachTest()
        {
            var list = new List<Person> { new Person() {Name = "Jon"} };
            var sut = list.AsEnumerable().ForEach(p => { p.Name = "Mark"; });
            Assert.AreEqual("Mark", list.First().Name);
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}
