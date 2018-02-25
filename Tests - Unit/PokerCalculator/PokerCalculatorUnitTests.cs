using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Configuration;
using Card = PokerCalculator.Domain.PokerObjects.Card;

namespace PokerCalculator.Tests.Unit.PokerCalculator
{
	[TestFixture]
	public class PokerCalculatorUnitTests : AbstractUnitTestBase
	{
		private Domain.PokerCalculator.PokerCalculator _instance;
		private Deck _deck;
		private IHandRankCalculator _handRankCalculator;
		private IRandomNumberGenerator _randomNumberGenerator;
		private IUtilitiesService _utilitiesService;

		[SetUp]
		protected override void Setup()
		{
			_utilitiesService = new UtilitiesService();
			_randomNumberGenerator = MockRepository.GenerateStrictMock<IRandomNumberGenerator>();
			_handRankCalculator = MockRepository.GenerateStrictMock<IHandRankCalculator>();

			base.Setup();

			_instance = MockRepository.GeneratePartialMock<Domain.PokerCalculator.PokerCalculator>(_handRankCalculator);
			_deck = MockRepository.GenerateStrictMock<Deck>(_randomNumberGenerator, _utilitiesService);
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IUtilitiesService>().Instance(_utilitiesService));
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));
			windsorContainer.Register(Component.For<IEqualityComparer<Card>>().Instance(new CardComparer()));
			windsorContainer.Register(Component.For<IHandRankCalculator>().Instance(_handRankCalculator));
		}

		#region Properties and Fields

		[Test]
		public void NumIterationsToCalculate()
		{
			//arrange
			var expected = int.Parse(ConfigurationManager.AppSettings["PokerCalculator.Domain.PokerCalculator.NumIterationsToCalculate"]);

			//act
			var actual = _instance._numIterationsToCalculate;

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region Instance Methods

		#region CalculatePokerOdds

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CalculatePokerOdds_WHERE_single_iteration_and_my_hand_loses_against_opponent_SHOULD_record_loss_in_odds(PokerHand pokerHand)
		{
			//arrange
			const int numOpponents = 2;
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			ConfigurationManager.AppSettings["PokerCalculator.Domain.PokerCalculator.NumIterationsToCalculate"] = "1";

			var clonedDeck = MockRepository.GenerateStrictMock<Deck>();
			_deck.Stub(x => x.Clone()).Return(clonedDeck);

			var clonedMyHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			myHand.Stub(x => x.Clone()).Return(clonedMyHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedMyHand, clonedDeck, 2));

			var clonedBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			boardHand.Stub(x => x.Clone()).Return(clonedBoardHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedBoardHand, clonedDeck, 5));

			var combinedMyAndBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			clonedMyHand.Stub(x => x.Operator_plus(clonedBoardHand)).Return(combinedMyAndBoardHand);

			var opponentHands = new List<Hand>
			{
				MockRepository.GenerateStrictMock<Hand>(new List<Card>()),
				MockRepository.GenerateStrictMock<Hand>(new List<Card>())
			};
			_instance.Stub(x => x.CreateOpponentHands(clonedDeck, clonedBoardHand, numOpponents)).Return(opponentHands);

			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.GetBestOpponentHand(opponentHands)).Return(bestOpponentHand);

			var myHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			combinedMyAndBoardHand.Stub(x => x.Rank).Return(myHandRank);

			myHandRank.Stub(x => x.PokerHand).Return(pokerHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			bestOpponentHand.Stub(x => x.Rank).Return(bestOpponentHandRank);

			myHandRank.Stub(x => x.Operator_LessThan(bestOpponentHandRank)).Return(true);

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, numOpponents);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual.NumWins, Is.EqualTo(0));
			Assert.That(actual.NumDraws, Is.EqualTo(0));
			Assert.That(actual.NumLosses, Is.EqualTo(1));

			foreach (var pokerHandFrequency in actual.PokerHandFrequencies)
			{
				var expectedFrequency = pokerHandFrequency.Key == pokerHand ? 1 : 0;
				Assert.That(pokerHandFrequency.Value, Is.EqualTo(expectedFrequency));
			}
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CalculatePokerOdds_WHERE_single_iteration_and_my_hand_wins_against_opponent_SHOULD_record_win_in_odds(PokerHand pokerHand)
		{
			//arrange
			const int numOpponents = 2;
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			ConfigurationManager.AppSettings["PokerCalculator.Domain.PokerCalculator.NumIterationsToCalculate"] = "1";

			var clonedDeck = MockRepository.GenerateStrictMock<Deck>();
			_deck.Stub(x => x.Clone()).Return(clonedDeck);

			var clonedMyHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			myHand.Stub(x => x.Clone()).Return(clonedMyHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedMyHand, clonedDeck, 2));

			var clonedBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			boardHand.Stub(x => x.Clone()).Return(clonedBoardHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedBoardHand, clonedDeck, 5));

			var combinedMyAndBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			clonedMyHand.Stub(x => x.Operator_plus(clonedBoardHand)).Return(combinedMyAndBoardHand);

			var opponentHands = new List<Hand>
			{
				MockRepository.GenerateStrictMock<Hand>(new List<Card>()),
				MockRepository.GenerateStrictMock<Hand>(new List<Card>())
			};
			_instance.Stub(x => x.CreateOpponentHands(clonedDeck, clonedBoardHand, numOpponents)).Return(opponentHands);

			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.GetBestOpponentHand(opponentHands)).Return(bestOpponentHand);

			var myHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			combinedMyAndBoardHand.Stub(x => x.Rank).Return(myHandRank);

			myHandRank.Stub(x => x.PokerHand).Return(pokerHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			bestOpponentHand.Stub(x => x.Rank).Return(bestOpponentHandRank);

			myHandRank.Stub(x => x.Operator_LessThan(bestOpponentHandRank)).Return(false);
			myHandRank.Stub(x => x.Operator_GreaterThan(bestOpponentHandRank)).Return(true);

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, numOpponents);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual.NumWins, Is.EqualTo(1));
			Assert.That(actual.NumDraws, Is.EqualTo(0));
			Assert.That(actual.NumLosses, Is.EqualTo(0));

			foreach (var pokerHandFrequency in actual.PokerHandFrequencies)
			{
				var expectedFrequency = pokerHandFrequency.Key == pokerHand ? 1 : 0;
				Assert.That(pokerHandFrequency.Value, Is.EqualTo(expectedFrequency));
			}
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CalculatePokerOdds_WHERE_single_iteration_and_my_hand_draws_against_opponent_SHOULD_record_draw_in_odds(PokerHand pokerHand)
		{
			//arrange
			const int numOpponents = 2;
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			ConfigurationManager.AppSettings["PokerCalculator.Domain.PokerCalculator.NumIterationsToCalculate"] = "1";

			var clonedDeck = MockRepository.GenerateStrictMock<Deck>();
			_deck.Stub(x => x.Clone()).Return(clonedDeck);

			var clonedMyHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			myHand.Stub(x => x.Clone()).Return(clonedMyHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedMyHand, clonedDeck, 2));

			var clonedBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			boardHand.Stub(x => x.Clone()).Return(clonedBoardHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedBoardHand, clonedDeck, 5));

			var combinedMyAndBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			clonedMyHand.Stub(x => x.Operator_plus(clonedBoardHand)).Return(combinedMyAndBoardHand);

			var opponentHands = new List<Hand>
			{
				MockRepository.GenerateStrictMock<Hand>(new List<Card>()),
				MockRepository.GenerateStrictMock<Hand>(new List<Card>())
			};
			_instance.Stub(x => x.CreateOpponentHands(clonedDeck, clonedBoardHand, numOpponents)).Return(opponentHands);

			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.GetBestOpponentHand(opponentHands)).Return(bestOpponentHand);

			var myHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			combinedMyAndBoardHand.Stub(x => x.Rank).Return(myHandRank);

			myHandRank.Stub(x => x.PokerHand).Return(pokerHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			bestOpponentHand.Stub(x => x.Rank).Return(bestOpponentHandRank);

			myHandRank.Stub(x => x.Operator_LessThan(bestOpponentHandRank)).Return(false);
			myHandRank.Stub(x => x.Operator_GreaterThan(bestOpponentHandRank)).Return(false);

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, numOpponents);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual.NumWins, Is.EqualTo(0));
			Assert.That(actual.NumDraws, Is.EqualTo(1));
			Assert.That(actual.NumLosses, Is.EqualTo(0));

			foreach (var pokerHandFrequency in actual.PokerHandFrequencies)
			{
				var expectedFrequency = pokerHandFrequency.Key == pokerHand ? 1 : 0;
				Assert.That(pokerHandFrequency.Value, Is.EqualTo(expectedFrequency));
			}
		}

		[Test]
		public void CalculatePokerOdds_WHERE_multiple_iteration_and_my_hand_draws_against_opponent_SHOULD_record_draw_in_odds(PokerHand pokerHand)
		{
			//arrange
			Assert.Fail("write this test");
			const int numOpponents = 2;
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			ConfigurationManager.AppSettings["PokerCalculator.Domain.NumIterationsToCalculate"] = "2";

			var clonedDeck = MockRepository.GenerateStrictMock<Deck>();
			_deck.Stub(x => x.Clone()).Return(clonedDeck);

			var clonedMyHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			myHand.Stub(x => x.Clone()).Return(clonedMyHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedMyHand, clonedDeck, 2));

			var clonedBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			boardHand.Stub(x => x.Clone()).Return(clonedBoardHand);
			_instance.Expect(x => x.DealRequiredNumberOfCardsToHand(clonedBoardHand, clonedDeck, 5));

			var combinedMyAndBoardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			clonedMyHand.Stub(x => x.Operator_plus(clonedBoardHand)).Return(combinedMyAndBoardHand);

			var opponentHands = new List<Hand>
			{
				MockRepository.GenerateStrictMock<Hand>(new List<Card>()),
				MockRepository.GenerateStrictMock<Hand>(new List<Card>())
			};
			_instance.Stub(x => x.CreateOpponentHands(clonedDeck, clonedBoardHand, numOpponents)).Return(opponentHands);

			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.GetBestOpponentHand(opponentHands)).Return(bestOpponentHand);

			var myHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			combinedMyAndBoardHand.Stub(x => x.Rank).Return(myHandRank);

			myHandRank.Stub(x => x.PokerHand).Return(pokerHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			bestOpponentHand.Stub(x => x.Rank).Return(bestOpponentHandRank);

			myHandRank.Stub(x => x.Operator_LessThan(bestOpponentHandRank)).Return(false);
			myHandRank.Stub(x => x.Operator_GreaterThan(bestOpponentHandRank)).Return(false);

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, numOpponents);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual.NumWins, Is.EqualTo(0));
			Assert.That(actual.NumDraws, Is.EqualTo(1));
			Assert.That(actual.NumLosses, Is.EqualTo(0));

			foreach (var pokerHandFrequency in actual.PokerHandFrequencies)
			{
				var expectedFrequency = pokerHandFrequency.Key == pokerHand ? 1 : 0;
				Assert.That(pokerHandFrequency.Value, Is.EqualTo(expectedFrequency));
			}
		}

		#region DealRequiredNumberOfCardsToHand

		[Test]
		public void DealRequiredNumberOfCardsToHand_WHERE_hand_contains_more_than_required_number_of_cards_SHOULD_do_nothing()
		{
			//arrange
			var hand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			hand.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Hearts),
				new Card(CardValue.Ace, CardSuit.Diamonds),
				new Card(CardValue.Four, CardSuit.Spades),
				new Card(CardValue.Seven, CardSuit.Clubs)
			});

			//act
			_instance.DealRequiredNumberOfCardsToHand(hand, _deck, 3);

			//assert
			_deck.AssertWasNotCalled(x => x.TakeRandomCards(Arg<int>.Is.Anything));
			hand.AssertWasNotCalled(x => x.AddCards(Arg<List<Card>>.Is.Anything));
		}

		[Test]
		public void DealRequiredNumberOfCardsToHand_WHERE_hand_contains_exactly_the_required_number_of_cards_SHOULD_do_nothing()
		{
			//arrange
			var hand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			hand.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Hearts),
				new Card(CardValue.Ace, CardSuit.Diamonds),
				new Card(CardValue.Four, CardSuit.Spades)
			});

			//act
			_instance.DealRequiredNumberOfCardsToHand(hand, _deck, 3);

			//assert
			_deck.AssertWasNotCalled(x => x.TakeRandomCards(Arg<int>.Is.Anything));
			hand.AssertWasNotCalled(x => x.AddCards(Arg<List<Card>>.Is.Anything));
		}

		[Test]
		public void DealRequiredNumberOfCardsToHand_WHERE_hand_contains_less_than_the_required_number_of_cards_SHOULD_deal_required_number_of_cards()
		{
			//arrange
			var hand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			hand.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Hearts)
			});

			var cardsToAddToHand = new List<Card> { new Card(CardValue.King, CardSuit.Spades) };
			_deck.Stub(x => x.TakeRandomCards(2)).Return(cardsToAddToHand);

			hand.Expect(x => x.AddCards(cardsToAddToHand));

			//act
			_instance.DealRequiredNumberOfCardsToHand(hand, _deck, 3);

			//assert
			hand.VerifyAllExpectations();
		}

		#endregion

		#region CreateOpponentHands

		[Test]
		public void CreateOpponentHands_WHERE_zero_opponents_SHOULD_return_empty_list()
		{
			//arrange
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			//act
			var actual = _instance.CreateOpponentHands(_deck, boardHand, 0);

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void CreateOpponentHands()
		{
			//arrange
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			var opponent1StartingHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.ConstructTwoCardOpponentHand(_deck)).Return(opponent1StartingHand).Repeat.Once();

			var opponent1FullHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			opponent1StartingHand.Stub(x => x.Operator_plus(boardHand)).Return(opponent1FullHand);

			var opponent2StartingHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.ConstructTwoCardOpponentHand(_deck)).Return(opponent2StartingHand).Repeat.Once();

			var opponent2FullHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			opponent2StartingHand.Stub(x => x.Operator_plus(boardHand)).Return(opponent2FullHand);

			var opponent3StartingHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.ConstructTwoCardOpponentHand(_deck)).Return(opponent3StartingHand).Repeat.Once();

			var opponent3FullHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			opponent3StartingHand.Stub(x => x.Operator_plus(boardHand)).Return(opponent3FullHand);

			//act
			var actual = _instance.CreateOpponentHands(_deck, boardHand, 3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.Some.EqualTo(opponent1FullHand));
			Assert.That(actual, Has.Some.EqualTo(opponent2FullHand));
			Assert.That(actual, Has.Some.EqualTo(opponent3FullHand));
		}

		#region ConstructorTwoCardOpponentHand

		[Test]
		public void ConstructTwoCardOpponentHand()
		{
			//arrange
			var card1 = new Card(CardValue.Eight, CardSuit.Diamonds);
			var card2 = new Card(CardValue.Four, CardSuit.Hearts);
			_deck.Stub(x => x.TakeRandomCards(2)).Return(new List<Card> { card1, card2 });

			//act
			var actual = _instance.ConstructTwoCardOpponentHand(_deck);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(2));
			Assert.That(actual.Cards[0], Is.EqualTo(card1));
			Assert.That(actual.Cards[1], Is.EqualTo(card2));
		}

		#endregion

		#endregion

		#region GetBestOpponentHand

		[Test]
		public void GetBestOpponentHand_WHERE_no_opponents_SHOULD_return_null()
		{
			//act
			var actual = _instance.GetBestOpponentHand(new List<Hand>());

			//assert
			Assert.That(actual, Is.Null);
		}

		[Test]
		public void GetBestOpponentHand_WHERE_single_opponent_SHOULD_return_opponents_hand()
		{
			//arrange
			var opponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			//act
			var actual = _instance.GetBestOpponentHand(new List<Hand> { opponentHand });

			//assert
			Assert.That(actual, Is.EqualTo(opponentHand));
		}

		[Test]
		public void GetBestOpponentHand_WHERE_multiple_opponents_SHOULD_return_best_opponents_hand()
		{
			//arrange
			var opponentHand1 = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var opponentHand2 = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var opponentHand3 = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			var opponentHand1Rank = MockRepository.GeneratePartialMock<HandRank>(PokerHand.ThreeOfAKind, null);
			opponentHand1.Stub(x => x.Rank).Return(opponentHand1Rank);

			var opponentHand2Rank = MockRepository.GeneratePartialMock<HandRank>(PokerHand.Straight, null);
			opponentHand2.Stub(x => x.Rank).Return(opponentHand2Rank);

			var opponentHand3Rank = MockRepository.GeneratePartialMock<HandRank>(PokerHand.HighCard, null);
			opponentHand3.Stub(x => x.Rank).Return(opponentHand3Rank);

			opponentHand1Rank.Stub(x => x.CompareTo(opponentHand2Rank)).Return(-1);
			opponentHand2Rank.Stub(x => x.CompareTo(opponentHand3Rank)).Return(1);
			opponentHand1Rank.Stub(x => x.CompareTo(opponentHand3Rank)).Return(1);
			opponentHand3Rank.Stub(x => x.CompareTo(opponentHand1Rank)).Return(-1);
			opponentHand2Rank.Stub(x => x.CompareTo(opponentHand1Rank)).Return(1);
			opponentHand3Rank.Stub(x => x.CompareTo(opponentHand2Rank)).Return(-1);

			//act
			var actual = _instance.GetBestOpponentHand(new List<Hand> { opponentHand1, opponentHand2, opponentHand3 });

			//assert
			Assert.That(actual, Is.EqualTo(opponentHand2));
		}

		#endregion

		#endregion

		#endregion
	}
}
