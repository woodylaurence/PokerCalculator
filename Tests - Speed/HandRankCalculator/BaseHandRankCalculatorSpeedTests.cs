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
		public override void Setup()
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
			const int numIterations = 10000;
			for (var i = 0; i < numIterations; i++)
			{
				var randomCards = deck.GetRandomCards(5);
				var hand = new Hand(randomCards);

				var stopwatch = Stopwatch.StartNew();
				var handRank = _instance.CalculateHandRank(hand);
				handRankSpeedResults.HandRankCalculationTimes[handRank.PokerHand] += stopwatch.ElapsedTicks;
				handRankSpeedResults.HandRankFrequency[handRank.PokerHand]++;
			}

			Console.WriteLine($"Total time spent: {handRankSpeedResults.TotalCalculationTime / TimeSpan.TicksPerMillisecond}ms");
			Console.WriteLine("PokerHand breakdown:");
			foreach (var handRankSpeedResult in handRankSpeedResults.HandRankAverageCalculationTimes)
			{
				var pokerHand = handRankSpeedResult.Key;
				var total = handRankSpeedResults.HandRankFrequency[pokerHand];
				var averageTime = 1000 * handRankSpeedResult.Value / TimeSpan.TicksPerMillisecond;
				var totalTime = handRankSpeedResults.HandRankCalculationTimes[pokerHand] / TimeSpan.TicksPerMillisecond;
				Console.WriteLine($"{pokerHand.ToString()} : Probability - {(total / (double)numIterations):N7}\tTotal - {total}\tTotal time - {totalTime}ms\tAverage time - {averageTime:N1}μs");
			}
		}

		private class HandRankSpeedResults
		{
			public long TotalCalculationTime => HandRankCalculationTimes.Sum(x => x.Value);

			public Dictionary<PokerHand, long> HandRankCalculationTimes { get; }
			public Dictionary<PokerHand, int> HandRankFrequency { get; }
			public Dictionary<PokerHand, double> HandRankAverageCalculationTimes => HandRankCalculationTimes.ToDictionary(x => x.Key, x => HandRankFrequency[x.Key] == 0 ? 0 : x.Value / (double)HandRankFrequency[x.Key]);

			public HandRankSpeedResults(IUtilitiesService utilitiesService)
			{
				HandRankCalculationTimes = utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0L);
				HandRankFrequency = utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0);
			}
		}
	}
}