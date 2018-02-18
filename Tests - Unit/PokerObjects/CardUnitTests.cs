using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class CardUnitTests : AbstractUnitTestBase
	{
		#region Constructor

		[Test, TestCaseSource(typeof(CardTestCases), nameof(CardTestCases.AllCardsTestCaseData))]
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
