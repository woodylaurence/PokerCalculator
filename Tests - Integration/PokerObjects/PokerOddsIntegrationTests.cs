using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class PokerOddsIntegrationTests : LocalTestBase
	{
		[Test]
		public void Constructor()
		{
			//act
			var actual = new PokerOdds(UtilitiesService);

			//assert
			Assert.That(actual.WinPercentage, Is.EqualTo(0));
			Assert.That(actual.DrawPercentage, Is.EqualTo(0));
			Assert.That(actual.LosePercentage, Is.EqualTo(0));

			Assert.That(actual.PokerHandPercentages, Has.Count.EqualTo(10));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.RoyalFlush));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.StraightFlush));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.FourOfAKind));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.FullHouse));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.Flush));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.Straight));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.ThreeOfAKind));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.TwoPair));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.Pair));
			Assert.That(actual.PokerHandPercentages.ContainsKey(PokerHand.HighCard));

			Assert.That(actual.PokerHandPercentages.Values, Has.All.EqualTo(0));
		}
	}
}
