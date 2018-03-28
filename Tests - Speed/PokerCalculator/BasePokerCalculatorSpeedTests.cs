using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PokerCalculator.Tests.Speed.PokerCalculator
{
	public abstract class BasePokerCalculatorSpeedTests : LocalTestBase
	{
		private IPokerCalculator _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = ServiceLocator.Current.GetInstance<IPokerCalculator>();
		}

		[Test]
		public void PokerCalculator_WITH_empty_hands_2_opponents_and_10000_iterations()
		{
			//arrange
			const int numIterations = 10000;

			var deck = new Deck();

			var myHand = new Hand(new List<Card>());
			var boardHand = new Hand(new List<Card>());

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			var averageTicksPerIteration = stopwatch.ElapsedTicks / numIterations;
			Console.WriteLine($"Average time per iteration: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration)}");
			Console.WriteLine($"Maximum iterations possible in 1 second: {TimeSpan.TicksPerSecond / averageTicksPerIteration:N0}");
			Console.WriteLine($"Time for 1,000,000 simulations: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000)}");

			Console.WriteLine("\r\nPrevious verison");
			Console.WriteLine("Average time per iteration: 164.9μs");
		}

		[Test]
		public void PokerCalculator_WITH_empty_hands_5_opponents_and_10000_iterations()
		{
			//arrange
			const int numIterations = 10000;

			var deck = new Deck();

			var myHand = new Hand(new List<Card>());
			var boardHand = new Hand(new List<Card>());

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 5, numIterations);

			var averageTicksPerIteration = stopwatch.ElapsedTicks / numIterations;
			Console.WriteLine($"Average time per iteration: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration)}");
			Console.WriteLine($"Maximum iterations possible in 1 second: {TimeSpan.TicksPerSecond / averageTicksPerIteration:N0}");
			Console.WriteLine($"Time for 1,000,000 simulations: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000)}");

			Console.WriteLine("\r\nPrevious verison");
			Console.WriteLine("Average time per iteration: 168.5μs");
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_2_opponents_and_10000_iterations()
		{
			//arrange
			const int numIterations = 10000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);
			var boardHand = new Hand(new List<Card>());

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			var averageTicksPerIteration = stopwatch.ElapsedTicks / numIterations;
			Console.WriteLine($"Average time per iteration: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration)}");
			Console.WriteLine($"Maximum iterations possible in 1 second: {TimeSpan.TicksPerSecond / averageTicksPerIteration:N0}");
			Console.WriteLine($"Time for 1,000,000 simulations: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000)}");

			Console.WriteLine("\r\nPrevious verison");
			Console.WriteLine("Average time per iteration: 101.6μs");
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_board_with_3_and_2_opponents_and_10000_iterations()
		{
			//arrange
			const int numIterations = 10000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);

			var flop = deck.TakeRandomCards(3);
			var boardHand = new Hand(flop);

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			var averageTicksPerIteration = stopwatch.ElapsedTicks / numIterations;
			Console.WriteLine($"Average time per iteration: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration)}");
			Console.WriteLine($"Maximum iterations possible in 1 second: {TimeSpan.TicksPerSecond / averageTicksPerIteration:N0}");
			Console.WriteLine($"Time for 1,000,000 simulations: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000)}");

			Console.WriteLine("\r\nPrevious verison");
			Console.WriteLine("Average time per iteration: 100.5μs");
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_board_with_4_and_2_opponents_and_10000_iterations()
		{
			//arrange
			const int numIterations = 10000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);

			var turn = deck.TakeRandomCards(4);
			var boardHand = new Hand(turn);

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			var averageTicksPerIteration = stopwatch.ElapsedTicks / numIterations;
			Console.WriteLine($"Average time per iteration: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration)}");
			Console.WriteLine($"Maximum iterations possible in 1 second: {TimeSpan.TicksPerSecond / averageTicksPerIteration:N0}");
			Console.WriteLine($"Time for 1,000,000 simulations: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000)}");

			Console.WriteLine("\r\nPrevious verison");
			Console.WriteLine("Average time per iteration: 99.4μs");
		}

		[Test]
		public void PokerCalculator_WITH_my_hand_with_2_cards_and_board_with_5_and_2_opponents_and_10000_iterations()
		{
			//arrange
			const int numIterations = 10000;

			var deck = new Deck();

			var myCards = deck.TakeRandomCards(2);
			var myHand = new Hand(myCards);

			var river = deck.TakeRandomCards(5);
			var boardHand = new Hand(river);

			//act
			var stopwatch = Stopwatch.StartNew();
			_instance.CalculatePokerOdds(deck, myHand, boardHand, 2, numIterations);

			var averageTicksPerIteration = stopwatch.ElapsedTicks / numIterations;
			Console.WriteLine($"Average time per iteration: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration)}");
			Console.WriteLine($"Maximum iterations possible in 1 second: {TimeSpan.TicksPerSecond / averageTicksPerIteration:N0}");
			Console.WriteLine($"Time for 1,000,000 simulations: {Utilities.GetTicksAsStringWithUnit(averageTicksPerIteration * 1000000)}");

			Console.WriteLine("\r\nPrevious verison");
			Console.WriteLine("Average time per iteration: 95.0μs");
		}
	}
}
