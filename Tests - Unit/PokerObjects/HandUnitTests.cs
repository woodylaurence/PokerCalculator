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
	}
}
