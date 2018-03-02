using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared.TestData;

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
		public void Constructor_string_WHERE_different_casing_SHOULD_not_care_about_casing(string stringValue, CardValue expectedValue, CardSuit expectedSuit)
		{
			//act
			var actual = new Card(stringValue, UtilitiesService);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsAsString))]
		public void Constructor_string(string cardAsString, CardValue expectedCardValue, CardSuit expectedCardSuit)
		{
			//act
			var actual = new Card(cardAsString, UtilitiesService);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedCardValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedCardSuit));
		}

		#endregion
	}
}
