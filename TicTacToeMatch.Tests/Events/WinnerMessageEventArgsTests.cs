using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using TicTacToeMatch.Events;

namespace TicTacToeMatch.Tests.Events
{
    [ExcludeFromCodeCoverage]
    [Category("UnitTest")]
    public class WinnerMessageEventArgsTests
    {
        [Test]
        public void WinnerMessageEventArgs_DefaultConstruction_MessageIsTestWinnerIsDevil()
        {
            WinnerMessageEventArgs actual = new WinnerMessageEventArgs("Test1", "Devil");

            Assert.That(actual.Message, Is.EqualTo("Test1"));
            Assert.That(actual.Winner, Is.EqualTo("Devil"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void WinnerMessageEventArgs_MessageIsNullOrWhiteSpace_ThrowsArgumentOutOfRangeException(String message)
        {
            Assert.That(() => new WinnerMessageEventArgs(message, "TestWinner"), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void WinnerMessageEventArgs_WinnerIsNullOrWhiteSpace_ThrowsArgumentOutOfRangeException(String winner)
        {
            Assert.That(() => new WinnerMessageEventArgs("TestMessage", winner), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}