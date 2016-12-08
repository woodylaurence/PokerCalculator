using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class CardIntegrationTests : LocalTestBase
	{
		#region Create

		[TestCase(CardValue.Ace, CardSuit.Clubs)]
		[TestCase(CardValue.Two, CardSuit.Diamonds)]
		[TestCase(CardValue.Six, CardSuit.Hearts)]
		[TestCase(CardValue.Queen, CardSuit.Spades)]
		public void Create(CardValue value, CardSuit suit)
		{
			//act
			var actual = Card.Create(value, suit);

			//assert
			Assert.That(actual.Value, Is.EqualTo(value));
			Assert.That(actual.Suit, Is.EqualTo(suit));
		}

		#endregion
	}
}
