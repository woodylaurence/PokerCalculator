using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PokerCalculator.App.Configuration;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokerCalculator.App
{
	public class PokerCalculatorConsoleAppHostedService : IHostedService
	{
		private readonly IPokerCalculator _pokerCalculator;
		private readonly AppSettings _appSettings;

		public PokerCalculatorConsoleAppHostedService(IPokerCalculator pokerCalculator, IOptions<AppSettings> appSettings)
		{
			_pokerCalculator = pokerCalculator ?? throw new ArgumentNullException(nameof(pokerCalculator));
			_appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
		}

		#region StartAsync

		public Task StartAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("Welcome to the PokerCalculator.");

			while (true)
			{
				var deck = new Deck();
				Console.WriteLine("Please enter your cards (press enter for no cards):");
				var playersHand = ReadHand(deck);
				Console.WriteLine("Please enter the board's cards (press enter for no cards):");
				var boardHand = ReadHand(deck);

				var numOpponents = ReadNumberOfOpponents();

				Console.Clear();
				Console.WriteLine($"Your hand: {string.Join(" ", playersHand.Cards.Select(x => $"{x.Value}{x.Suit}"))}");
				Console.WriteLine($"Board hand: {string.Join(" ", boardHand.Cards.Select(x => $"{x.Value}{x.Suit}"))}");
				Console.WriteLine($"{numOpponents} opponents...");

				var stopwatch = Stopwatch.StartNew();
				var pokerOdds = _pokerCalculator.CalculatePokerOdds(deck, playersHand, boardHand, numOpponents, _appSettings.NumIterations);

				DisplayPokerOdds(pokerOdds, stopwatch);

				Console.WriteLine();
				Console.WriteLine("Press any key to continue, 'X' to quit.");
				var input = Console.ReadKey();
				if (input.Key == ConsoleKey.X) break;

				Console.Clear();
			}

			return Task.CompletedTask;
		}

		private Hand ReadHand(Deck deck)
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

		private bool ParseHandFromString(string handAsString, Hand hand, Deck deck)
		{
			if (string.IsNullOrWhiteSpace(handAsString)) return true;

			var cardsForHand = handAsString.Split(' ').ToList();

			var clonedDeck = deck.Clone();
			try
			{
				var cards = cardsForHand.Select(x => new Card(x)).ToList();
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

		private int ReadNumberOfOpponents()
		{
			Console.WriteLine("Please enter the number of opponents:");
			while (true)
			{
				var numOpponentsAsString = Console.ReadLine();
				if (int.TryParse(numOpponentsAsString, out var numOpponents)) return numOpponents;
				Console.WriteLine("ERROR: There was a problem parsing that input as the number of opponents, please try again:");
			}
		}

		private void DisplayPokerOdds(PokerOdds pokerOdds, Stopwatch stopwatch)
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

		#endregion

		#region StopAsync

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		#endregion
	}
}
