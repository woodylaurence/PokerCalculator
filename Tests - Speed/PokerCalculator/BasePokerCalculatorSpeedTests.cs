using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Speed.PokerCalculator.TestData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PokerCalculator.Tests.Speed.PokerCalculator
{
	public abstract class BasePokerCalculatorSpeedTests : LocalTestBase
	{
		private IPokerCalculator _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = ServiceProvider.GetRequiredService<IPokerCalculator>();
		}

		[Test]
		public void PokerCalculator_WITH_empty_hands_2_opponents_and_100000_iterations()
		{
			//arrange
			const int numIterations = 100000;

			var deck = new Deck();

			var myHand = new Hand(new List<Card>());
			var boardHand = new Hand(new List<Card>());

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			DisplaySpeedResults(stopwatch.ElapsedTicks / (double)numIterations, 1);
		}

		[Test]
		public void PokerCalculator_WITH_empty_hands_5_opponents_and_100000_iterations()
		{
			//arrange
			const int numIterations = 100000;

			var deck = new Deck();

			var myHand = new Hand(new List<Card>());
			var boardHand = new Hand(new List<Card>());

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 5, numIterations);

			DisplaySpeedResults(stopwatch.ElapsedTicks / (double)numIterations, 2);
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_2_opponents_and_100000_iterations()
		{
			//arrange
			const int numIterations = 100000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);
			var boardHand = new Hand(new List<Card>());

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			DisplaySpeedResults(stopwatch.ElapsedTicks / (double)numIterations, 3);
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_board_with_3_and_2_opponents_and_100000_iterations()
		{
			//arrange
			const int numIterations = 100000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);

			var flop = deck.TakeRandomCards(3);
			var boardHand = new Hand(flop);

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			DisplaySpeedResults(stopwatch.ElapsedTicks / (double)numIterations, 4);
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_board_with_4_and_2_opponents_and_100000_iterations()
		{
			//arrange
			const int numIterations = 100000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);

			var turn = deck.TakeRandomCards(4);
			var boardHand = new Hand(turn);

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			DisplaySpeedResults(stopwatch.ElapsedTicks / (double)numIterations, 5);
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_board_with_5_and_2_opponents_and_100000_iterations()
		{
			//arrange
			const int numIterations = 100000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);

			var river = deck.TakeRandomCards(5);
			var boardHand = new Hand(river);

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			DisplaySpeedResults(stopwatch.ElapsedTicks / (double)numIterations, 6);
		}

		#region Display Speed Results

		private void DisplaySpeedResults(double currentAverageTicksPerIteration, int testId)
		{
			Console.WriteLine("            Version          | Average / iteration  |  Iterations /s  | 1M Simulations Time");
			Console.WriteLine("-----------------------------|----------------------|-----------------|--------------------");
			DisplaySpeedResult("Current", currentAverageTicksPerIteration);

			var assembly = GetType().Assembly;
			var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.PokerCalculator.TestData.timing-data.json");
			using (var streamReader = new StreamReader(stream))
			{
				var timingDataObjects = JsonSerializer.Deserialize<List<PokerCalculatorSpeedTestsDataObject>>(streamReader.ReadToEnd());
				foreach (var x in timingDataObjects.OrderByDescending(x => x.VersionOrdinal))
				{
					DisplaySpeedResult(x.VersionName, x.Results.First(y => y.TestId == testId).TicksPerIteration);
				}
			}
		}

		private void DisplaySpeedResult(string versionName, double averageTicksPerIteration)
		{
			var timePerIteration = Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration);
			var iterationsPerSecond = (TimeSpan.TicksPerSecond / averageTicksPerIteration).ToString("N0");
			var timeForMillionSimulations = Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000);

			Console.WriteLine($"{versionName.CenterString(29)}|{timePerIteration.CenterString(22)}|{iterationsPerSecond.CenterString(17)}|{timeForMillionSimulations.CenterString(20)}");
		}

		#endregion
	}
}
