using Castle.MicroKernel.Registration;
using NUnit.Framework;
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

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_cardComparer = MockRepository.GenerateStrictMock<IEqualityComparer<Card>>();

			_instance = MockRepository.GeneratePartialMock<Hand>(new List<Card>(), _cardComparer, true);

			WindsorContainer.Register(Component.For<IEqualityComparer<Card>>().Instance(_cardComparer));
		}

		#region Constructor

		[Test]
		public void Constructor_cards_SHOULD_service_locate_cardComparer_and_handRankCalculator_and_call_full_constructor()
		{
			//arrange
			var card1 = new Card(CardValue.Six, CardSuit.Hearts);
			var card2 = new Card(CardValue.Ten, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Ace, CardSuit.Diamonds);

			var cards = new List<Card> { card1, card2, card3 };

			_cardComparer.Stub(x => x.GetHashCode(card1)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2)).Return(2);
			_cardComparer.Stub(x => x.GetHashCode(card3)).Return(3);

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
		public void Constructor_full_WHERE_setting_check_cards_as_false_SHOULD_not_do_any_checks_before_setting_cards()
		{
			//arrange
			var card1 = new Card(CardValue.Jack, CardSuit.Diamonds);
			var card2 = new Card(CardValue.Jack, CardSuit.Clubs);

			var cards = new List<Card> { card1, card2, card1 };

			//act
			var actual = new Hand(cards, _cardComparer, false);

			//asert
			Assert.That(actual.Cards, Is.EqualTo(cards));
		}

		[Test]
		public void Constructor_full_WHERE_setting_check_cards_not_supplied_SHOULD_default_to_true()
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
			var actualException = Assert.Throws<ArgumentException>(() => new Hand(cards, _cardComparer));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain more than seven cards\r\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
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
			var actualException = Assert.Throws<ArgumentException>(() => new Hand(cards, _cardComparer, true));
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

			_cardComparer.Stub(x => x.GetHashCode(duplicatedCard1)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(duplicatedCard2)).Return(1);
			_cardComparer.Stub(x => x.Equals(duplicatedCard1, duplicatedCard2)).Return(true);

			_cardComparer.Stub(x => x.GetHashCode(card3)).Return(3);

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => new Hand(cards, _cardComparer, true));
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

			_cardComparer.Stub(x => x.GetHashCode(card1)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2)).Return(2);
			_cardComparer.Stub(x => x.GetHashCode(card3)).Return(3);

			//act
			var actual = new Hand(cards, _cardComparer, true);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards[0], Is.EqualTo(card1));
			Assert.That(actual.Cards[1], Is.EqualTo(card2));
			Assert.That(actual.Cards[2], Is.EqualTo(card3));
		}

		#endregion

		#region Instance Methods

		#region AddCard

		[Test]
		public void AddCard()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Ace, CardSuit.Diamonds);
			_instance.Expect(x => x.AddCards(Arg<List<Card>>.Matches(y => y.Count == 1 && y.Contains(cardToAdd))));

			//act
			_instance.AddCard(cardToAdd);

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion

		#endregion

		#region Operator Overloads

		#region + Overload

		[Test]
		public void AdditionOverload_calls_helper()
		{
			//arrange
			var hand2 = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var expected = MockRepository.GenerateStrictMock<Hand>(new List<Card>());

			_instance.Stub(x => x.Operator_plus(hand2)).Return(expected);

			//act
			var actual = _instance + hand2;

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Operator_plus()
		{
			//arrange
			var hand1Card1 = new Card(CardValue.Eight, CardSuit.Diamonds);
			var hand1Card2 = new Card(CardValue.Ten, CardSuit.Clubs);
			var hand1Card3 = new Card(CardValue.Seven, CardSuit.Clubs);
			_instance.Stub(x => x.Cards).Return(new List<Card> { hand1Card1, hand1Card2, hand1Card3 });

			var hand2 = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			var hand2Card1 = new Card(CardValue.Four, CardSuit.Diamonds);
			var hand2Card2 = new Card(CardValue.Ace, CardSuit.Spades);
			var hand2Cards = new List<Card> { hand2Card1, hand2Card2 };
			hand2.Stub(x => x.Cards).Return(hand2Cards);

			var expectedHand = MockRepository.GenerateStrictMock<Hand>(new List<Card>());
			_instance.Stub(x => x.Clone()).Return(expectedHand);

			expectedHand.Expect(x =>
				x.AddCards(Arg<List<Card>>.Matches(y => y != hand2Cards &&
														y.Count == 2 &&
														y.Contains(hand2Card1) &&
														y.Contains(hand2Card2))));

			//act
			var actual = _instance.Operator_plus(hand2);

			//assert
			Assert.That(actual, Is.EqualTo(expectedHand));
		}

		#region Clone

		[Test]
		public void Clone()
		{
			//arrange
			var card1 = new Card(CardValue.Jack, CardSuit.Hearts);
			var card2 = new Card(CardValue.Three, CardSuit.Spades);
			var card3 = new Card(CardValue.Seven, CardSuit.Hearts);
			var handCards = new List<Card> { card1, card2, card3 };
			_instance.Stub(x => x.Cards).Return(handCards);

			_cardComparer.Stub(x => x.GetHashCode(card1)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2)).Return(2);
			_cardComparer.Stub(x => x.GetHashCode(card3)).Return(3);

			//act
			var actual = _instance.Clone();

			//assert
			Assert.That(actual, Is.Not.EqualTo(_instance));
			Assert.That(actual.Cards, Is.Not.SameAs(handCards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards, Has.One.EqualTo(card1));
			Assert.That(actual.Cards, Has.One.EqualTo(card2));
			Assert.That(actual.Cards, Has.One.EqualTo(card3));
		}

		#endregion

		#region AddCards

		[Test]
		public void AddCards_WHERE_empty_list_of_cards_to_add_SHOULD_add_no_cards_to_hand_and_unset_rank()
		{
			//arrange
			_instance.Expect(x => x.VerifyCardsCanBeAdded(new List<Card>()));

			var existingCard = new Card(CardValue.Five, CardSuit.Diamonds);
			var handCards = new List<Card> { existingCard };
			_instance.Stub(x => x.Cards).Return(handCards);

			//act
			_instance.AddCards(new List<Card>());

			//assert
			_instance.VerifyAllExpectations();

			Assert.That(handCards, Has.Count.EqualTo(1));
			Assert.That(handCards, Has.Some.EqualTo(existingCard));
		}

		[Test]
		public void AddCards_WHERE_list_of_single_card_to_add_SHOULD_add_card_to_hand_and_unset_rank()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Jack, CardSuit.Hearts);
			var cardsToAdd = new List<Card> { cardToAdd };

			_instance.Expect(x => x.VerifyCardsCanBeAdded(cardsToAdd));

			var existingcard1 = new Card(CardValue.Five, CardSuit.Diamonds);
			var existingcard2 = new Card(CardValue.Ace, CardSuit.Clubs);
			var handCards = new List<Card> { existingcard1, existingcard2 };
			_instance.Stub(x => x.Cards).Return(handCards);

			//act
			_instance.AddCards(cardsToAdd);

			//assert
			_instance.VerifyAllExpectations();

			Assert.That(handCards, Has.Count.EqualTo(3));
			Assert.That(handCards, Has.Some.EqualTo(existingcard1));
			Assert.That(handCards, Has.Some.EqualTo(existingcard2));
			Assert.That(handCards, Has.Some.EqualTo(cardToAdd));
		}

		[Test]
		public void AddCards_WHERE_list_of_multiple_cards_to_add_SHOULD_add_cards_to_hand_and_unset_rank()
		{
			//arrange
			var cardToAdd1 = new Card(CardValue.Jack, CardSuit.Hearts);
			var cardToAdd2 = new Card(CardValue.King, CardSuit.Spades);
			var cardsToAdd = new List<Card> { cardToAdd1, cardToAdd2 };
			_instance.Expect(x => x.VerifyCardsCanBeAdded(cardsToAdd));

			var existingcard1 = new Card(CardValue.Five, CardSuit.Diamonds);
			var existingcard2 = new Card(CardValue.Ace, CardSuit.Clubs);
			var handCards = new List<Card> { existingcard1, existingcard2 };
			_instance.Stub(x => x.Cards).Return(handCards);

			//act
			_instance.AddCards(cardsToAdd);

			//assert
			_instance.VerifyAllExpectations();

			Assert.That(handCards, Has.Count.EqualTo(4));
			Assert.That(handCards, Has.Some.EqualTo(existingcard1));
			Assert.That(handCards, Has.Some.EqualTo(existingcard2));
			Assert.That(handCards, Has.Some.EqualTo(cardToAdd1));
			Assert.That(handCards, Has.Some.EqualTo(cardToAdd2));
		}

		#endregion

		#region VerifyCardsCanBeAdded

		[Test]
		public void VerifyCardsCanBeAdded_WHERE_hand_is_initially_empty_but_trying_to_add_more_than_seven_cards_SHOULD_throw_error()
		{
			//arrange
			var cardsToAdd = new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Clubs),
				new Card(CardValue.Nine, CardSuit.Spades),
				new Card(CardValue.Two, CardSuit.Diamonds),
				new Card(CardValue.Ace, CardSuit.Diamonds),
				new Card(CardValue.Seven, CardSuit.Hearts),
				new Card(CardValue.Eight, CardSuit.Spades),
				new Card(CardValue.Four, CardSuit.Diamonds),
				new Card(CardValue.King, CardSuit.Clubs)
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.VerifyCardsCanBeAdded(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot have more than seven cards"));
		}

		[Test]
		public void VerifyCardsCanBeAdded_WHERE_hand_is_initially_not_empty_but_trying_to_add_multiple_cards_taking_total_cards_over_seven_SHOULD_throw_error()
		{
			//arrange
			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Jack, CardSuit.Diamonds),
				new Card(CardValue.Queen, CardSuit.Spades),
				new Card(CardValue.Five, CardSuit.Hearts),
				new Card(CardValue.King, CardSuit.Diamonds)
			});

			var cardsToAdd = new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Clubs),
				new Card(CardValue.Nine, CardSuit.Spades),
				new Card(CardValue.Two, CardSuit.Diamonds),
				new Card(CardValue.Ace, CardSuit.Diamonds)
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.VerifyCardsCanBeAdded(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot have more than seven cards"));
		}

		[Test]
		public void VerifyCardsCanBeAdded_WHERE_adding_card_that_already_exists_in_hand_SHOULD_throw_error()
		{
			//arrange
			var card1InHand = new Card(CardValue.King, CardSuit.Diamonds);
			var card2InHand = new Card(CardValue.Nine, CardSuit.Spades);
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InHand, card2InHand });

			var card1ToAdd = new Card(CardValue.Eight, CardSuit.Clubs);
			var card2ToAdd = new Card(CardValue.Nine, CardSuit.Spades);
			var cardsToAdd = new List<Card> { card1ToAdd, card2ToAdd };

			_cardComparer.Stub(x => x.GetHashCode(card1ToAdd)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2ToAdd)).Return(2);
			_cardComparer.Stub(x => x.GetHashCode(card1InHand)).Return(3);
			_cardComparer.Stub(x => x.GetHashCode(card2InHand)).Return(2);

			_cardComparer.Stub(x => x.Equals(card1InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card1InHand, card2ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card2ToAdd)).Return(true);

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.VerifyCardsCanBeAdded(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void VerifyCardsCanBeAdded_WHERE_adding_duplicate_cards_SHOULD_throw_error()
		{
			//arrange
			var card1InHand = new Card(CardValue.King, CardSuit.Diamonds);
			var card2InHand = new Card(CardValue.Nine, CardSuit.Spades);
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InHand, card2InHand });

			var card1ToAdd = new Card(CardValue.Eight, CardSuit.Clubs);
			var card2ToAdd = new Card(CardValue.Four, CardSuit.Spades);
			var card3ToAdd = new Card(CardValue.Eight, CardSuit.Clubs);
			var cardsToAdd = new List<Card> { card1ToAdd, card2ToAdd, card3ToAdd };

			_cardComparer.Stub(x => x.GetHashCode(card1ToAdd)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2ToAdd)).Return(2);
			_cardComparer.Stub(x => x.GetHashCode(card3ToAdd)).Return(1);

			_cardComparer.Stub(x => x.Equals(card1ToAdd, card3ToAdd)).Return(true);

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.VerifyCardsCanBeAdded(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void VerifyCardsCanBeAdded_WHERE_adding_distinct_cards_bringing_total_cards_in_hand_to_seven_SHOULD_not_throw_error()
		{
			//arrange
			var card1InHand = new Card(CardValue.Jack, CardSuit.Diamonds);
			var card2InHand = new Card(CardValue.Queen, CardSuit.Spades);
			var card3InHand = new Card(CardValue.Five, CardSuit.Hearts);
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InHand, card2InHand, card3InHand });

			var card1ToAdd = new Card(CardValue.Jack, CardSuit.Clubs);
			var card2ToAdd = new Card(CardValue.Nine, CardSuit.Spades);
			var card3ToAdd = new Card(CardValue.Two, CardSuit.Diamonds);
			var card4ToAdd = new Card(CardValue.Ace, CardSuit.Diamonds);
			var cardsToAdd = new List<Card> { card1ToAdd, card2ToAdd, card3ToAdd, card4ToAdd };

			_cardComparer.Stub(x => x.GetHashCode(card1ToAdd)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2ToAdd)).Return(2);
			_cardComparer.Stub(x => x.GetHashCode(card3ToAdd)).Return(3);
			_cardComparer.Stub(x => x.GetHashCode(card4ToAdd)).Return(4);
			_cardComparer.Stub(x => x.GetHashCode(card1InHand)).Return(5);
			_cardComparer.Stub(x => x.GetHashCode(card2InHand)).Return(6);
			_cardComparer.Stub(x => x.GetHashCode(card3InHand)).Return(7);

			_cardComparer.Stub(x => x.Equals(card1InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card1InHand, card2ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card1InHand, card3ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card1InHand, card4ToAdd)).Return(false);

			_cardComparer.Stub(x => x.Equals(card2InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card2ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card3ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card4ToAdd)).Return(false);

			_cardComparer.Stub(x => x.Equals(card3InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card3InHand, card2ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card3InHand, card3ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card3InHand, card4ToAdd)).Return(false);

			//act + assert
			_instance.VerifyCardsCanBeAdded(cardsToAdd);
		}

		[Test]
		public void VerifyCardsCanBeAdded()
		{
			//arrange
			var card1InHand = new Card(CardValue.Jack, CardSuit.Diamonds);
			var card2InHand = new Card(CardValue.Queen, CardSuit.Spades);
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InHand, card2InHand });

			var card1ToAdd = new Card(CardValue.Jack, CardSuit.Clubs);
			var card2ToAdd = new Card(CardValue.Nine, CardSuit.Spades);
			var cardsToAdd = new List<Card> { card1ToAdd, card2ToAdd };

			_cardComparer.Stub(x => x.GetHashCode(card1ToAdd)).Return(1);
			_cardComparer.Stub(x => x.GetHashCode(card2ToAdd)).Return(2);

			_cardComparer.Stub(x => x.Equals(card1InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card1InHand, card2ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card1ToAdd)).Return(false);
			_cardComparer.Stub(x => x.Equals(card2InHand, card2ToAdd)).Return(false);

			//act + assert
			_instance.VerifyCardsCanBeAdded(cardsToAdd);
		}

		#endregion

		#endregion

		#endregion
	}
}
