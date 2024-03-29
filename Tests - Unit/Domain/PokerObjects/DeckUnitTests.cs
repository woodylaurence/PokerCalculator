﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.Domain.TestData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Tests.Unit.Domain.PokerObjects
{
	[TestFixture]
	public class DeckUnitTests : AbstractUnitTestBase
	{
		private Deck _instance;
		private Mock<IRandomNumberGenerator> _randomNumberGenerator;

		[SetUp]
		protected override void Setup()
		{
			_randomNumberGenerator = new Mock<IRandomNumberGenerator>();

			base.Setup();
		}

		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddSingleton(_randomNumberGenerator.Object);
		}

		#region Constructor

		#region Constructor_RandomNumberGenerator

		[Test]
		public void Constructor_randomnumbergenerator_SHOULD_create_deck_with_all_possible_cards()
		{
			//act
			var actual = new Deck(_randomNumberGenerator.Object);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(52));
			CardTestCaseData.AllCards.ForEach(x => Assert.That(actual.Cards.Contains(x), $"Deck is missing card: {x}"));
		}

		#endregion

		#region Constructor_Cards

		[Test]
		public void Constructor_cards_SHOULD_create_deck_with_supplied_cards()
		{
			//arrange
			var cards = new List<Card>
			{
				new (CardValue.Nine, CardSuit.Diamonds),
				new (CardValue.Seven, CardSuit.Hearts),
				new (CardValue.Four, CardSuit.Spades),
				new (CardValue.Four, CardSuit.Diamonds)
			};

			//act
			var actual = new Deck(cards);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			cards.ForEach(x => Assert.That(actual.Cards.Contains(x)));
		}

		[Test]
		public void Constructor_cards_SHOULD_store_cards_in_a_new_memory_location_from_supplied_list()
		{
			//arrange
			var cards = new List<Card>
			{
				new (CardValue.Nine, CardSuit.Diamonds),
				new (CardValue.Seven, CardSuit.Hearts),
				new (CardValue.Four, CardSuit.Spades),
				new (CardValue.Four, CardSuit.Diamonds)
			};

			//act
			var actual = new Deck(cards);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));

			var cardToAddAfterDeckCreated = new Card(CardValue.Two, CardSuit.Spades);
			cards.Add(cardToAddAfterDeckCreated);
			Assert.That(actual.Cards, Has.None.EqualTo(cardToAddAfterDeckCreated));
		}

		#endregion

		#endregion

		#region Instance Methods

		#region Shuffle

		[Test]
		public void Shuffle_SHOULD_randomise_order_of_cards()
		{
			//arrange
			var card1 = new Card(CardValue.Ace, CardSuit.Clubs);
			var card2 = new Card(CardValue.Two, CardSuit.Clubs);
			var card3 = new Card(CardValue.Three, CardSuit.Clubs);
			var card4 = new Card(CardValue.Four, CardSuit.Clubs);
			_instance = new Deck(new List<Card> { card1, card2, card3, card4 });

			_randomNumberGenerator.SetupSequence(x => x.Next(5000))
								  .Returns(1781)
								  .Returns(514)
								  .Returns(4981)
								  .Returns(45);

			//act
			_instance.Shuffle();

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(4));
			Assert.That(_instance.Cards[0], Is.EqualTo(card4));
			Assert.That(_instance.Cards[1], Is.EqualTo(card2));
			Assert.That(_instance.Cards[2], Is.EqualTo(card1));
			Assert.That(_instance.Cards[3], Is.EqualTo(card3));
		}

		#endregion

		#region Clone

		[Test]
		public void Clone_SHOULD_create_new_deck_with_same_cards_in_as_original_in_the_same_order()
		{
			//arrange
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
			_instance = new Deck(cardsInOriginalDeck);

			//act
			var actual = _instance.Clone();

			//assert
			Assert.That(actual.Cards[0], Is.EqualTo(card1InOriginalDeck));
			Assert.That(actual.Cards[1], Is.EqualTo(card2InOriginalDeck));
			Assert.That(actual.Cards[2], Is.EqualTo(card3InOriginalDeck));
			Assert.That(actual.Cards[3], Is.EqualTo(card4InOriginalDeck));
		}

		[Test]
		public void Clone_SHOULD_store_cards_in_new_deck_in_new_memory_location()
		{
			//arrange
			var cardsInOriginalDeck = new List<Card>
			{
				new (CardValue.Three, CardSuit.Spades),
				new (CardValue.Seven, CardSuit.Hearts),
				new (CardValue.Eight, CardSuit.Hearts)
			};
			_instance = new Deck(cardsInOriginalDeck);

			//act
			var actual = _instance.Clone();

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(_instance.Cards));

			var randomCardFromOriginalDeck = _instance.TakeRandomCard();
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCardFromOriginalDeck));
			Assert.That(actual.Cards, Has.One.EqualTo(randomCardFromOriginalDeck));
		}

		#endregion

		#region RemoveCard

		[Test]
		public void RemoveCard_WHERE_requested_card_to_remove_is_not_in_deck_SHOULD_throw_error()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck(new List<Card>
			{
				new (CardValue.Three, CardSuit.Hearts),
				new (CardValue.Seven, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.RemoveCard(value, suit));
			Assert.That(actualException.Message, Is.EqualTo("Cannot remove card from deck, card is not in deck"));
		}

		[Test]
		public void RemoveCard_SHOULD_remove_requested_card_from_deck()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck();

			//act
			_instance.RemoveCard(value, suit);

			//assert
			Assert.That(_instance.Cards, Has.None.EqualTo(new Card(value, suit)));
		}

		[Test]
		public void RemoveCard_SHOULD_leave_other_cards_in_deck()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck();

			//act
			_instance.RemoveCard(value, suit);

			//assert
			var otherCards = CardTestCaseData.AllCards.Where(x => x != new Card(value, suit)).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x)));
		}

		#endregion

		#region TakeCard

		[Test]
		public void TakeCard_WHERE_requested_card_to_take_is_not_in_deck_SHOULD_throw_error()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck(new List<Card>
			{
				new (CardValue.Three, CardSuit.Hearts),
				new (CardValue.Seven, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.TakeCard(value, suit));
			Assert.That(actualException.Message, Is.EqualTo("Cannot remove card from deck, card is not in deck"));
		}

		[Test]
		public void TakeCard_SHOULD_remove_requested_card_from_deck()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck();

			//act
			_instance.TakeCard(value, suit);

			//assert
			Assert.That(_instance.Cards, Has.None.EqualTo(new Card(value, suit)));
		}

		[Test]
		public void TakeCard_SHOULD_return_requested_card()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck();

			//act
			var actual = _instance.TakeCard(value, suit);

			//assert
			Assert.That(actual, Is.EqualTo(new Card(value, suit)));
		}

		[Test]
		public void TakeCard_SHOULD_leave_other_cards_in_deck()
		{
			//arrange
			const CardValue value = CardValue.Four;
			const CardSuit suit = CardSuit.Diamonds;
			_instance = new Deck();

			//act
			_instance.TakeCard(value, suit);

			//assert
			var otherCards = CardTestCaseData.AllCards.Where(x => x != new Card(value, suit)).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x)));
		}

		#endregion

		#region TakeRandomCard

		[Test]
		public void TakeRandomCard_WHERE_no_cards_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance = new Deck(new List<Card>());

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.TakeRandomCard());
			Assert.That(actualException.Message, Is.EqualTo("Cannot take more cards than there are left in the deck"));
		}

		[Test]
		public void TakeRandomCard_SHOULD_remove_random_card_from_the_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex = 33;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex);

			var randomCardToTakeFromDeck = _instance.Cards[randomIndex];

			//act
			_instance.TakeRandomCard();

			//assert
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCardToTakeFromDeck));
		}

		[Test]
		public void TakeRandomCard_SHOULD_return_random_card()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex = 13;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex);

			var randomCardToTakeFromDeck = _instance.Cards[randomIndex];

			//act
			var actual = _instance.TakeRandomCard();

			//assert
			Assert.That(actual, Is.EqualTo(randomCardToTakeFromDeck));
		}

		[Test]
		public void TakeRandomCard_SHOULD_leave_other_cards_in_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex = 44;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex);

			var randomCardToTakeFromDeck = _instance.Cards[randomIndex];

			//act
			_instance.TakeRandomCard();

			//assert
			var otherCards = CardTestCaseData.AllCards.Where(x => x != randomCardToTakeFromDeck).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x)));
		}

		#endregion

		#region TakeRandomCards

		[Test]
		public void TakeRandomCards_WHERE_trying_to_take_more_cards_than_there_are_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance = new Deck(new List<Card>
			{
				new (CardValue.Queen, CardSuit.Spades),
				new (CardValue.Eight, CardSuit.Diamonds)
			});

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.TakeRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot take more cards than there are left in the deck"));
		}

		[Test]
		public void TakeRandomCards_SHOULD_remove_random_cards_from_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex1 = 22;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 4;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 1)).Returns(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 32;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 2)).Returns(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			_instance.TakeRandomCards(3);

			//assert
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCard1ToTakeFromDeck));
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCard2ToTakeFromDeck));
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCard3ToTakeFromDeck));
		}

		[Test]
		public void TakeRandomCards_SHOULD_return_random_cards()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex1 = 44;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 25;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 1)).Returns(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 49;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 2)).Returns(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			var actual = _instance.TakeRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.One.EqualTo(randomCard1ToTakeFromDeck));
			Assert.That(actual, Has.One.EqualTo(randomCard2ToTakeFromDeck));
			Assert.That(actual, Has.One.EqualTo(randomCard3ToTakeFromDeck));
		}

		[Test]
		public void TakeRandomCards_SHOULD_leave_other_cards_in_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex1 = 13;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 11;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 1)).Returns(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 19;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 2)).Returns(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			_instance.TakeRandomCards(3);

			//assert
			var otherCards = CardTestCaseData.AllCards.Where(x => x != randomCard1ToTakeFromDeck &&
																  x != randomCard2ToTakeFromDeck &&
																  x != randomCard3ToTakeFromDeck).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x), $"Deck is missing {x}"));
		}

		#endregion

		#region GetRandomCards

		[Test]
		public void GetRandomCards_WHERE_trying_to_get_more_cards_than_there_are_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance = new Deck(new List<Card>
			{
				new (CardValue.Seven, CardSuit.Hearts),
				new (CardValue.Two, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.GetRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot get more cards than there are left in the deck"));
		}

		[Test]
		public void GetRandomCards_SHOULD_not_remove_any_cards_from_the_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(17);
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 1)).Returns(2);
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 2)).Returns(29);

			//act
			_instance.GetRandomCards(3);

			//assert
			CardTestCaseData.AllCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x)));
		}

		[Test]
		public void GetRandomCards_SHOULD_return_random_cards()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator.Object);

			const int randomIndex1 = 6;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count)).Returns(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 5;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 1)).Returns(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 10;
			_randomNumberGenerator.Setup(x => x.Next(_instance.Cards.Count - 2)).Returns(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			var actual = _instance.GetRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.One.EqualTo(randomCard1ToTakeFromDeck));
			Assert.That(actual, Has.One.EqualTo(randomCard2ToTakeFromDeck));
			Assert.That(actual, Has.One.EqualTo(randomCard3ToTakeFromDeck));
		}

		#endregion

		#endregion
	}
}
