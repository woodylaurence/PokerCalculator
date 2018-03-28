using Castle.MicroKernel.Registration;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
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
		private IRandomNumberGenerator _randomNumberGenerator;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_randomNumberGenerator = MockRepository.GenerateStrictMock<IRandomNumberGenerator>();

			Utilities.MethodObject = MockRepository.GenerateStrictMock<Utilities>();

			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<CardSuit>()).Return(new List<CardSuit>()).Repeat.Once();
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<CardValue>()).Return(new List<CardValue>()).Repeat.Once();

			_instance = MockRepository.GeneratePartialMock<Deck>(_randomNumberGenerator);
		}

		[TearDown]
		protected void TearDown()
		{
			Utilities.MethodObject = new Utilities();
		}

		#region Constructor

		[Test]
		public void Constructor()
		{
			//arrange
			WindsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));

			var cardSuits = new List<CardSuit> { CardSuit.Clubs, CardSuit.Hearts };
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<CardSuit>()).Return(cardSuits);

			var cardValues = new List<CardValue> { CardValue.Eight, CardValue.King };
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<CardValue>()).Return(cardValues);

			//act
			var actual = new Deck();

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

		[Test]
		public void Constructor_supplying_cards_SHOULD_create_deck_with_supplied_cardS_in_as_new_list()
		{
			//arrange
			var newRandomNumberGenerator = MockRepository.GenerateStrictMock<IRandomNumberGenerator>();
			WindsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(newRandomNumberGenerator));

			var card1 = MockRepository.GenerateStrictMock<Card>(CardValue.Seven, CardSuit.Clubs);
			var card2 = MockRepository.GenerateStrictMock<Card>(CardValue.Nine, CardSuit.Diamonds);
			var card3 = MockRepository.GenerateStrictMock<Card>(CardValue.Ace, CardSuit.Hearts);

			var cards = new List<Card> { card1, card2, card3 };

			//act
			var actual = new Deck(cards);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
		}

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
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(firstRandomNumber).Repeat.Once();

			const int secondRandomNumber = 514;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(secondRandomNumber).Repeat.Once();

			const int thirdRandomNumber = 4981;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(thirdRandomNumber).Repeat.Once();

			const int fourthRandomNumber = 45;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(fourthRandomNumber).Repeat.Once();

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

		#region Clone

		[Test]
		public void Clone()
		{
			//arrange
			WindsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));

			var card1InOriginalDeck = new Card(CardValue.Eight, CardSuit.Clubs);
			var card2InOriginalDeck = new Card(CardValue.Seven, CardSuit.Spades);
			var card3InOriginalDeck = new Card(CardValue.Ace, CardSuit.Diamonds);
			var card4InOriginalDeck = new Card(CardValue.Four, CardSuit.Hearts);
			var cardsInOriginalDeck = new List<Card>
			{
				card1InOriginalDeck,
				card2InOriginalDeck,
				card3InOriginalDeck,
				card4InOriginalDeck
			};
			_instance.Stub(x => x.Cards).Return(cardsInOriginalDeck);

			//act
			var actual = _instance.Clone();

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cardsInOriginalDeck));
			Assert.That(actual.Cards[0], Is.EqualTo(card1InOriginalDeck));
			Assert.That(actual.Cards[1], Is.EqualTo(card2InOriginalDeck));
			Assert.That(actual.Cards[2], Is.EqualTo(card3InOriginalDeck));
			Assert.That(actual.Cards[3], Is.EqualTo(card4InOriginalDeck));
		}

		#endregion

		#region RemoveCard

		[Test]
		public void RemoveCard()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance.Expect(x => x.TakeCard(value, suit)).Return(new Card(CardValue.Jack, CardSuit.Clubs));

			//act
			_instance.RemoveCard(value, suit);

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion

		#region TakeCard

		[Test]
		public void TakeCard_WHERE_no_card_with_given_value_and_suit_in_deck_SHOULD_throw_error()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;

			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(value, CardSuit.Clubs),
				new Card(CardValue.Jack, suit),
				new Card(CardValue.Ace, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.TakeCard(value, suit));
			Assert.That(actualException.Message, Is.EqualTo("No matching card in Deck."));
		}

		[Test]
		public void TakeCard()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;

			var matchingCard = new Card(value, suit);
			var cardsInDeck = new List<Card>
			{
				new Card(value, CardSuit.Clubs),
				new Card(CardValue.Jack, suit),
				new Card(CardValue.Ace, CardSuit.Spades),
				matchingCard
			};
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			//act
			var actual = _instance.TakeCard(value, suit);

			//assert
			Assert.That(actual, Is.EqualTo(matchingCard));
			Assert.That(cardsInDeck, Has.Count.EqualTo(3));
			Assert.That(cardsInDeck, Has.None.EqualTo(matchingCard));
		}

		#endregion

		#region TakeRandomCard

		[Test]
		public void TakeRandomCard()
		{
			//arrange
			var expected = new Card(CardValue.Five, CardSuit.Spades);
			var notExpected = new Card(CardValue.Seven, CardSuit.Diamonds);
			_instance.Stub(x => x.TakeRandomCards(1)).Return(new List<Card> { expected, notExpected });

			//act
			var actual = _instance.TakeRandomCard();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region TakeRandomCards

		[Test]
		public void TakeRandomCards_WHERE_trying_to_take_more_cards_than_there_are_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Queen, CardSuit.Spades),
				new Card(CardValue.Eight, CardSuit.Diamonds)
			});

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.TakeRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot take more cards than there are left in the Deck."));
		}

		[Test]
		public void TakeRandomCards()
		{
			//arrange
			var card1 = new Card(CardValue.Queen, CardSuit.Spades);
			var card2 = new Card(CardValue.Eight, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Nine, CardSuit.Clubs);
			var card4 = new Card(CardValue.Ace, CardSuit.Hearts);
			var card5 = new Card(CardValue.Seven, CardSuit.Spades);

			_instance.Stub(x => x.Cards).Return(new List<Card> { card1, card2, card3, card4, card5 }).Repeat.Times(4);
			_randomNumberGenerator.Stub(x => x.Next(5)).Return(3);

			_instance.Stub(x => x.Cards).Return(new List<Card> { card1, card2, card3, card5 }).Repeat.Times(3);
			_randomNumberGenerator.Stub(x => x.Next(4)).Return(0);

			var cardsLeftInHand = new List<Card> { card2, card3, card5 };
			_instance.Stub(x => x.Cards).Return(cardsLeftInHand).Repeat.Times(3);
			_randomNumberGenerator.Stub(x => x.Next(3)).Return(1);

			//act
			var actual = _instance.TakeRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(card4));
			Assert.That(actual[1], Is.EqualTo(card1));
			Assert.That(actual[2], Is.EqualTo(card3));

			Assert.That(cardsLeftInHand, Has.Count.EqualTo(2));
			Assert.That(cardsLeftInHand[0], Is.EqualTo(card2));
			Assert.That(cardsLeftInHand[1], Is.EqualTo(card5));
			Assert.That(cardsLeftInHand, Has.None.EqualTo(card1));
			Assert.That(cardsLeftInHand, Has.None.EqualTo(card3));
			Assert.That(cardsLeftInHand, Has.None.EqualTo(card4));
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

			_randomNumberGenerator.Stub(x => x.Next(5)).Return(2);
			_randomNumberGenerator.Stub(x => x.Next(4)).Return(2);
			_randomNumberGenerator.Stub(x => x.Next(3)).Return(0);

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
