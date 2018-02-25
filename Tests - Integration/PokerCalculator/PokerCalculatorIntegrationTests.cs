using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PokerCalculator.Tests.Integration.PokerCalculator
{
	[TestFixture]
	public class PokerCalculatorIntegrationTests : LocalTestBase
	{
		private IPokerCalculator _instance;
		private IRandomNumberGenerator _randomNumberGenerator;
		private Deck _deck;

		[SetUp]
		protected override void Setup()
		{
			_randomNumberGenerator = new RandomNumberGenerator();

			base.Setup();

			_instance = ServiceLocator.Current.GetInstance<IPokerCalculator>();
			_deck = new Deck(_randomNumberGenerator, UtilitiesService);
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().ImplementedBy<RandomNumberGenerator>());
			windsorContainer.Register(Component.For<IHandRankCalculator>().ImplementedBy<Domain.HandRankCalculator.HandRankCalculator>());
			windsorContainer.Register(Component.For<IPokerCalculator>().ImplementedBy<Domain.PokerCalculator.PokerCalculator>());
		}

		[TearDown]
		protected void TearDown()
		{
			ConfigurationManager.AppSettings["PokerCalculator.Helpers.FakeRandomNumberGenerator.RandomSeedingValue"] = "1337";
		}

		[Test]
		public void CalculatePokerOdds()
		{
			//arrange
			var first = _deck.Cards.First(x => x.Value == CardValue.Ace && x.Suit == CardSuit.Spades);
			var second = _deck.Cards.First(x => x.Value == CardValue.Seven && x.Suit == CardSuit.Diamonds);

			var third = _deck.Cards.First(x => x.Value == CardValue.Jack && x.Suit == CardSuit.Clubs);
			var fourth = _deck.Cards.First(x => x.Value == CardValue.Six && x.Suit == CardSuit.Clubs);
			var fifth = _deck.Cards.First(x => x.Value == CardValue.Four && x.Suit == CardSuit.Hearts);


			_deck.RemoveCard(first);
			_deck.RemoveCard(second);
			_deck.RemoveCard(third);
			_deck.RemoveCard(fourth);
			_deck.RemoveCard(fifth);

			var myHand = new Hand(new List<Card> { first, second });
			var boardHand = new Hand(new List<Card> { third, fourth, fifth });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 3, 10000);

			//output
			Console.WriteLine("% Win Results:");
			Console.WriteLine($"Win - {actual.WinPercentage * 100:N1}");
			Console.WriteLine($"Draw - {actual.DrawPercentage * 100:N1}");
			Console.WriteLine($"Loss - {actual.LossPercentage * 100:N1}");

			Console.WriteLine("");
			Console.WriteLine("");

			Console.WriteLine("PokerHand Results:");
			foreach (var actualPokerHandPercentage in actual.PokerHandPercentages)
			{
				Console.WriteLine($"{actualPokerHandPercentage.Key} - {actualPokerHandPercentage.Value * 100:N1}");
			}
		}
	}
}
