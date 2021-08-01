using NUnit.Framework;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Tests.Shared;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.Domain.PokerCalculator
{
	[TestFixture]
	public class PokerOddsUnitTests : AbstractUnitTestBase
	{
		#region Constructor

		[Test]
		public void Constructor()
		{
			//act
			var actual = new PokerOdds();

			//assert
			//todo think these should all be set to an empty value, not null
			Assert.That(actual.WinPercentageWithError, Is.Null);
			Assert.That(actual.DrawPercentageWithError, Is.Null);
			Assert.That(actual.LossPercentageWithError, Is.Null);
			Assert.That(actual.PokerHandPercentagesWithErrors, Is.Null);
		}

		#endregion

		#region AggregatePokerOdds

		[Test]
		public void AggregatePokerOdds_WHERE_single_item_supplied_SHOULD_throw_error()
		{
			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => PokerOdds.AggregatePokerOdds(new List<PokerOdds> { new PokerOdds() }));
			Assert.That(actualException.Message, Is.EqualTo("Cannot aggregate less than two PokerOdds"));
		}

		[Test]
		[Ignore("Cannot set data for these tests, think they will make more sense when we rework the object")]
		public void AggregatePokerOdds_WHERE_multiple_poker_odds_SHOULD_return_aggregated_poker_odds()
		{
			//arrange
			/*var pokerOdds1 = new PokerOdds
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
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.EqualTo(0.0123382).Within(0.0000001));*/
		}

		#endregion
	}
}