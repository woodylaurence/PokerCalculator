using System;
using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class DeckUnitTests : AbstractUnitTestBase
	{
		private Deck _instance;

		[SetUp]
		public new void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<Deck>();

			Deck.MethodObject = MockRepository.GenerateStrictMock<Deck>();
			Card.MethodObject = MockRepository.GenerateStrictMock<Card>();
			MyRandom.MethodObject = MockRepository.GenerateStrictMock<MyRandom>();
		}

		[TearDown]
		public void TearDown()
		{
			Deck.MethodObject = new Deck();
			Card.MethodObject = new Card();
			MyRandom.MethodObject = new MyRandom();
		}

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
			_instance.Stub(x => x.GetAllCardSuits()).Return(new List<CardSuit> { CardSuit.Clubs, CardSuit.Hearts });
			_instance.Stub(x => x.GetAllCardValues()).Return(new List<CardValue> { CardValue.Eight, CardValue.King });

			var card1 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.Eight, CardSuit.Clubs)).Return(card1);

			var card2 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.Eight, CardSuit.Hearts)).Return(card2);

			var card3 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.King, CardSuit.Hearts)).Return(card3);

			var card4 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.King, CardSuit.Clubs)).Return(card4);

			//act
			var actual = _instance.CreateSlave();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card4));
		}

		#region GetAllCardSuits

		[Test]
		public void GetAllCardSuits()
		{
			//act
			var actual = _instance.GetAllCardSuits();

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Spades));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Hearts));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Diamonds));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Clubs));
		}

		#endregion

		#region GetAllCardValues

		[Test]
		public void GetAllCardValues()
		{
			//act
			var actual = _instance.GetAllCardValues();

			//assert
			Assert.That(actual, Has.Count.EqualTo(13));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Ace));
			Assert.That(actual, Has.Some.EqualTo(CardValue.King));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Queen));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Jack));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Ten));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Nine));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Eight));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Seven));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Six));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Five));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Four));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Three));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Two));
		}

		#endregion

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

		#region Shuffle

		[Test]
		public void Shuffle()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();
			var card4 = MockRepository.GenerateStrictMock<Card>();

			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				card1, card2, card3, card4
			});

			card1.Stub(x => x.Value).Return(CardValue.Ace);
			card2.Stub(x => x.Value).Return(CardValue.Two);
			card3.Stub(x => x.Value).Return(CardValue.Three);
			card4.Stub(x => x.Value).Return(CardValue.Four);

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
			var cardToRemove = MockRepository.GenerateStrictMock<Card>();

			var card1InDeck = MockRepository.GenerateStrictMock<Card>();
			var card2InDeck = MockRepository.GenerateStrictMock<Card>();
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InDeck, card2InDeck });

			cardToRemove.Stub(x => x.Value).Return(CardValue.Two);
			cardToRemove.Stub(x => x.Suit).Return(CardSuit.Clubs);

			card1InDeck.Stub(x => x.Value).Return(CardValue.Two);
			card1InDeck.Stub(x => x.Suit).Return(CardSuit.Diamonds);

			card2InDeck.Stub(x => x.Value).Return(CardValue.Seven);
			card2InDeck.Stub(x => x.Suit).Return(CardSuit.Clubs);

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.RemoveCard(cardToRemove));
			Assert.That(actualException.Message, Is.EqualTo("Cannot remove Card, it is not in Deck."));
		}

		[Test]
		public void RemoveCard()
		{
			//arrange
			var cardToRemove = MockRepository.GenerateStrictMock<Card>();

			var card1InDeck = MockRepository.GenerateStrictMock<Card>();
			var card2InDeck = MockRepository.GenerateStrictMock<Card>();
			var cardsInDeck = new List<Card> { card1InDeck, card2InDeck };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			cardToRemove.Stub(x => x.Value).Return(CardValue.Two);
			cardToRemove.Stub(x => x.Suit).Return(CardSuit.Clubs);

			card1InDeck.Stub(x => x.Value).Return(CardValue.Seven);
			card1InDeck.Stub(x => x.Suit).Return(CardSuit.Hearts);

			card2InDeck.Stub(x => x.Value).Return(CardValue.Two);
			card2InDeck.Stub(x => x.Suit).Return(CardSuit.Clubs);

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
			var actualException = Assert.Throws<Exception>(() =>_instance.TakeRandomCard());
			Assert.That(actualException.Message, Is.EqualTo("No cards left in Deck to take."));
		}

		[Test]
		public void TakeRandomCard()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();
			var card4 = MockRepository.GenerateStrictMock<Card>();
			var card5 = MockRepository.GenerateStrictMock<Card>();


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
				MockRepository.GenerateStrictMock<Card>(),
				MockRepository.GenerateStrictMock<Card>()
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.GetRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot get more cards than there are left in the Deck."));
		}

		[Test]
		public void GetRandomCards()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();
			var card4 = MockRepository.GenerateStrictMock<Card>();
			var card5 = MockRepository.GenerateStrictMock<Card>();

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
	}
}
