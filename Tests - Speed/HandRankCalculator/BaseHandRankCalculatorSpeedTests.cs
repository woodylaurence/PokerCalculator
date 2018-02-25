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
	public abstract class BaseHandRankCalculatorSpeedTests : LocalTestBase
	{
		private IHandRankCalculator _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = ServiceLocator.Current.GetInstance<IHandRankCalculator>();
		}

		[Test]
		public void HandRankCalculator_SpeedTest1()
		{
			//arrange
			var deck = new Deck();
			var handRankSpeedResults = new HandRankSpeedResults(UtilitiesService);

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
		}

		private class HandRankSpeedResults
		{
			private long TotalCalculationTime => HandRankCalculationTimes.Sum(x => x.Value);
			private Dictionary<PokerHand, double> HandRankAverageCalculationTimes => HandRankCalculationTimes.ToDictionary(x => x.Key, x => HandRankFrequency[x.Key] == 0 ? 0 : x.Value / (double)HandRankFrequency[x.Key]);

			public Dictionary<PokerHand, long> HandRankCalculationTimes { get; }
			public Dictionary<PokerHand, int> HandRankFrequency { get; }

			public HandRankSpeedResults(IUtilitiesService utilitiesService)
			{
				HandRankCalculationTimes = utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0L);
				HandRankFrequency = utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0);
			}

			public void DisplaySpeedResults()
			{
				Console.WriteLine($"Total time spent: {TotalCalculationTime / TimeSpan.TicksPerMillisecond}ms");
				Console.WriteLine("PokerHand breakdown:");

				foreach (var handRankSpeedResult in HandRankAverageCalculationTimes)
				{
					var pokerHand = handRankSpeedResult.Key;
					var displayPokerHand = pokerHand.ToString().PadRight(14);
					var averageTimeAsString = GetTimedValueAsStringWithUnit(handRankSpeedResult.Value);
					var totalTimeAsString = GetTimedValueAsStringWithUnit(HandRankCalculationTimes[pokerHand]);
					Console.WriteLine($"{displayPokerHand} : Total time - {totalTimeAsString.PadRight(11)}\tAverage time - {averageTimeAsString}");
				}
			}

			private string GetTimedValueAsStringWithUnit(double ticks)
			{
				if (ticks > TimeSpan.TicksPerMillisecond) return $"{ticks / TimeSpan.TicksPerMillisecond:N1}ms";
				if (ticks * 1000 > TimeSpan.TicksPerMillisecond) return $"{ticks * 1000 / TimeSpan.TicksPerMillisecond:N1}μs";
				return $"{ticks * 1000000 / TimeSpan.TicksPerMillisecond:N1}ns";
			}
		}
	}
}