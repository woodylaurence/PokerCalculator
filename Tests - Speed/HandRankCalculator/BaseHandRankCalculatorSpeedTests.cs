using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PokerCalculator.Tests.Speed.HandRankCalculator
{
	public abstract class BaseHandRankCalculatorSpeedTests<THandRank, TRank> : LocalTestBase where THandRank : IHandRank<TRank>
	{
		private IHandRankCalculator<THandRank, TRank> _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = ServiceLocator.Current.GetInstance<IHandRankCalculator<THandRank, TRank>>();
		}

		[Test]
		public void HandRankCalculator_SpeedTest1()
		{
			//arrange
			var deck = new Deck();
			var handRankSpeedResults = new HandRankSpeedResults();

			//act
			const int numIterations = 500000;
			for (var i = 0; i < numIterations; i++)
			{
				var randomCards = deck.GetRandomCards(5);
				var hand = new Hand(randomCards);

				var stopwatch = Stopwatch.StartNew();
				var handRank = _instance.CalculateHandRank(hand);
				handRankSpeedResults.HandRankCalculationTimes[handRank.PokerHand] += stopwatch.ElapsedTicks;
				handRankSpeedResults.HandRankFrequency[handRank.PokerHand]++;
			}

			handRankSpeedResults.DisplaySpeedResults();
			handRankSpeedResults.DisplayPreviousSpeedResults();
		}

		private class HandRankSpeedResults
		{
			private long TotalCalculationTime => HandRankCalculationTimes.Sum(x => x.Value);
			private Dictionary<PokerHand, double> HandRankAverageCalculationTimes => HandRankCalculationTimes.ToDictionary(x => x.Key, x => HandRankFrequency[x.Key] == 0 ? 0 : x.Value / (double)HandRankFrequency[x.Key]);

			public Dictionary<PokerHand, long> HandRankCalculationTimes { get; }
			public Dictionary<PokerHand, int> HandRankFrequency { get; }

			public HandRankSpeedResults()
			{
				HandRankCalculationTimes = Utilities.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0L);
				HandRankFrequency = Utilities.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0);
			}

			public void DisplaySpeedResults()
			{
				Console.WriteLine($"Total time spent: {Utilities.GetTicksAsStringWithUnit(TotalCalculationTime)}");
				Console.WriteLine("PokerHand breakdown:");

				foreach (var handRankSpeedResult in HandRankAverageCalculationTimes)
				{
					var pokerHand = handRankSpeedResult.Key;
					var displayPokerHand = pokerHand.ToString().PadRight(14);
					var averageTimeAsString = Utilities.GetTicksAsStringWithUnit(handRankSpeedResult.Value);
					var totalTimeAsString = Utilities.GetTicksAsStringWithUnit(HandRankCalculationTimes[pokerHand]);
					Console.WriteLine($"{displayPokerHand} : Total time - {totalTimeAsString.PadRight(11)}\tAverage time - {averageTimeAsString}");
				}
			}

			public void DisplayPreviousSpeedResults()
			{
				Console.WriteLine("\r\nPrevious version");

				var oldHandRankAverageCalculationTimes = new Dictionary<PokerHand, string>
				{
					{ PokerHand.HighCard, "19.2μs" },
					{ PokerHand.Pair, "19.1μs" },
					{ PokerHand.TwoPair, "17.6μs" },
					{ PokerHand.ThreeOfAKind, "18.3μs" },
					{ PokerHand.Straight, "14.2μs" },
					{ PokerHand.Flush, "15.0μs" },
					{ PokerHand.FullHouse, "16.4" },
					{ PokerHand.FourOfAKind, "25.1μs" },
					{ PokerHand.StraightFlush, "13.2μs" },
					{ PokerHand.RoyalFlush, "13.3μs" }
				};


				foreach (var handRankSpeedResult in oldHandRankAverageCalculationTimes)
				{
					Console.WriteLine($"{handRankSpeedResult.Key.ToString().PadRight(14)} : Average time - {handRankSpeedResult.Value}");
				}
			}
		}
	}
}