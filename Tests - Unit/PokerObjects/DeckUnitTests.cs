using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class DeckUnitTests : AbstractUnitTestBase
	{
		private Deck _instance;

		[SetUp]
		public override void Setup()
		{
			base.Setup();

			_instance = MockRepository.GeneratePartialMock<Deck>();

			Deck.MethodObject = MockRepository.GenerateStrictMock<Deck>();
			MyRandom.MethodObject = MockRepository.GenerateStrictMock<MyRandom>();
			Utilities.MethodObject = MockRepository.GenerateStrictMock<Utilities>();
		}

		[TearDown]
		public void TearDown()
		{
			Deck.MethodObject = new Deck();
			MyRandom.MethodObject = new MyRandom();
			Utilities.MethodObject = new Utilities();
		}

		#region Static Methods

		#region Create

		[Test]
		public void Create_calls_slave()
		{
			//arrange
			Deck.MethodObject.Expect(x => x.CreateSlave()).Return(_instance);

			//act
			var actual = Deck.Create();

			//assert
			Deck.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void CreateSlave()
		{
			//arrange
			var cardSuits = new List<CardSuit> { CardSuit.Clubs, CardSuit.Hearts };
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<CardSuit>()).Return(cardSuits);

			var cardValues = new List<CardValue> { CardValue.Eight, CardValue.King };
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<CardValue>()).Return(cardValues);

			//act
			var actual = _instance.CreateSlave();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.Eight &&
																  x.Suit == CardSuit.Clubs));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.Eight &&
																  x.Suit == CardSuit.Hearts));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.King &&
																  x.Suit == CardSuit.Clubs));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.King &&
																  x.Suit == CardSuit.Hearts));
		}

		#endregion

		#region CreateShuffledDeck

		[Test]
		public void CreateShuffledDeck_calls_slave()
		{
			//arrange
			Deck.MethodObject.Expect(x => x.CreateShuffledDeckSlave()).Return(_instance);

			//act
			var actual = Deck.CreateShuffledDeck();

			//assert
			Deck.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void CreateShuffledDeckSlave()
		{
			//arrange
			var expected = MockRepository.GenerateStrictMock<Deck>();
			Deck.MethodObject.Stub(x => x.CreateSlave()).Return(expected);
			expected.Expect(x => x.Shuffle());

			//act
			var actual = _instance.CreateShuffledDeckSlave();

			//assert
			expected.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#endregion

		#region Instance Methods

		#region Shuffle

		[Test]
		public void Shuffle()
		{
			//arrange
			var card1 = new Card(CardValue.Ace, CardSuit.Clubs);
			var card2 = new Card(CardValue.Two, CardSuit.Clubs);
			var card3 = new Card(CardValue.Three, CardSuit.Clubs);
			var card4 = new Card(CardValue.Four, CardSuit.Clubs);

			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				card1, card2, card3, card4
			});

			const int firstRandomNumber = 1781;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(firstRandomNumber).Repeat.Once();

			const int secondRandomNumber = 514;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(secondRandomNumber).Repeat.Once();

			const int thirdRandomNumber = 4981;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(thirdRandomNumber).Repeat.Once();

			const int fourthRandomNumber = 45;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(fourthRandomNumber).Repeat.Once();

			_instance.Expect(x => x.Cards = Arg<List<Card>>.Matches(y => y.Count == 4 &&
																		 y[0] == card4 &&
																		 y[1] == card2 &&
																		 y[2] == card1 &&
																		 y[3] == card3));

			//act
			_instance.Shuffle();

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion

		#region RemoveCard

		[Test]
		public void RemoveCard_WHERE_card_is_not_in_deck_SHOULD_throw_error()
		{
			//arrange
			var cardToRemove = new Card(CardValue.Two, CardSuit.Clubs);

			var card1InDeck = new Card(CardValue.Two, CardSuit.Diamonds);
			var card2InDeck = new Card(CardValue.Seven, CardSuit.Clubs);
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InDeck, card2InDeck });

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.RemoveCard(cardToRemove));
			Assert.That(actualException.Message, Is.EqualTo("Cannot remove Card, it is not in Deck."));
		}

		[Test]
		public void RemoveCard()
		{
			//arrange
			var cardToRemove = new Card(CardValue.Two, CardSuit.Clubs);

			var card1InDeck = new Card(CardValue.Seven, CardSuit.Hearts);
			var card2InDeck = new Card(CardValue.Two, CardSuit.Clubs);

			var cardsInDeck = new List<Card> { card1InDeck, card2InDeck };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			//act
			_instance.RemoveCard(cardToRemove);

			//assert
			Assert.That(cardsInDeck, Has.Count.EqualTo(1));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card1InDeck));
			Assert.That(cardsInDeck, Has.None.EqualTo(card2InDeck));
		}

		#endregion

		#region TakeRandomCard

		[Test]
		public void TakeRandomCard_WHERE_no_more_cards_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance.Stub(x => x.Cards).Return(new List<Card>());

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.TakeRandomCard());
			Assert.That(actualException.Message, Is.EqualTo("No cards left in Deck to take."));
		}

		[Test]
		public void TakeRandomCard()
		{
			//arrange
			var card1 = new Card(CardValue.Seven, CardSuit.Hearts);
			var card2 = new Card(CardValue.Nine, CardSuit.Hearts);
			var card3 = new Card(CardValue.Four, CardSuit.Clubs);
			var card4 = new Card(CardValue.Two, CardSuit.Diamonds);
			var card5 = new Card(CardValue.Two, CardSuit.Spades);

			var cardsInDeck = new List<Card> { card1, card2, card3, card4, card5 };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5)).Return(3);

			//act
			var actual = _instance.TakeRandomCard();

			//assert
			Assert.That(actual, Is.EqualTo(card4));

			Assert.That(cardsInDeck, Has.Count.EqualTo(4));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card1));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card2));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card3));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card5));
			Assert.That(cardsInDeck, Has.None.EqualTo(card4));
		}

		#endregion

		#region GetRandomCards

		[Test]
		public void GetRandomCards_WHERE_not_enough_cards_left_in_deck()
		{
			//arrange
			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Ace, CardSuit.Hearts),
				new Card(CardValue.Nine, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.GetRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot get more cards than there are left in the Deck."));
		}

		[Test]
		public void GetRandomCards()
		{
			//arrange
			var card1 = new Card(CardValue.Eight, CardSuit.Clubs);
			var card2 = new Card(CardValue.Ace, CardSuit.Hearts);
			var card3 = new Card(CardValue.Nine, CardSuit.Spades);
			var card4 = new Card(CardValue.King, CardSuit.Spades);
			var card5 = new Card(CardValue.Two, CardSuit.Clubs);

			var cardsInDeck = new List<Card> { card1, card2, card3, card4, card5 };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5)).Return(2);
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(4)).Return(2);
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(3)).Return(0);

			//act
			var actual = _instance.GetRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.Some.EqualTo(card1));
			Assert.That(actual, Has.Some.EqualTo(card3));
			Assert.That(actual, Has.Some.EqualTo(card4));

			Assert.That(cardsInDeck, Has.Count.EqualTo(5));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card1));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card2));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card3));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card4));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card5));
		}

		#endregion

		#endregion
	}
}
