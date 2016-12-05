using System;
using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using Rhino.Mocks;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Unit
{
	[TestFixture]
	public class HandUnitTests
	{
		Hand _instance;

		[SetUp]
		public void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<Hand>();

			Hand.MethodObject = MockRepository.GenerateStrictMock<Hand>();
		}

		[TearDown]
		public void TearDown()
		{
			Hand.MethodObject = new Hand();
		}

		#region Properties and Fields

		[Test]
		public void Rank_get_WHERE_backing_field_has_already_been_set_SHOULD_return_value_of_backing_field()
		{
			//arrange
			var handRank = MockRepository.GenerateStrictMock<HandRank>();
			_instance.Stub(x => x._rank).Return(handRank);

			//act
			var actual = _instance.Rank;

			//assert
			Assert.That(actual, Is.EqualTo(handRank));
			_instance.AssertWasNotCalled(x => x.CalculateRank());
		}

		[Test]
		public void Rank_get_WHERE_backing_field_is_null_SHOULD_return_calculate_value_of_rank_set_backing_field_and_return_rank()
		{
			//arrange
			_instance.Stub(x => x._rank).Return(null).Repeat.Once();

			var handRank = MockRepository.GenerateStrictMock<HandRank>();
			_instance.Stub(x => x.CalculateRank()).Return(handRank);

			_instance.Expect(x => x._rank = handRank);
			_instance.Stub(x => x._rank).Return(handRank).Repeat.Once();

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
			var handRank = MockRepository.GenerateStrictMock<HandRank>();
			_instance.Expect(x => x._rank = handRank);

			//act
			_instance.Rank = handRank;

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion

		#region Create

		[Test]
		public void Create_WHERE_not_supplying_cards_SHOULD_call_slave_with_null_list_of_cards()
		{
			//arrange
			Hand.MethodObject.Expect(x => x.CreateSlave(null)).Return(_instance);

			//act
			var actual = Hand.Create();

			//assert
			Hand.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void Create_WHERE_supplying_cards_SHOULD_call_slave_with_supplied_cards()
		{
			//arrange
			var cards = new List<Card> { MockRepository.GenerateStrictMock<Card>() };
			Hand.MethodObject.Expect(x => x.CreateSlave(cards)).Return(_instance);

			//act
			var actual = Hand.Create(cards);

			//assert
			Hand.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void CreateSlave_WHERE_more_than_seven_cards_in_supplied_cards_SHOULD_throw_error()
		{
			//arrange
			var cards = new List<Card> 
			{ 
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>()
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.CreateSlave(cards));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain more than seven cards\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void CreateSlave_WHERE_cards_contains_duplicates_SHOULD_throw_error()
		{
			//arrange
			var duplicatedCard1 = MockRepository.GenerateStrictMock<Card>();
			var duplicatedCard2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();

			const CardValue cardValue = CardValue.Nine;
			const CardSuit cardSuit = CardSuit.Clubs;
			duplicatedCard1.Stub(x => x.Value).Return(cardValue);
			duplicatedCard2.Stub(x => x.Value).Return(cardValue);
			card3.Stub(x => x.Value).Return(CardValue.Eight);

			duplicatedCard1.Stub(x => x.Suit).Return(cardSuit);
			duplicatedCard2.Stub(x => x.Suit).Return(cardSuit);
			card3.Stub(x => x.Suit).Return(CardSuit.Hearts);

			var cards = new List<Card> { duplicatedCard1, card3, duplicatedCard2 };

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.CreateSlave(cards));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void CreateSlave_SHOULD_copy_supplied_cards_to_Cards_property()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();

			var cards = new List<Card> { card1, card2, card3 };

			card1.Stub(x => x.Value).Return(CardValue.Six);
			card2.Stub(x => x.Value).Return(CardValue.Ten);
			card3.Stub(x => x.Value).Return(CardValue.Ace);

			card1.Stub(x => x.Suit).Return(CardSuit.Hearts);
			card2.Stub(x => x.Suit).Return(CardSuit.Diamonds);
			card3.Stub(x => x.Suit).Return(CardSuit.Diamonds);

			//act
			var actual = _instance.CreateSlave(cards);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards [0], Is.EqualTo(card1));
			Assert.That(actual.Cards [1], Is.EqualTo(card2));
			Assert.That(actual.Cards [2], Is.EqualTo(card3));
		}

		#endregion

		#region AddCard

		[Test]
		public void AddCard_WHERE_hand_already_has_seven_cards_SHOULD_throw_exception()
		{
			//arrange
			var cardToAdd = MockRepository.GenerateStrictMock<Card>();
			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>()
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.AddCard(cardToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot have more than seven cards"));
		}

		[Test]
		public void AddCard_WHERE_hand_already_contains_card_SHOULD_throw_exception()
		{
			//arrange
			var cardToAdd = MockRepository.GenerateStrictMock<Card>();
			var cardInHand = MockRepository.GenerateStrictMock<Card>();
			_instance.Stub(x => x.Cards).Return(new List<Card> { cardInHand });

			const CardValue value = CardValue.Eight;
			cardToAdd.Stub(x => x.Value).Return(value);
			cardInHand.Stub(x => x.Value).Return(value);

			const CardSuit suit = CardSuit.Diamonds;
			cardToAdd.Stub(x => x.Suit).Return(suit);
			cardInHand.Stub(x => x.Suit).Return(suit);

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.AddCard(cardToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void AddCard_WHERE_hand_initially_has_no_cards_SHOULD_add_hand_to_card_and_reset_rank()
		{
			//arrange
			var cardToAdd = MockRepository.GenerateStrictMock<Card>();
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
			var cardToAdd = MockRepository.GenerateStrictMock<Card>();
			var cardInHand = MockRepository.GenerateStrictMock<Card>();
			var cards = new List<Card> { cardInHand };
			_instance.Stub(x => x.Cards).Return(cards).Repeat.Times(3);

			_instance.Expect(x => x.Rank = null);

			cardToAdd.Stub(x => x.Value).Return(CardValue.Four);
			cardInHand.Stub(x => x.Value).Return(CardValue.Nine);

			cardToAdd.Stub(x => x.Suit).Return(CardSuit.Diamonds);
			cardInHand.Stub(x => x.Suit).Return(CardSuit.Hearts);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(cards, Has.Count.EqualTo(2));
			Assert.That(cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(cards, Has.Some.EqualTo(cardInHand));
		}

		#endregion

		#region GetFlushValues

		[Test]
		public void GetFlushValues_SHOULD_group_cards_together_by_suit_and_return_list_of_card_values_ordered_by_value_descending_for_suit_with_most_cards()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();
			var card4 = MockRepository.GenerateStrictMock<Card>();
			var card5 = MockRepository.GenerateStrictMock<Card>();
			var card6 = MockRepository.GenerateStrictMock<Card>();

			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				card1, card2, card3, card4, card5, card6
			});

			const CardValue card1Value = CardValue.Six;
			card1.Stub(x => x.Suit).Return(CardSuit.Clubs);
			card1.Stub(x => x.Value).Return(card1Value);

			card2.Stub(x => x.Suit).Return(CardSuit.Hearts);
			card2.Stub(x => x.Value).Return(CardValue.Ace);

			const CardValue card3Value = CardValue.Four;
			card3.Stub(x => x.Suit).Return(CardSuit.Clubs);
			card3.Stub(x => x.Value).Return(card3Value);

			card4.Stub(x => x.Suit).Return(CardSuit.Diamonds);
			card4.Stub(x => x.Value).Return(CardValue.Jack);

			const CardValue card5Value = CardValue.Nine;
			card5.Stub(x => x.Suit).Return(CardSuit.Clubs);
			card5.Stub(x => x.Value).Return(card5Value);

			card6.Stub(x => x.Suit).Return(CardSuit.Diamonds);
			card6.Stub(x => x.Value).Return(CardValue.Two);

			//act
			var actual = _instance.GetFlushValues();

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(card5Value));
			Assert.That(actual[1], Is.EqualTo(card1Value));
			Assert.That(actual[2], Is.EqualTo(card3Value));
		}

		#endregion

		#region GetStraightValues

		[Test]
		public void GetStraightValues_without_card_list_SHOULD_pass_hand_cards_to_GetStraightValues()
		{
			var cardInHand = MockRepository.GenerateStrictMock<Card>();
			_instance.Stub(x => x.Cards).Return(new List<Card> { cardInHand });

			const CardValue cardValue = CardValue.Seven;
			cardInHand.Stub(x => x.Value).Return(cardValue);

			var expected = new List<CardValue> { CardValue.Nine };
			_instance.Expect(x => x.GetStraightValues(new List<CardValue> { cardValue })).Return(expected);

			//act
			var actual = _instance.GetStraightValues();

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_no_consecutive_values_SHOULD_return_highest_value()
		{
			//Cards Values: K J 9 8 4 6 2

			//arrange
			const CardValue highestCardValue = CardValue.King;
			var cardValue = new List<CardValue>
			{
				CardValue.Eight,
				CardValue.Jack,
				highestCardValue,
				CardValue.Two,
				CardValue.Four,
				CardValue.Six
			};

			//act
			var actual = _instance.GetStraightValues(cardValue);

			//assert
			Assert.That(actual, Has.Count.EqualTo(1));
			Assert.That(actual[0], Is.EqualTo(highestCardValue));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_straight_but_has_longer_straight_starting_at_lower_value_SHOULD_return_longer_straight_with_cards_ordered_by_value()
		{
			//Cards Values: K 10 9 5 4 3 2 

			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();
			var card4 = MockRepository.GenerateStrictMock<Card>();
			var card5 = MockRepository.GenerateStrictMock<Card>();
			var card6 = MockRepository.GenerateStrictMock<Card>();
			var card7 = MockRepository.GenerateStrictMock<Card>();

			const CardValue straightCardValue1 = CardValue.Five;
			const CardValue straightCardValue2 = CardValue.Four;
			const CardValue straightCardValue3 = CardValue.Three;
			const CardValue straightCardValue4 = CardValue.Two;

			var cardValues = new List<CardValue>
			{
				CardValue.King,
				straightCardValue2,
				CardValue.Nine,
				CardValue.Ten,
				straightCardValue3,
				straightCardValue1,
				straightCardValue4
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
			Assert.That(actual[3], Is.EqualTo(straightCardValue4));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_two_straights_of_same_length_SHOULD_return_higher_straight_with_cards_ordered_by_value()
		{
			//Cards Values: J 10 9 6 4 3 2 

			//arrange
			const CardValue straightCardValue1 = CardValue.Jack;
			const CardValue straightCardValue2 = CardValue.Ten;
			const CardValue straightCardValue3 = CardValue.Nine;

			var cardValues = new List<CardValue>
			{
				CardValue.Six,
				straightCardValue2,
				CardValue.Four,
				CardValue.Three,
				straightCardValue3,
				straightCardValue1,
				CardValue.Two
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_hand_has_ace_and_have_longer_straight_using_ace_as_low_than_ace_as_high_SHOULD_return_longer_straight_with_ace_used_as_low_and_cards_ordered_by_value()
		{
			//Cards Values: K Q 9 4 3 2 A

			//arrange
			const CardValue straightCardValue1 = CardValue.Four;
			const CardValue straightCardValue2 = CardValue.Three;
			const CardValue straightCardValue3 = CardValue.Two;
			const CardValue straightCardValue4 = CardValue.Ace;

			var cardValues = new List<CardValue>
			{
				CardValue.Queen,
				straightCardValue1,
				CardValue.King,
				straightCardValue2,
				straightCardValue4,
				CardValue.Nine,
				straightCardValue3
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
			Assert.That(actual[3], Is.EqualTo(straightCardValue4));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_hand_has_ace_and_have_longer_straight_using_ace_as_high_than_ace_as_low_SHOULD_return_longer_straight_with_ace_used_as_high_and_cards_ordered_by_value()
		{
			//Cards Values: K Q J 7 3 2 A

			//arrange
			const CardValue straightCardValue1 = CardValue.Ace;
			const CardValue straightCardValue2 = CardValue.King;
			const CardValue straightCardValue3 = CardValue.Queen;
			const CardValue straightCardValue4 = CardValue.Jack;

			var cardValues = new List<CardValue>
			{
				CardValue.Seven,
				straightCardValue3,
				straightCardValue2,
				CardValue.Two,
				straightCardValue4,
				straightCardValue1,
				CardValue.Three
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
			Assert.That(actual[3], Is.EqualTo(straightCardValue4));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_hand_has_ace_and_have_higher_straight_using_ace_as_high_than_ace_as_low_SHOULD_return_higher_straight_with_ace_used_as_high_and_cards_ordered_by_value()
		{
			//Cards Values: K Q 9 7 3 2 A

			//arrange
			const CardValue straightCardValue1 = CardValue.Ace;
			const CardValue straightCardValue2 = CardValue.King;
			const CardValue straightCardValue3 = CardValue.Queen;

			var cardValues = new List<CardValue>
			{
				straightCardValue3,
				CardValue.Seven,
				straightCardValue2,
				CardValue.Two,
				straightCardValue1,
				CardValue.Three,
				CardValue.Nine
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
		}

		#endregion
	}
}
