using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using TicTacToeMatch.Entities;

namespace TicTacToeMatch.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    [Category("UnitTest")]
    public class PointIndexTests
    {

        [Test]
        public void PointIndex_DefaultConstruction_XIsZeroAndYIsZero()
        {
            PointIndex actual = new PointIndex();

            Assert.That(actual.X, Is.EqualTo(0));
            Assert.That(actual.Y, Is.EqualTo(0));
        }

        [Test]
        public void PointIndex_ConstructionWithPArameters_XIsFiveAndYIsThree()
        {
            PointIndex actual = new PointIndex(5, 3);

            Assert.That(actual.X, Is.EqualTo(5));
            Assert.That(actual.Y, Is.EqualTo(3));
        }

        [Test]
        public void Equals_BothXAndYAreEqually_ResultIsTrue()
        {
            // 1) Parameter ist kein Typ PointIndex!
            //    => Ist ein PointIndex ein BitConverter??? => Nein
            // 2) Parameter ist vom Type PointIndex!
            // 2a) beide sind gleich wenn x und y gleich sind
            // 2b) beide sind ungleich wenn entweder x oder y ungleich ist.

            // Bsp 2a.
            // arrange
            PointIndex other = new PointIndex(5, 3);
            PointIndex testee = new PointIndex(5, 3);

            // act
            Boolean actual = testee.Equals(other);

            //assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Equals_ParameterIsNotTypeOfPointIndex_ResultIsFalse()
        {
            PointIndex actual = new PointIndex();
            String s = "foo";
            Boolean test = actual.Equals(s);

            Assert.That(test, Is.False);
        }

        // Elegant way
        [Test]
        [TestCase(5, 2)]
        [TestCase(4, 3)]
        [TestCase(4, 2)]
        public void Equals_OtherXIsNotFiveOrOtherYIsNotThree_ResultIsFalse(Int32 x, Int32 y)
        {
            // arrange
            PointIndex testee = new PointIndex(5, 3);
            PointIndex other = new PointIndex(x, y);

            // act
            Boolean actual = testee.Equals(other);

            //assert
            Assert.That(actual, Is.False);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        public void GetHashCode_VerifyHashCodeIsEqually_ResultIsTrue(Int32 x, Int32 y)
        {
            PointIndex actual = new PointIndex(x,y);

            Int32 expected = (x.ToString() + y.ToString()).GetHashCode();

            Assert.That(actual.GetHashCode(), Is.EqualTo(expected));
        }
    }
}