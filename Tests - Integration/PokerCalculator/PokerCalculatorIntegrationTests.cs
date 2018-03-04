using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared.TestObjects;
using System;
using System.Collections.Generic;
using System.Configuration;

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
			_randomNumberGenerator = new FakeRandomNumberGenerator();

			base.Setup();

			_instance = ServiceLocator.Current.GetInstance<IPokerCalculator>();
			_deck = new Deck(_randomNumberGenerator, UtilitiesService);
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));
			windsorContainer.Register(Component.For<IHandRankCalculator>().ImplementedBy<Domain.HandRankCalculator.HandRankCalculator>());
			windsorContainer.Register(Component.For<IPokerCalculator>().ImplementedBy<Domain.PokerCalculator.PokerCalculator>());
		}

		[TearDown]
		protected void TearDown()
		{
			ConfigurationManager.AppSettings["PokerCalculator.Helpers.FakeRandomNumberGenerator.RandomSeedingValue"] = "1337";
		}

		[Test]
		public void CalculatePokerOdds_WHERE_no_cards_in_hand_or_board_with_three_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHand = new Hand(new List<Card>());
			var boardHand = new Hand(new List<Card>());

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 3, 100000);

			//output
			//			Assert.That(actual.WinPercentage, Is.InRange() Is.EqualTo(0.4).Within());

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

		[Test]
		public void CalculatePokerOdds_WHERE_4D_and_5C_in_hand_no_cards_on_board_and_three_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = new Card(CardValue.Four, CardSuit.Diamonds);
			var myHandCard2 = new Card(CardValue.Five, CardSuit.Clubs);

			_deck.RemoveCard(myHandCard1);
			_deck.RemoveCard(myHandCard2);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });
			var boardHand = new Hand(new List<Card>());

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 3, 100000);

			//output
			Assert.That(actual.WinPercentage, Is.InRange(0.1735, 0.1785));
			Assert.That(actual.DrawPercentage, Is.InRange(0.029, 0.033));
			Assert.That(actual.LossPercentage, Is.InRange(0.785, 0.8));

			Assert.That(actual.PokerHandPercentages[PokerHand.StraightFlush], Is.InRange(0, 0.0003));
			Assert.That(actual.PokerHandPercentages[PokerHand.FourOfAKind], Is.InRange(0, 0.002));
			Assert.That(actual.PokerHandPercentages[PokerHand.FullHouse], Is.InRange(0.019, 0.025));
			Assert.That(actual.PokerHandPercentages[PokerHand.Flush], Is.InRange(0.017, 0.02));
			Assert.That(actual.PokerHandPercentages[PokerHand.Straight], Is.InRange(0.0875, 0.0975));
			Assert.That(actual.PokerHandPercentages[PokerHand.ThreeOfAKind], Is.InRange(0.04, 0.046));
			Assert.That(actual.PokerHandPercentages[PokerHand.TwoPair], Is.InRange(0.22, 0.225));
			Assert.That(actual.PokerHandPercentages[PokerHand.Pair], Is.InRange(0.423, 0.428));
			Assert.That(actual.PokerHandPercentages[PokerHand.HighCard], Is.InRange(0.169, 0.173));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_7H_and_7C_in_hand_2D_KC_and_JS_on_board_and_two_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = new Card(CardValue.Seven, CardSuit.Hearts);
			var myHandCard2 = new Card(CardValue.Seven, CardSuit.Clubs);

			_deck.RemoveCard(myHandCard1);
			_deck.RemoveCard(myHandCard2);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = new Card(CardValue.Two, CardSuit.Diamonds);
			var boardHandCard2 = new Card(CardValue.King, CardSuit.Clubs);
			var boardHandCard3 = new Card(CardValue.Jack, CardSuit.Spades);

			_deck.RemoveCard(boardHandCard1);
			_deck.RemoveCard(boardHandCard2);
			_deck.RemoveCard(boardHandCard3);

			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3 });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 2, 100000);

			//output
			Assert.That(actual.WinPercentage, Is.InRange(0.385, 0.395));
			Assert.That(actual.DrawPercentage, Is.InRange(0.0010, 0.0015));
			Assert.That(actual.LossPercentage, Is.InRange(0.607, 0.611));

			Assert.That(actual.PokerHandPercentages[PokerHand.StraightFlush], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.FourOfAKind], Is.InRange(0.0009, 0.0013));
			Assert.That(actual.PokerHandPercentages[PokerHand.FullHouse], Is.InRange(0.022, 0.026));
			Assert.That(actual.PokerHandPercentages[PokerHand.Flush], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.Straight], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.ThreeOfAKind], Is.InRange(0.065, 0.067));
			Assert.That(actual.PokerHandPercentages[PokerHand.TwoPair], Is.InRange(0.3756, 0.3762));
			Assert.That(actual.PokerHandPercentages[PokerHand.Pair], Is.InRange(0.5305, 0.5355));
			Assert.That(actual.PokerHandPercentages[PokerHand.HighCard], Is.EqualTo(0));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_AS_and_9D_in_hand_9C_10S_JS_4C_on_board_and_five_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = new Card(CardValue.Ace, CardSuit.Spades);
			var myHandCard2 = new Card(CardValue.Nine, CardSuit.Diamonds);

			_deck.RemoveCard(myHandCard1);
			_deck.RemoveCard(myHandCard2);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = new Card(CardValue.Nine, CardSuit.Clubs);
			var boardHandCard2 = new Card(CardValue.Ten, CardSuit.Spades);
			var boardHandCard3 = new Card(CardValue.Jack, CardSuit.Spades);
			var boardHandCard4 = new Card(CardValue.Four, CardSuit.Clubs);

			_deck.RemoveCard(boardHandCard1);
			_deck.RemoveCard(boardHandCard2);
			_deck.RemoveCard(boardHandCard3);
			_deck.RemoveCard(boardHandCard4);

			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3, boardHandCard4 });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 5, 100000);

			//output
			Assert.That(actual.WinPercentage, Is.InRange(0.124, 0.128));
			Assert.That(actual.DrawPercentage, Is.InRange(0.0037, 0.0044));
			Assert.That(actual.LossPercentage, Is.InRange(0.867, 0.872));

			Assert.That(actual.PokerHandPercentages[PokerHand.StraightFlush], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.FourOfAKind], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.FullHouse], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.Flush], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.Straight], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.ThreeOfAKind], Is.InRange(0.0423, 0.0433));
			Assert.That(actual.PokerHandPercentages[PokerHand.TwoPair], Is.InRange(0.261, 0.265));
			Assert.That(actual.PokerHandPercentages[PokerHand.Pair], Is.InRange(0.692, 0.696));
			Assert.That(actual.PokerHandPercentages[PokerHand.HighCard], Is.EqualTo(0));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_AS_and_10D_in_hand_9C_10S_JS_KC_QC_on_board_and_three_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = new Card(CardValue.Ace, CardSuit.Spades);
			var myHandCard2 = new Card(CardValue.Ten, CardSuit.Diamonds);

			_deck.RemoveCard(myHandCard1);
			_deck.RemoveCard(myHandCard2);
			
			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = new Card(CardValue.Nine, CardSuit.Clubs);
			var boardHandCard2 = new Card(CardValue.Ten, CardSuit.Spades);
			var boardHandCard3 = new Card(CardValue.Jack, CardSuit.Spades);
			var boardHandCard4 = new Card(CardValue.King, CardSuit.Clubs);
			var boardHandCard5 = new Card(CardValue.Queen, CardSuit.Clubs);

			_deck.RemoveCard(boardHandCard1);
			_deck.RemoveCard(boardHandCard2);
			_deck.RemoveCard(boardHandCard3);
			_deck.RemoveCard(boardHandCard4);
			_deck.RemoveCard(boardHandCard5);

			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3, boardHandCard4, boardHandCard5 });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 3, 100000);

			//output
			Assert.That(actual.WinPercentage, Is.InRange(0.562, 0.569));
			Assert.That(actual.DrawPercentage, Is.InRange(0.298, 0.306));
			Assert.That(actual.LossPercentage, Is.InRange(0.130, 0.137));

			Assert.That(actual.PokerHandPercentages[PokerHand.StraightFlush], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.FourOfAKind], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.FullHouse], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.Flush], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.Straight], Is.EqualTo(1));
			Assert.That(actual.PokerHandPercentages[PokerHand.ThreeOfAKind], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.TwoPair], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.Pair], Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentages[PokerHand.HighCard], Is.EqualTo(0));
		}
	}
}
