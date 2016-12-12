using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class DeckIntegrationTests : LocalTestBase
	{
		#region Create

		[Test]
		public void Create_SHOULD_return_deck_full_of_every_card()
		{
			//act
			var actual = Deck.Create();

			//assert
			Assert.That(actual, Has.Count.EqualTo(52));
			Assert.That(actual.TrueForAll(x => CardTestCases.AllCards.Contains(x)));
			Assert.That(CardTestCases.AllCards.TrueForAll(x => actual.Contains(x)));
			Assert.Fail("Something about ordering of the deck");
		}

		#endregion
	}
}
