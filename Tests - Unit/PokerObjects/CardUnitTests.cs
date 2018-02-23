using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class CardUnitTests : AbstractUnitTestBase
	{
		#region Constructor

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsTestCaseData))]
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
