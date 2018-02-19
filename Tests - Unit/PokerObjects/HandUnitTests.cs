using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class HandUnitTests : AbstractUnitTestBase
	{
		private Hand _instance;
		private IEqualityComparer<Card> _cardComparer;
		private IHandRankCalculator _handRankCalculator;

		[SetUp]
		public override void Setup()
		{
			base.Setup();

			_cardComparer = MockRepository.GenerateStrictMock<IEqualityComparer<Card>>();
			_handRankCalculator = MockRepository.GenerateStrictMock<IHandRankCalculator>();

			_instance = MockRepository.GeneratePartialMock<Hand>(new List<Card>(), _cardComparer, _handRankCalculator);
		}

		#region Properties and Fields

		[Test]
		public void Rank_get_WHERE_backing_field_has_already_been_set_SHOULD_return_value_of_backing_field()
		{
			//arrange
			var handRank = new HandRank(PokerHand.HighCard);
			_instance.Stub(x => x._rank).Return(handRank);

			//act
			var actual = _instance.Rank;

			//assert
			Assert.That(actual, Is.EqualTo(handRank));
			_handRankCalculator.AssertWasNotCalled(x => x.CalculateHandRank(_instance));
		}

		[Test]
		public void Rank_get_WHERE_backing_field_is_null_SHOULD_return_calculate_value_of_rank_set_backing_field_and_return_rank()
		{
			//arrange
			_instance.Stub(x => x._rank).Return(null).Repeat.Once();

			var handRank = new HandRank(PokerHand.ThreeOfAKind);
			_handRankCalculator.Stub(x => x.CalculateHandRank(_instance)).Return(handRank);

			_instance.Expect(x => x._rank = handRank);

			//act
			var actual = _instance.Rank;

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(handRank));
		}

		[Test]
		public void Rank_set_SHOULD_set_value_of_backing_field()
		{
			//arrange
			var handRank = new HandRank(PokerHand.FullHouse);
			_instance.Expect(x => x._rank = handRank);

			//act
			_instance.Rank = handRank;

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion

		#region Constructor

		[Test]
		public void Constructor_cards_SHOULD_service_locate_cardComparer_and_handRankCalculator_and_call_full_constructor()
		{
			//arrange
			var card1 = new Card(CardValue.Six, CardSuit.Hearts);
			var card2 = new Card(CardValue.Ten, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Ace, CardSuit.Diamonds);

			var cards = new List<Card> { card1, card2, card3 };


			//act
			var actual = new Hand(cards);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards[0], Is.EqualTo(card1));
			Assert.That(actual.Cards[1], Is.EqualTo(card2));
			Assert.That(actual.Cards[2], Is.EqualTo(card3));
		}

		[Test]
		public void Constructor_full_WHERE_more_than_seven_cards_in_supplied_cards_SHOULD_throw_error()
		{
			//arrange
			var cards = new List<Card>
			{
				new Card(CardValue.Ace, CardSuit.Diamonds),
				new Card(CardValue.Two, CardSuit.Hearts),
				new Card(CardValue.Four, CardSuit.Spades),
				new Card(CardValue.Ace, CardSuit.Spades),
				new Card(CardValue.Nine, CardSuit.Diamonds),
				new Card(CardValue.Eight, CardSuit.Clubs),
				new Card(CardValue.King, CardSuit.Diamonds),
				new Card(CardValue.Three, CardSuit.Clubs)
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => new Hand(cards, _cardComparer, _handRankCalculator));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain more than seven cards\r\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void Constructor_full_WHERE_cards_contains_duplicates_SHOULD_throw_error()
		{
			//arrange
			const CardValue cardValue = CardValue.Nine;
			const CardSuit cardSuit = CardSuit.Clubs;
			var duplicatedCard1 = new Card(cardValue, cardSuit);
			var duplicatedCard2 = new Card(cardValue, cardSuit);
			var card3 = new Card(CardValue.Eight, CardSuit.Hearts);

			var cards = new List<Card> { duplicatedCard1, card3, duplicatedCard2 };

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => new Hand(cards, _cardComparer, _handRankCalculator));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards\r\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void Constructor_full_handRankCalculator_SHOULD_copy_supplied_cards_to_Cards_property()
		{
			//arrange
			var card1 = new Card(CardValue.Six, CardSuit.Hearts);
			var card2 = new Card(CardValue.Ten, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Ace, CardSuit.Diamonds);

			var cards = new List<Card> { card1, card2, card3 };

			//act
			var actual = new Hand(cards, _cardComparer, _handRankCalculator);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards[0], Is.EqualTo(card1));
			Assert.That(actual.Cards[1], Is.EqualTo(card2));
			Assert.That(actual.Cards[2], Is.EqualTo(card3));
		}

		#endregion

		#region AddCard

		[Test]
		public void AddCard_WHERE_hand_already_has_seven_cards_SHOULD_throw_exception()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Ace, CardSuit.Diamonds);
			_instance.Stub(x => x.Cards).Return(new List<Card>
			{

				new Card(CardValue.Two, CardSuit.Hearts),
				new Card(CardValue.Four, CardSuit.Spades),
				new Card(CardValue.Ace, CardSuit.Spades),
				new Card(CardValue.Nine, CardSuit.Diamonds),
				new Card(CardValue.Eight, CardSuit.Clubs),
				new Card(CardValue.King, CardSuit.Diamonds),
				new Card(CardValue.Three, CardSuit.Clubs)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.AddCard(cardToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot have more than seven cards"));
		}

		[Test]
		public void AddCard_WHERE_hand_already_contains_card_SHOULD_throw_exception()
		{
			//arrange
			const CardValue value = CardValue.Eight;
			const CardSuit suit = CardSuit.Diamonds;
			var cardToAdd = new Card(value, suit);
			var cardInHand = new Card(value, suit);
			_instance.Stub(x => x.Cards).Return(new List<Card> { cardInHand });

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.AddCard(cardToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void AddCard_WHERE_hand_initially_has_no_cards_SHOULD_add_hand_to_card_and_reset_rank()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Six, CardSuit.Diamonds);
			var cards = new List<Card>();
			_instance.Stub(x => x.Cards).Return(cards).Repeat.Times(3);

			_instance.Expect(x => x.Rank = null);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(cards, Has.Count.EqualTo(1));
			Assert.That(cards, Has.Some.EqualTo(cardToAdd));
		}

		[Test]
		public void AddCard_WHERE_hand_has_some_cards_SHOULD_add_card_to_hand_along_with_other_cards_and_reset_rank()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Four, CardSuit.Diamonds);
			var cardInHand = new Card(CardValue.Nine, CardSuit.Hearts);

			var cards = new List<Card> { cardInHand };
			_instance.Stub(x => x.Cards).Return(cards).Repeat.Times(3);

			_instance.Expect(x => x.Rank = null);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(cards, Has.Count.EqualTo(2));
			Assert.That(cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(cards, Has.Some.EqualTo(cardInHand));
		}

		#endregion
	}
}
