using Castle.MicroKernel.Registration;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.TestData;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class DeckUnitTests : AbstractUnitTestBase
	{
		private Deck _instance;
		private IRandomNumberGenerator _randomNumberGenerator;
		private CardComparer _cardComparer;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_randomNumberGenerator = MockRepository.GenerateStrictMock<IRandomNumberGenerator>();
			WindsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));

			_cardComparer = new CardComparer();
		}

		#region Constructor

		#region Constructor_RandomNumberGenerator

		[Test]
		public void Constructor_randomnumbergenerator_SHOULD_create_deck_with_all_possible_cards()
		{
			//act
			var actual = new Deck(_randomNumberGenerator);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(52));
			CardTestCaseData.AllCards.ForEach(x => Assert.That(actual.Cards.Contains(x, _cardComparer), $"Deck is missing card: {x}"));
		}

		#endregion

		#region Constructor_Cards

		[Test]
		public void Constructor_cards_SHOULD_create_deck_with_supplied_cards()
		{
			//arrange
			var cards = new List<Card>
			{
				new Card(CardValue.Nine, CardSuit.Diamonds),
				new Card(CardValue.Seven, CardSuit.Hearts),
				new Card(CardValue.Four, CardSuit.Spades),
				new Card(CardValue.Four, CardSuit.Diamonds)
			};

			//act
			var actual = new Deck(cards);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			cards.ForEach(x => Assert.That(actual.Cards.Contains(x, _cardComparer)));
		}

		[Test]
		public void Constructor_cards_SHOULD_store_cards_in_a_new_memory_location_from_supplied_list()
		{
			//arrange
			var cards = new List<Card>
			{
				new Card(CardValue.Nine, CardSuit.Diamonds),
				new Card(CardValue.Seven, CardSuit.Hearts),
				new Card(CardValue.Four, CardSuit.Spades),
				new Card(CardValue.Four, CardSuit.Diamonds)
			};

			//act
			var actual = new Deck(cards);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));

			var cardToAddAfterDeckCreated = new Card(CardValue.Two, CardSuit.Spades);
			cards.Add(cardToAddAfterDeckCreated);
			Assert.That(actual.Cards, Has.None.EqualTo(cardToAddAfterDeckCreated).Using(_cardComparer));
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

			const int firstRandomNumber = 1781;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(firstRandomNumber).Repeat.Once();

			const int secondRandomNumber = 514;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(secondRandomNumber).Repeat.Once();

			const int thirdRandomNumber = 4981;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(thirdRandomNumber).Repeat.Once();

			const int fourthRandomNumber = 45;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(fourthRandomNumber).Repeat.Once();

			//act
			_instance.Shuffle();

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(4));
			Assert.That(_instance.Cards[0], Is.EqualTo(card4).Using(_cardComparer));
			Assert.That(_instance.Cards[1], Is.EqualTo(card2).Using(_cardComparer));
			Assert.That(_instance.Cards[2], Is.EqualTo(card1).Using(_cardComparer));
			Assert.That(_instance.Cards[3], Is.EqualTo(card3).Using(_cardComparer));
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
			Assert.That(actual.Cards[0], Is.EqualTo(card1InOriginalDeck).Using(_cardComparer));
			Assert.That(actual.Cards[1], Is.EqualTo(card2InOriginalDeck).Using(_cardComparer));
			Assert.That(actual.Cards[2], Is.EqualTo(card3InOriginalDeck).Using(_cardComparer));
			Assert.That(actual.Cards[3], Is.EqualTo(card4InOriginalDeck).Using(_cardComparer));
		}

		[Test]
		public void Clone_SHOULD_store_cards_in_new_deck_in_new_memory_location()
		{
			//arrange
			var cardsInOriginalDeck = new List<Card>
			{
				new Card(CardValue.Three, CardSuit.Spades),
				new Card(CardValue.Seven, CardSuit.Hearts),
				new Card(CardValue.Eight, CardSuit.Hearts)
			};
			_instance = new Deck(cardsInOriginalDeck);

			//act
			var actual = _instance.Clone();

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(_instance.Cards));

			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(1);
			var randomCardFromOriginalDeck = _instance.TakeRandomCard();
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCardFromOriginalDeck).Using(_cardComparer));
			Assert.That(actual.Cards, Has.One.EqualTo(randomCardFromOriginalDeck).Using(_cardComparer));
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
				new Card(CardValue.Three, CardSuit.Hearts),
				new Card(CardValue.Seven, CardSuit.Spades)
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
			Assert.That(_instance.Cards, Has.None.EqualTo(new Card(value, suit)).Using(_cardComparer));
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
			var otherCards = CardTestCaseData.AllCards.Where(x => _cardComparer.Equals(x, new Card(value, suit)) == false).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x).Using(_cardComparer)));
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
				new Card(CardValue.Three, CardSuit.Hearts),
				new Card(CardValue.Seven, CardSuit.Spades)
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
			Assert.That(_instance.Cards, Has.None.EqualTo(new Card(value, suit)).Using(_cardComparer));
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
			Assert.That(actual, Is.EqualTo(new Card(value, suit)).Using(_cardComparer));
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
			var otherCards = CardTestCaseData.AllCards.Where(x => _cardComparer.Equals(x, new Card(value, suit)) == false).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x).Using(_cardComparer)));
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
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex = 33;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex);

			var randomCardToTakeFromDeck = _instance.Cards[randomIndex];

			//act
			_instance.TakeRandomCard();

			//assert
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCardToTakeFromDeck).Using(_cardComparer));
		}

		[Test]
		public void TakeRandomCard_SHOULD_return_random_card()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex = 13;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex);

			var randomCardToTakeFromDeck = _instance.Cards[randomIndex];

			//act
			var actual = _instance.TakeRandomCard();

			//assert
			Assert.That(actual, Is.EqualTo(randomCardToTakeFromDeck).Using(_cardComparer));
		}

		[Test]
		public void TakeRandomCard_SHOULD_leave_other_cards_in_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex = 44;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex);

			var randomCardToTakeFromDeck = _instance.Cards[randomIndex];

			//act
			_instance.TakeRandomCard();

			//assert
			var otherCards = CardTestCaseData.AllCards.Where(x => _cardComparer.Equals(x, randomCardToTakeFromDeck) == false).ToList();
			otherCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x).Using(_cardComparer)));
		}

		#endregion

		#region TakeRandomCards

		[Test]
		public void TakeRandomCards_WHERE_trying_to_take_more_cards_than_there_are_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance = new Deck(new List<Card>
			{
				new Card(CardValue.Queen, CardSuit.Spades),
				new Card(CardValue.Eight, CardSuit.Diamonds)
			});

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.TakeRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot take more cards than there are left in the deck"));
		}

		[Test]
		public void TakeRandomCards_SHOULD_remove_random_cards_from_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex1 = 22;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 4;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 1)).Return(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 32;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 2)).Return(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			_instance.TakeRandomCards(3);

			//assert
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCard1ToTakeFromDeck).Using(_cardComparer));
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCard2ToTakeFromDeck).Using(_cardComparer));
			Assert.That(_instance.Cards, Has.None.EqualTo(randomCard3ToTakeFromDeck).Using(_cardComparer));
		}

		[Test]
		public void TakeRandomCards_SHOULD_return_random_cards()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex1 = 44;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 25;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 1)).Return(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 49;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 2)).Return(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			var actual = _instance.TakeRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.One.EqualTo(randomCard1ToTakeFromDeck).Using(_cardComparer));
			Assert.That(actual, Has.One.EqualTo(randomCard2ToTakeFromDeck).Using(_cardComparer));
			Assert.That(actual, Has.One.EqualTo(randomCard3ToTakeFromDeck).Using(_cardComparer));
		}

		[Test]
		public void TakeRandomCards_SHOULD_leave_other_cards_in_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex1 = 13;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 11;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 1)).Return(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 19;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 2)).Return(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			_instance.TakeRandomCards(3);

			//assert
			var otherCards = CardTestCaseData.AllCards.Where(x => _cardComparer.Equals(x, randomCard1ToTakeFromDeck) &&
																  _cardComparer.Equals(x, randomCard2ToTakeFromDeck) &&
																  _cardComparer.Equals(x, randomCard3ToTakeFromDeck)).ToList();
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
				new Card(CardValue.Seven, CardSuit.Hearts),
				new Card(CardValue.Two, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.GetRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot get more cards than there are left in the deck"));
		}

		[Test]
		public void GetRandomCards_SHOULD_not_remove_any_cards_from_the_deck()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(17);
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 1)).Return(2);
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 2)).Return(29);

			//act
			_instance.GetRandomCards(3);

			//assert
			CardTestCaseData.AllCards.ForEach(x => Assert.That(_instance.Cards, Has.One.EqualTo(x).Using(_cardComparer)));
		}

		[Test]
		public void GetRandomCards_SHOULD_return_random_cards()
		{
			//arrange
			_instance = new Deck(_randomNumberGenerator);

			const int randomIndex1 = 6;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count)).Return(randomIndex1);
			var randomCard1ToTakeFromDeck = _instance.Cards[randomIndex1];

			const int randomIndex2 = 5;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 1)).Return(randomIndex2);
			var randomCard2ToTakeFromDeck = _instance.Cards[randomIndex2];

			const int randomIndex3 = 10;
			_randomNumberGenerator.Stub(x => x.Next(_instance.Cards.Count - 2)).Return(randomIndex3);
			var randomCard3ToTakeFromDeck = _instance.Cards[randomIndex3 + 2];

			//act
			var actual = _instance.GetRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.One.EqualTo(randomCard1ToTakeFromDeck).Using(_cardComparer));
			Assert.That(actual, Has.One.EqualTo(randomCard2ToTakeFromDeck).Using(_cardComparer));
			Assert.That(actual, Has.One.EqualTo(randomCard3ToTakeFromDeck).Using(_cardComparer));
		}

		#endregion

		#endregion
	}
}
