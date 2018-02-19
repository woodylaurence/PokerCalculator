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

		#endregion
	}
}
