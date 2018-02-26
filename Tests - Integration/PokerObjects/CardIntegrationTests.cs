using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class CardIntegrationTests : LocalTestBase
	{
		#region Constructor

		[TestCase(CardValue.Ace, CardSuit.Clubs)]
		[TestCase(CardValue.Two, CardSuit.Diamonds)]
		[TestCase(CardValue.Six, CardSuit.Hearts)]
		[TestCase(CardValue.Queen, CardSuit.Spades)]
		public void Constructor(CardValue value, CardSuit suit)
		{
			//act
			var actual = new Card(value, suit);

			//assert
			Assert.That(actual.Value, Is.EqualTo(value));
			Assert.That(actual.Suit, Is.EqualTo(suit));
		}

		[TestCase("AD", CardValue.Ace, CardSuit.Diamonds)]
		[TestCase("4c", CardValue.Four, CardSuit.Clubs)]
		[TestCase("qs", CardValue.Queen, CardSuit.Spades)]
		[TestCase("10H", CardValue.Ten, CardSuit.Hearts)]
		public void Constructor_string(string stringValue, CardValue expectedValue, CardSuit expectedSuit)
		{
			//act
			var actual = new Card(stringValue);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}

		#endregion
	}
}
