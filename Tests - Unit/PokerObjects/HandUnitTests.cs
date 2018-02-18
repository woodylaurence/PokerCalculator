using Castle.MicroKernel.Registration;
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
		Hand _instance;
		private IHandRankCalculator _handRankCalculator;

		[SetUp]
		public new void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<Hand>();

			_handRankCalculator = MockRepository.GenerateStrictMock<IHandRankCalculator>();
			WindsorContainer.Register(Component.For<IHandRankCalculator>().Instance(_handRankCalculator));

			Hand.MethodObject = MockRepository.GenerateStrictMock<Hand>();
			HandRank.MethodObject = MockRepository.GenerateStrictMock<HandRank>();
		}

		[TearDown]
		public void TearDown()
		{
			Hand.MethodObject = new Hand();
			HandRank.MethodObject = new HandRank();
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
			_handRankCalculator.AssertWasNotCalled(x => x.CalculateHandRank(_instance));
		}

		[Test]
		public void Rank_get_WHERE_backing_field_is_null_SHOULD_return_calculate_value_of_rank_set_backing_field_and_return_rank()
		{
			//arrange
			_instance.Stub(x => x._rank).Return(null).Repeat.Once();

			var handRank = MockRepository.GenerateStrictMock<HandRank>();
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
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Spades) };
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
			var actualException = Assert.Throws<ArgumentException>(() => _instance.CreateSlave(cards));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain more than seven cards\r\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void CreateSlave_WHERE_cards_contains_duplicates_SHOULD_throw_error()
		{
			//arrange
			const CardValue cardValue = CardValue.Nine;
			const CardSuit cardSuit = CardSuit.Clubs;
			var duplicatedCard1 = new Card(cardValue, cardSuit);
			var duplicatedCard2 = new Card(cardValue, cardSuit);
			var card3 = new Card(CardValue.Eight, CardSuit.Hearts);

			var cards = new List<Card> { duplicatedCard1, card3, duplicatedCard2 };

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.CreateSlave(cards));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards\r\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void CreateSlave_SHOULD_copy_supplied_cards_to_Cards_property()
		{
			//arrange
			var card1 = new Card(CardValue.Six, CardSuit.Hearts);
			var card2 = new Card(CardValue.Ten, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Ace, CardSuit.Diamonds);

			var cards = new List<Card> { card1, card2, card3 };

			//act
			var actual = _instance.CreateSlave(cards);

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
