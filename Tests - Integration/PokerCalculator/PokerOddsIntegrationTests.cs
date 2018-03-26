using NUnit.Framework;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.PokerCalculator
{
	[TestFixture]
	public class PokerOddsIntegrationTests : LocalTestBase
	{
		#region Constructor

		[Test]
		public void Constructor()
		{
			//act
			var actual = new PokerOdds();

			//assert
			Assert.That(actual.NumWins, Is.EqualTo(0));
			Assert.That(actual.NumDraws, Is.EqualTo(0));
			Assert.That(actual.NumLosses, Is.EqualTo(0));
			Assert.That(actual.TotalNumHands, Is.EqualTo(0));

			Assert.That(actual.WinPercentage, Is.EqualTo(0));
			Assert.That(actual.DrawPercentage, Is.EqualTo(0));
			Assert.That(actual.LossPercentage, Is.EqualTo(0));

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

			Assert.That(actual.PokerHandFrequencies, Has.Count.EqualTo(10));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.RoyalFlush));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.StraightFlush));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.FourOfAKind));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.FullHouse));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.Flush));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.Straight));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.ThreeOfAKind));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.TwoPair));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.Pair));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.HighCard));
			Assert.That(actual.PokerHandFrequencies.Values, Has.All.EqualTo(0));
		}

		#endregion

		#region Static Methods

		#region AggregatePokerOdds

		[Test]
		public void AggregatePokerOdds_WHERE_multiple_poker_odds_SHOULD_return_aggregated_poker_odds()
		{
			//arrange
			var pokerOdds1 = new PokerOdds
			{
				NumWins = 3871,
				NumDraws = 891,
				NumLosses = 7825,
				PokerHandFrequencies = new Dictionary<PokerHand, int>
				{
					{ PokerHand.RoyalFlush, 2 },
					{ PokerHand.StraightFlush, 4 },
					{ PokerHand.FourOfAKind, 15 },
					{ PokerHand.FullHouse, 56 },
					{ PokerHand.Flush, 90 },
					{ PokerHand.Straight, 182 },
					{ PokerHand.ThreeOfAKind, 4614 },
					{ PokerHand.TwoPair, 3486 },
					{ PokerHand.Pair, 6441 },
					{ PokerHand.HighCard, 4861 }
				}
			};

			var pokerOdds2 = new PokerOdds
			{
				NumWins = 2168,
				NumDraws = 1021,
				NumLosses = 9398,
				PokerHandFrequencies = new Dictionary<PokerHand, int>
				{
					{ PokerHand.RoyalFlush, 1 },
					{ PokerHand.StraightFlush, 7 },
					{ PokerHand.FourOfAKind, 13 },
					{ PokerHand.FullHouse, 78 },
					{ PokerHand.Flush, 102 },
					{ PokerHand.Straight, 170 },
					{ PokerHand.ThreeOfAKind, 3892 },
					{ PokerHand.TwoPair, 4022 },
					{ PokerHand.Pair, 5862 },
					{ PokerHand.HighCard, 5237 }
				}
			};

			var pokerOdds3 = new PokerOdds
			{
				NumWins = 3056,
				NumDraws = 945,
				NumLosses = 8586,
				PokerHandFrequencies = new Dictionary<PokerHand, int>
				{
					{ PokerHand.RoyalFlush, 0 },
					{ PokerHand.StraightFlush, 5 },
					{ PokerHand.FourOfAKind, 18 },
					{ PokerHand.FullHouse, 60 },
					{ PokerHand.Flush, 88 },
					{ PokerHand.Straight, 143 },
					{ PokerHand.ThreeOfAKind, 4165 },
					{ PokerHand.TwoPair, 3777 },
					{ PokerHand.Pair, 5900 },
					{ PokerHand.HighCard, 4999 }
				}
			};

			var listOfPokerOdds = new List<PokerOdds> { pokerOdds1, pokerOdds2, pokerOdds3 };

			//act
			var actual = PokerOdds.AggregatePokerOdds(listOfPokerOdds);

			//assert
			Assert.That(actual.WinPercentageWithError.Percentage, Is.EqualTo(0.240857).Within(0.000001));
			Assert.That(actual.WinPercentageWithError.Error, Is.EqualTo(0.055252).Within(0.000001));
			Assert.That(actual.DrawPercentageWithError.Percentage, Is.EqualTo(0.075660).Within(0.000001));
			Assert.That(actual.DrawPercentageWithError.Error, Is.EqualTo(0.004237).Within(0.000001));
			Assert.That(actual.LossPercentageWithError.Percentage, Is.EqualTo(0.683483).Within(0.000001));
			Assert.That(actual.LossPercentageWithError.Error, Is.EqualTo(0.051028).Within(0.000001));

			Assert.That(actual.PokerHandPercentagesWithErrors, Has.Count.EqualTo(10));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Percentage, Is.EqualTo(0.00007945).Within(0.00000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error, Is.EqualTo(0.00006487).Within(0.00000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Percentage, Is.EqualTo(0.0004237).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error, Is.EqualTo(0.0000991).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Percentage, Is.EqualTo(0.0012182).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error, Is.EqualTo(0.0001632).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Percentage, Is.EqualTo(0.0051376).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error, Is.EqualTo(0.0007602).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Percentage, Is.EqualTo(0.0074151).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error, Is.EqualTo(0.0004912).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Percentage, Is.EqualTo(0.0131088).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error, Is.EqualTo(0.0012957).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Percentage, Is.EqualTo(0.3355579).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error, Is.EqualTo(0.0236482).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Percentage, Is.EqualTo(0.2988533).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error, Is.EqualTo(0.0174060).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Percentage, Is.EqualTo(0.4820582).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error, Is.EqualTo(0.0210091).Within(0.0000001));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Percentage, Is.EqualTo(0.3998040).Within(0.0000001));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.EqualTo(0.0123382).Within(0.0000001));
		}

		#endregion

		#endregion
	}
}
