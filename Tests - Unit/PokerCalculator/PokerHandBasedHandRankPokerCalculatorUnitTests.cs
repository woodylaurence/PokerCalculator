using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System.Collections.Generic;
using Card = PokerCalculator.Domain.PokerObjects.Card;

namespace PokerCalculator.Tests.Unit.PokerCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankPokerCalculatorUnitTests : AbstractUnitTestBase
	{
		private PokerHandBasedHandRankPokerCalculator _instance;
		private Deck _deck;
		private IHandRankCalculator<PokerHandBasedHandRank, PokerHand> _handRankCalculator;
		private IRandomNumberGenerator _randomNumberGenerator;
		private IUtilitiesService _utilitiesService;

		[SetUp]
		protected override void Setup()
		{
			_utilitiesService = new UtilitiesService();
			_randomNumberGenerator = MockRepository.GenerateStrictMock<IRandomNumberGenerator>();
			_handRankCalculator = MockRepository.GenerateStrictMock<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>();

			base.Setup();

			_instance = MockRepository.GeneratePartialMock<PokerHandBasedHandRankPokerCalculator>(_handRankCalculator);
			_deck = MockRepository.GenerateStrictMock<Deck>(_randomNumberGenerator, _utilitiesService);

			PokerOdds.MethodObject = MockRepository.GenerateStrictMock<PokerOdds>(_utilitiesService);
			IComparableExtensionMethods.MethodObject = MockRepository.GenerateStrictMock<IComparableExtensionMethodsConcreteObject>();
		}

		[TearDown]
		protected void TearDown()
		{
			PokerOdds.MethodObject = new PokerOdds(_utilitiesService);
			IComparableExtensionMethods.MethodObject = new IComparableExtensionMethodsConcreteObject();
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IUtilitiesService>().Instance(_utilitiesService));
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));
			windsorContainer.Register(Component.For<IEqualityComparer<Card>>().Instance(new CardComparer()));
			windsorContainer.Register(Component.For<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>().Instance(_handRankCalculator));
		}

		#region Instance Methods

		#region CalculatePokerOdds

		[Test]
		public void CalculatePokerOdds_WHERE_multiple_iterations_SHOULD_call_ExectutePokerCalculateIteration_multiple_times()
		{
			//arrange
			const int numOpponents = 156;
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			var pokerOdds1 = MockRepository.GenerateStub<PokerOdds>(_utilitiesService);
			_instance.Stub(x => x.InitializePokerOdds()).Return(pokerOdds1).Repeat.Once();
			_instance.Expect(x => x.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds1)).Repeat.Times(5);

			var pokerOdds2 = MockRepository.GenerateStub<PokerOdds>(_utilitiesService);
			_instance.Stub(x => x.InitializePokerOdds()).Return(pokerOdds2).Repeat.Once();
			_instance.Expect(x => x.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds2)).Repeat.Times(5);

			var pokerOdds3 = MockRepository.GenerateStub<PokerOdds>(_utilitiesService);
			_instance.Stub(x => x.InitializePokerOdds()).Return(pokerOdds3).Repeat.Once();
			_instance.Expect(x => x.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds3)).Repeat.Times(5);

			var pokerOdds4 = MockRepository.GenerateStub<PokerOdds>(_utilitiesService);
			_instance.Stub(x => x.InitializePokerOdds()).Return(pokerOdds4).Repeat.Once();
			_instance.Expect(x => x.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds4)).Repeat.Times(5);

			var pokerOdds5 = MockRepository.GenerateStub<PokerOdds>(_utilitiesService);
			_instance.Stub(x => x.InitializePokerOdds()).Return(pokerOdds5).Repeat.Once();
			_instance.Expect(x => x.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds5)).Repeat.Times(5);

			var aggregatedPokerOdds = MockRepository.GenerateStrictMock<PokerOdds>(_utilitiesService);
			PokerOdds.MethodObject.Stub(x =>
					x.AggregatePokerOddsSlave(Arg<List<PokerOdds>>.Matches(y => y.Count == 5 &&
																				y.Contains(pokerOdds1) &&
																				y.Contains(pokerOdds2) &&
																				y.Contains(pokerOdds3) &&
																				y.Contains(pokerOdds4) &&
																				y.Contains(pokerOdds5))))
				.Return(aggregatedPokerOdds);

			//act
			var actual = _instance.CalculatePokerOdds(_deck, myHand, boardHand, numOpponents, 25);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(aggregatedPokerOdds));
		}

		#region ExecuteCalculatePokerOddsForIteration

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_WHERE_zero_opponents()
		{
			//arrange
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var pokerOdds = MockRepository.GenerateStrictMock<PokerOdds>(_utilitiesService);

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

			var myHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(combinedMyAndBoardHand)).Return(myHandRank);

			_instance.Stub(x => x.SimulateOpponentHandsAndReturnBestHand(clonedDeck, clonedBoardHand, 0)).Return(null);

			IComparableExtensionMethods.MethodObject.Stub(x => x.IsLessThanSlave(myHandRank, null)).Return(false);
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsGreaterThanSlave(myHandRank, null)).Return(true);

			const int initialNumWins = 4;
			pokerOdds.Stub(x => x.NumWins).Return(initialNumWins);
			pokerOdds.Expect(x => x.NumWins = initialNumWins + 1);

			const PokerHand myHandRankPokerHand = PokerHand.ThreeOfAKind;
			myHandRank.Stub(x => x.PokerHand).Return(myHandRankPokerHand);

			const int initialPokerHandValud = 54;
			var pokerHandFrequencies = new Dictionary<PokerHand, int>
			{
				{ PokerHand.RoyalFlush, 0 },
				{ myHandRankPokerHand, initialPokerHandValud }
			};
			pokerOdds.Stub(x => x.PokerHandFrequencies).Return(pokerHandFrequencies);

			//act
			_instance.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, 0, pokerOdds);

			//assert
			pokerOdds.VerifyAllExpectations();
			Assert.That(pokerHandFrequencies[PokerHand.RoyalFlush], Is.EqualTo(0));
			Assert.That(pokerHandFrequencies[myHandRankPokerHand], Is.EqualTo(initialPokerHandValud + 1));
		}

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_WHERE_my_hand_loses_against_opponent_SHOULD_record_loss_in_odds()
		{
			//arrange
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var pokerOdds = MockRepository.GenerateStrictMock<PokerOdds>(_utilitiesService);

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

			var myHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(combinedMyAndBoardHand)).Return(myHandRank);

			const int numOpponents = 3;
			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.SimulateOpponentHandsAndReturnBestHand(clonedDeck, clonedBoardHand, numOpponents)).Return(bestOpponentHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(bestOpponentHand)).Return(bestOpponentHandRank);

			IComparableExtensionMethods.MethodObject.Stub(x => x.IsLessThanSlave(myHandRank, bestOpponentHandRank)).Return(true);

			const int initialNumLosses = 545;
			pokerOdds.Stub(x => x.NumLosses).Return(initialNumLosses);
			pokerOdds.Expect(x => x.NumLosses = initialNumLosses + 1);

			const PokerHand myHandRankPokerHand = PokerHand.FourOfAKind;
			myHandRank.Stub(x => x.PokerHand).Return(myHandRankPokerHand);

			const int initialPokerHandValud = 54;
			var pokerHandFrequencies = new Dictionary<PokerHand, int>
			{
				{ PokerHand.Pair, 0 },
				{ myHandRankPokerHand, initialPokerHandValud }
			};
			pokerOdds.Stub(x => x.PokerHandFrequencies).Return(pokerHandFrequencies);

			//act
			_instance.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds);

			//assert
			pokerOdds.VerifyAllExpectations();
			Assert.That(pokerHandFrequencies[PokerHand.Pair], Is.EqualTo(0));
			Assert.That(pokerHandFrequencies[myHandRankPokerHand], Is.EqualTo(initialPokerHandValud + 1));
		}

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_WHERE_my_hand_draws_against_opponent_SHOULD_record_draw_in_odds()
		{
			//arrange
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var pokerOdds = MockRepository.GenerateStrictMock<PokerOdds>(_utilitiesService);

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

			var myHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(combinedMyAndBoardHand)).Return(myHandRank);

			const int numOpponents = 6;
			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.SimulateOpponentHandsAndReturnBestHand(clonedDeck, clonedBoardHand, numOpponents)).Return(bestOpponentHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(bestOpponentHand)).Return(bestOpponentHandRank);

			IComparableExtensionMethods.MethodObject.Stub(x => x.IsLessThanSlave(myHandRank, bestOpponentHandRank)).Return(false);
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsGreaterThanSlave(myHandRank, bestOpponentHandRank)).Return(false);

			const int initialNumDraws = 41;
			pokerOdds.Stub(x => x.NumDraws).Return(initialNumDraws);
			pokerOdds.Expect(x => x.NumDraws = initialNumDraws + 1);

			const PokerHand myHandRankPokerHand = PokerHand.Pair;
			myHandRank.Stub(x => x.PokerHand).Return(myHandRankPokerHand);

			const int initialPokerHandValud = 54;
			var pokerHandFrequencies = new Dictionary<PokerHand, int>
			{
				{ PokerHand.ThreeOfAKind, 0 },
				{ myHandRankPokerHand, initialPokerHandValud }
			};
			pokerOdds.Stub(x => x.PokerHandFrequencies).Return(pokerHandFrequencies);

			//act
			_instance.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds);

			//assert
			pokerOdds.VerifyAllExpectations();
			Assert.That(pokerHandFrequencies[PokerHand.ThreeOfAKind], Is.EqualTo(0));
			Assert.That(pokerHandFrequencies[myHandRankPokerHand], Is.EqualTo(initialPokerHandValud + 1));
		}

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_WHERE_my_hand_wins_against_opponent_SHOULD_record_draw_in_odds()
		{
			//arrange
			var myHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var boardHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var pokerOdds = MockRepository.GenerateStrictMock<PokerOdds>(_utilitiesService);

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

			var myHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(combinedMyAndBoardHand)).Return(myHandRank);

			const int numOpponents = 6;
			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.SimulateOpponentHandsAndReturnBestHand(clonedDeck, clonedBoardHand, numOpponents)).Return(bestOpponentHand);

			var bestOpponentHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(bestOpponentHand)).Return(bestOpponentHandRank);

			IComparableExtensionMethods.MethodObject.Stub(x => x.IsLessThanSlave(myHandRank, bestOpponentHandRank)).Return(false);
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsGreaterThanSlave(myHandRank, bestOpponentHandRank)).Return(true);

			const int initialNumWins = 51;
			pokerOdds.Stub(x => x.NumWins).Return(initialNumWins);
			pokerOdds.Expect(x => x.NumWins = initialNumWins + 1);

			const PokerHand myHandRankPokerHand = PokerHand.StraightFlush;
			myHandRank.Stub(x => x.PokerHand).Return(myHandRankPokerHand);

			const int initialPokerHandValud = 54;
			var pokerHandFrequencies = new Dictionary<PokerHand, int>
			{
				{ PokerHand.Straight, 0 },
				{ myHandRankPokerHand, initialPokerHandValud }
			};
			pokerOdds.Stub(x => x.PokerHandFrequencies).Return(pokerHandFrequencies);

			//act
			_instance.ExecuteCalculatePokerOddsForIteration(_deck, myHand, boardHand, numOpponents, pokerOdds);

			//assert
			pokerOdds.VerifyAllExpectations();
			Assert.That(pokerHandFrequencies[PokerHand.Straight], Is.EqualTo(0));
			Assert.That(pokerHandFrequencies[myHandRankPokerHand], Is.EqualTo(initialPokerHandValud + 1));
		}

		#endregion

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

		#region SimulateOpponentHandsAndReturnBestHand

		[Test]
		public void SimulateOpponentHandsAndReturnBestHand()
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

			var bestOpponentHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x =>
					x.GetBestOpponentHand(Arg<List<Hand>>.Matches(y => y.Count == 3 &&
																	   y.Contains(opponent1FullHand) &&
																	   y.Contains(opponent2FullHand) &&
																	   y.Contains(opponent3FullHand))))
				.Return(bestOpponentHand);

			//act
			var actual = _instance.SimulateOpponentHandsAndReturnBestHand(_deck, boardHand, 3);

			//assert
			Assert.That(actual, Is.EqualTo(bestOpponentHand));
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

			var opponentHand1Rank = MockRepository.GeneratePartialMock<PokerHandBasedHandRank>(PokerHand.ThreeOfAKind, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(opponentHand1)).Return(opponentHand1Rank);

			var opponentHand2Rank = MockRepository.GeneratePartialMock<PokerHandBasedHandRank>(PokerHand.Straight, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(opponentHand2)).Return(opponentHand2Rank);

			var opponentHand3Rank = MockRepository.GeneratePartialMock<PokerHandBasedHandRank>(PokerHand.HighCard, null);
			_handRankCalculator.Stub(x => x.CalculateHandRank(opponentHand3)).Return(opponentHand3Rank);

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

		#endregion
	}
}
