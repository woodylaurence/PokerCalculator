using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared.TestObjects;
using System.Collections.Generic;
using System.Configuration;

namespace PokerCalculator.Tests.Integration.PokerCalculator
{
	public abstract class BasePokerCalculatorIntegrationTests : LocalTestBase
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
			_deck = new Deck(_randomNumberGenerator);
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));
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

			//assert
			Assert.That(actual.WinPercentageWithError.Error, Is.LessThanOrEqualTo(0.0025));
			Assert.That(actual.WinPercentageWithError.Percentage, Is.EqualTo(0.2360).Within(2 * actual.WinPercentageWithError.Error));

			Assert.That(actual.DrawPercentageWithError.Error, Is.LessThanOrEqualTo(0.0025));
			Assert.That(actual.DrawPercentageWithError.Percentage, Is.EqualTo(0.0310).Within(2 * actual.DrawPercentageWithError.Error));

			Assert.That(actual.LossPercentageWithError.Error, Is.LessThanOrEqualTo(0.0025));
			Assert.That(actual.LossPercentageWithError.Percentage, Is.EqualTo(0.7340).Within(2 * actual.LossPercentageWithError.Error));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error, Is.LessThanOrEqualTo(0.00004));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Percentage, Is.EqualTo(0.000015).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error, Is.LessThanOrEqualTo(0.00015));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Percentage, Is.EqualTo(0.00015).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error, Is.LessThanOrEqualTo(0.0005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Percentage, Is.EqualTo(0.0016).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Percentage, Is.EqualTo(0.026).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Percentage, Is.EqualTo(0.0302).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Percentage, Is.EqualTo(0.0457).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Percentage, Is.EqualTo(0.0483).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Percentage, Is.EqualTo(0.235).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Percentage, Is.EqualTo(0.438).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Percentage, Is.EqualTo(0.174).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_4D_and_5C_in_hand_no_cards_on_board_and_three_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = _deck.TakeCard(CardValue.Four, CardSuit.Diamonds);
			var myHandCard2 = _deck.TakeCard(CardValue.Five, CardSuit.Clubs);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });
			var boardHand = new Hand(new List<Card>());

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 3, 100000);

			//assert
			Assert.That(actual.WinPercentageWithError.Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.WinPercentageWithError.Percentage, Is.EqualTo(0.176).Within(2 * actual.WinPercentageWithError.Error));

			Assert.That(actual.DrawPercentageWithError.Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.DrawPercentageWithError.Percentage, Is.EqualTo(0.031).Within(2 * actual.DrawPercentageWithError.Error));

			Assert.That(actual.LossPercentageWithError.Error, Is.LessThanOrEqualTo(0.0035));
			Assert.That(actual.LossPercentageWithError.Percentage, Is.EqualTo(0.7925).Within(2 * actual.LossPercentageWithError.Error));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error, Is.LessThanOrEqualTo(0.00004));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Percentage, Is.EqualTo(0.000015).Within(0.00008));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error, Is.LessThanOrEqualTo(0.00015));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Percentage, Is.EqualTo(0.00015).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error, Is.LessThanOrEqualTo(0.0005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Percentage, Is.EqualTo(0.0015).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Percentage, Is.EqualTo(0.022).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Percentage, Is.EqualTo(0.0185).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Percentage, Is.EqualTo(0.0925).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Percentage, Is.EqualTo(0.043).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Percentage, Is.EqualTo(0.2225).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Percentage, Is.EqualTo(0.4255).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Percentage, Is.EqualTo(0.172).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_7H_and_7C_in_hand_2D_KC_and_JS_on_board_and_two_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = _deck.TakeCard(CardValue.Seven, CardSuit.Hearts);
			var myHandCard2 = _deck.TakeCard(CardValue.Seven, CardSuit.Clubs);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = _deck.TakeCard(CardValue.Two, CardSuit.Diamonds);
			var boardHandCard2 = _deck.TakeCard(CardValue.King, CardSuit.Clubs);
			var boardHandCard3 = _deck.TakeCard(CardValue.Jack, CardSuit.Spades);

			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3 });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 2, 100000);

			//assert
			Assert.That(actual.WinPercentageWithError.Error, Is.LessThanOrEqualTo(0.006));
			Assert.That(actual.WinPercentageWithError.Percentage, Is.EqualTo(0.39).Within(2 * actual.WinPercentageWithError.Error));

			Assert.That(actual.DrawPercentageWithError.Error, Is.LessThanOrEqualTo(0.001));
			Assert.That(actual.DrawPercentageWithError.Percentage, Is.EqualTo(0.001).Within(2 * actual.DrawPercentageWithError.Error));

			Assert.That(actual.LossPercentageWithError.Error, Is.LessThanOrEqualTo(0.006));
			Assert.That(actual.LossPercentageWithError.Percentage, Is.EqualTo(0.608).Within(2 * actual.LossPercentageWithError.Error));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error, Is.LessThanOrEqualTo(0.0005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Percentage, Is.EqualTo(0.001).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error, Is.LessThanOrEqualTo(0.002));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Percentage, Is.EqualTo(0.024).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Percentage, Is.EqualTo(0.066).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Percentage, Is.EqualTo(0.375).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Percentage, Is.EqualTo(0.5325).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Percentage, Is.EqualTo(0));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_AS_and_9D_in_hand_9C_10S_JS_4C_on_board_and_five_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = _deck.TakeCard(CardValue.Ace, CardSuit.Spades);
			var myHandCard2 = _deck.TakeCard(CardValue.Nine, CardSuit.Diamonds);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = _deck.TakeCard(CardValue.Nine, CardSuit.Clubs);
			var boardHandCard2 = _deck.TakeCard(CardValue.Ten, CardSuit.Spades);
			var boardHandCard3 = _deck.TakeCard(CardValue.Jack, CardSuit.Spades);
			var boardHandCard4 = _deck.TakeCard(CardValue.Four, CardSuit.Clubs);

			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3, boardHandCard4 });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 5, 100000);

			//assert
			Assert.That(actual.WinPercentageWithError.Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.WinPercentageWithError.Percentage, Is.EqualTo(0.127).Within(2 * actual.WinPercentageWithError.Error));

			Assert.That(actual.DrawPercentageWithError.Error, Is.LessThanOrEqualTo(0.001));
			Assert.That(actual.DrawPercentageWithError.Percentage, Is.EqualTo(0.004).Within(2 * actual.DrawPercentageWithError.Error));

			Assert.That(actual.LossPercentageWithError.Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.LossPercentageWithError.Percentage, Is.EqualTo(0.869).Within(2 * actual.LossPercentageWithError.Error));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Percentage, Is.EqualTo(0.0429).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Percentage, Is.EqualTo(0.266).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Percentage, Is.EqualTo(0.691).Within(2 * actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Percentage, Is.EqualTo(0));
		}

		[Test]
		public void CalculatePokerOdds_WHERE_AS_and_10D_in_hand_9C_10S_JS_KC_QC_on_board_and_three_opponents_SHOULD_calculate_winning_odds_and_hand_odds_accurately()
		{
			//arange
			var myHandCard1 = _deck.TakeCard(CardValue.Ace, CardSuit.Spades);
			var myHandCard2 = _deck.TakeCard(CardValue.Ten, CardSuit.Diamonds);

			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = _deck.TakeCard(CardValue.Nine, CardSuit.Clubs);
			var boardHandCard2 = _deck.TakeCard(CardValue.Ten, CardSuit.Spades);
			var boardHandCard3 = _deck.TakeCard(CardValue.Jack, CardSuit.Spades);
			var boardHandCard4 = _deck.TakeCard(CardValue.King, CardSuit.Clubs);
			var boardHandCard5 = _deck.TakeCard(CardValue.Queen, CardSuit.Clubs);

			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3, boardHandCard4, boardHandCard5 });

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, 3, 100000);

			//assert
			Assert.That(actual.WinPercentageWithError.Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.WinPercentageWithError.Percentage, Is.EqualTo(0.565).Within(2 * actual.WinPercentageWithError.Error));

			Assert.That(actual.DrawPercentageWithError.Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.DrawPercentageWithError.Percentage, Is.EqualTo(0.303).Within(2 * actual.DrawPercentageWithError.Error));

			Assert.That(actual.LossPercentageWithError.Error, Is.LessThanOrEqualTo(0.003));
			Assert.That(actual.LossPercentageWithError.Percentage, Is.EqualTo(0.133).Within(2 * actual.LossPercentageWithError.Error));

			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.RoyalFlush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.StraightFlush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FourOfAKind].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.FullHouse].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Flush].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Straight].Percentage, Is.EqualTo(1));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.ThreeOfAKind].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.TwoPair].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Error, Is.LessThanOrEqualTo(0.005));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.Pair].Percentage, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Error, Is.EqualTo(0));
			Assert.That(actual.PokerHandPercentagesWithErrors[PokerHand.HighCard].Percentage, Is.EqualTo(0));
		}
	}
}
