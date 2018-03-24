using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace PokerCalculator.App
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			WindsorContainerUtilities.SetupAndConfigureWindsorContainer();
			Console.WriteLine("Welcome to the PokerCalculator.");

			while (true)
			{
				PerformPokerCalculation();
				Console.ReadLine();
				Console.Clear();
			}
		}

		private static void PerformPokerCalculation()
		{
			var deck = new Deck();
			Console.WriteLine("Please enter your cards (press enter for no cards):");
			var playersHand = ReadHand(deck);
			Console.WriteLine("Please enter the board's cards (press enter for no cards):");
			var boardHand = ReadHand(deck);

			var numOpponents = ReadNumberOfOpponents();
			var numIterations = int.Parse(ConfigurationManager.AppSettings["PokerCalculator.App.NumIterations"]);

			Console.Clear();
			Console.WriteLine($"Your hand: {string.Join(" ", playersHand.Cards.Select(x => $"{x.Value}{x.Suit}"))}");
			Console.WriteLine($"Board hand: {string.Join(" ", boardHand.Cards.Select(x => $"{x.Value}{x.Suit}"))}");
			Console.WriteLine($"{numOpponents} opponents...");

			var pokerCalculator = ServiceLocator.Current.GetInstance<IPokerCalculator>();
			var stopwatch = Stopwatch.StartNew();
			var pokerOdds = pokerCalculator.CalculatePokerOdds(deck, playersHand, boardHand, numOpponents, numIterations);

			DisplayPokerOdds(pokerOdds, stopwatch);
		}

		private static Hand ReadHand(Deck deck)
		{
			var hand = new Hand(new List<Card>());
			while (true)
			{
				var handAsString = Console.ReadLine();
				var handParsedSuccessfully = ParseHandFromString(handAsString, hand, deck);
				if (handParsedSuccessfully) return hand;
				Console.WriteLine("ERROR: There was a problem reading those cards, please try again:");
			}
		}

		private static bool ParseHandFromString(string handAsString, Hand hand, Deck deck)
		{
			if (string.IsNullOrWhiteSpace(handAsString)) return true;

			var cardsForHand = handAsString.Split(' ').ToList();
			var utiltiesService = ServiceLocator.Current.GetInstance<IUtilitiesService>();

			var clonedDeck = deck.Clone();
			try
			{
				var cards = cardsForHand.Select(x => new Card(x, utiltiesService)).ToList();
				cards.ForEach(card =>
				{
					hand.AddCard(card);
					deck.RemoveCard(card.Value, card.Suit);
				});
				return true;
			}
			catch (Exception)
			{
				deck.Cards = clonedDeck.Cards;
				hand.Cards.Clear();
				return false;
			}
		}

		private static int ReadNumberOfOpponents()
		{
			Console.WriteLine("Please enter the number of opponents:");
			while (true)
			{
				var numOpponentsAsString = Console.ReadLine();
				if (int.TryParse(numOpponentsAsString, out var numOpponents)) return numOpponents;
				Console.WriteLine("ERROR: There was a problem parsing that input as the number of opponents, please try again:");
			}
		}

		private static void DisplayPokerOdds(PokerOdds pokerOdds, Stopwatch stopwatch)
		{
			Console.WriteLine();
			Console.WriteLine($"Results (calculated in {stopwatch.ElapsedMilliseconds}ms):");
			Console.WriteLine($"Win : {pokerOdds.WinPercentageWithError}");
			Console.WriteLine($"Draw : {pokerOdds.DrawPercentageWithError}");
			Console.WriteLine($"Loss : {pokerOdds.LossPercentageWithError}");

			Console.WriteLine();
			Console.WriteLine("Hand Odds:");
			foreach (var pokerHandOdds in pokerOdds.PokerHandPercentagesWithErrors.OrderByDescending(x => x.Key))
			{
				Console.WriteLine($"{pokerHandOdds.Key} : {pokerHandOdds.Value}");
			}
		}
	}
}
