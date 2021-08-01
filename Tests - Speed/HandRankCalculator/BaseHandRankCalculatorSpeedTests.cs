using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Speed.HandRankCalculator.TestData;
using PokerCalculator.Tests.Speed.PokerCalculator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PokerCalculator.Tests.Speed.HandRankCalculator
{
	public abstract class BaseHandRankCalculatorSpeedTests<THandRank, TRank> : LocalTestBase where THandRank : IHandRank<TRank>
	{
		private IHandRankCalculator<THandRank, TRank> _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = ServiceProvider.GetRequiredService<IHandRankCalculator<THandRank, TRank>>();
		}

		[Test]
		public void HandRankCalculator_SpeedTest1()
		{
			//arrange
			var deck = new Deck();
			var handRankSpeedResults = new HandRankCalculatorSpeedTestsDataObject();

			//act
			const int numIterations = 500000;
			for (var i = 0; i < numIterations; i++)
			{
				var randomCards = deck.GetRandomCards(5);
				var hand = new Hand(randomCards);

				var stopwatch = Stopwatch.StartNew();
				var handRank = _instance.CalculateHandRank(hand);
				handRankSpeedResults.PokerHandResults[handRank.PokerHand].AddCalculationData(stopwatch.ElapsedTicks);
			}

			Console.WriteLine("Total Times:");
			DisplaySpeedResults(handRankSpeedResults, x => x.TotalCalculationTicks);

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine("Average Times:");
			DisplaySpeedResults(handRankSpeedResults, x => x.AverageCalculationTicks);
		}

		private void DisplaySpeedResults(HandRankCalculatorSpeedTestsDataObject currentSpeedResults, Func<HandRankCalculatorSpeedTestVersionResult, double> propertyAccessorFunc)
		{
			var pokerHandNames = Utilities.GetEnumValues<PokerHand>();
			var columnHeaders = string.Join("|", pokerHandNames.Select(x => x.ToString().CenterString(13)));
			Console.WriteLine($"{"Version".CenterString(29)}|{columnHeaders}");
			Console.WriteLine($"-----------------------------{string.Join("", Enumerable.Range(0, 10).Select(x => "|-------------"))}");

			DisplaySpeedResult("Current", currentSpeedResults, propertyAccessorFunc);

			var assembly = GetType().Assembly;
			var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.HandRankCalculator.TestData.timing-data.json");
			using (var streamReader = new StreamReader(stream))
			{
				var timingDataObjects = JsonSerializer.Deserialize<List<HandRankCalculatorSpeedTestsDataObject>>(streamReader.ReadToEnd());
				foreach (var x in timingDataObjects.OrderByDescending(x => x.VersionOrdinal))
				{
					DisplaySpeedResult(x.VersionName, x, propertyAccessorFunc);
				}
			}
		}

		private void DisplaySpeedResult(string versionName, HandRankCalculatorSpeedTestsDataObject speedResults, Func<HandRankCalculatorSpeedTestVersionResult, double> propertyAccessorFunc)
		{
			var pokerHandTicks = speedResults.PokerHandResults.Select(x => Utilities.GetTicksAsStringWithUnit(propertyAccessorFunc(x.Value)).CenterString(13)).ToList();
			Console.WriteLine($"{versionName.CenterString(29)}|{string.Join("|", pokerHandTicks)}");
		}
	}
}