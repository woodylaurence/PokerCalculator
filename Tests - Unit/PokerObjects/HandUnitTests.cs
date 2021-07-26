using Castle.MicroKernel.Registration;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
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

			_cardComparer = new CardComparer();
			_instance = new Hand(new List<Card>(), _cardComparer);

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
		public void Constructor_cards_WHERE_more_than_seven_cards_in_supplied_cards_SHOULD_throw_error()
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
		public void Constructor_cards_WHERE_cards_contains_duplicates_SHOULD_throw_error()
		{
			//arrange
			const CardValue cardValue = CardValue.Nine;
			const CardSuit cardSuit = CardSuit.Clubs;
			var duplicatedCard1 = new Card(cardValue, cardSuit);
			var duplicatedCard2 = new Card(cardValue, cardSuit);
			var card3 = new Card(CardValue.Eight, CardSuit.Hearts);

			var cards = new List<Card> { duplicatedCard1, card3, duplicatedCard2 };

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => new Hand(cards, _cardComparer));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards\r\nParameter name: cards"));
			Assert.That(actualException.ParamName, Is.EqualTo("cards"));
		}

		[Test]
		public void Constructor_cards_SHOULD_copy_supplied_cards_to_Cards_property()
		{
			//arrange
			var card1 = new Card(CardValue.Six, CardSuit.Hearts);
			var card2 = new Card(CardValue.Ten, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Ace, CardSuit.Diamonds);

			var cards = new List<Card> { card1, card2, card3 };

			//act
			var actual = new Hand(cards, _cardComparer);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards[0], Is.EqualTo(card1));
			Assert.That(actual.Cards[1], Is.EqualTo(card2));
			Assert.That(actual.Cards[2], Is.EqualTo(card3));

			var cardAddedToOriginalCardList = new Card(CardValue.Ten, CardSuit.Hearts);
			cards.Add(cardAddedToOriginalCardList);
			Assert.That(actual.Cards, Has.None.EqualTo(cardAddedToOriginalCardList));
		}

		#endregion

		#region Instance Methods

		#region AddCard

		[Test]
		public void AddCard_WHERE_hand_already_has_7_cards_SHOULD_throw_error()
		{
			//arrange
			_instance.AddCards(new List<Card>
			{
				new Card(CardValue.Ace, CardSuit.Diamonds),
				new Card(CardValue.Two, CardSuit.Diamonds),
				new Card(CardValue.Three, CardSuit.Diamonds),
				new Card(CardValue.Four, CardSuit.Diamonds),
				new Card(CardValue.Five, CardSuit.Diamonds),
				new Card(CardValue.Six, CardSuit.Diamonds),
				new Card(CardValue.Seven, CardSuit.Diamonds),
			});

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.AddCard(new Card(CardValue.Eight, CardSuit.Diamonds)));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot have more than seven cards"));
		}

		[Test]
		public void AddCard_WHERE_hand_already_contains_card_being_added_SHOULD_throw_error()
		{
			//arrange
			_instance.AddCard(new Card(CardValue.Seven, CardSuit.Diamonds));

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.AddCard(new Card(CardValue.Seven, CardSuit.Diamonds)));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void AddCard_SHOULD_add_card()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Four, CardSuit.Hearts);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(1));
			Assert.That(_instance.Cards, Has.One.EqualTo(cardToAdd));
		}

		[Test]
		public void AddCard_SHOULD_leave_existing_cards()
		{
			//arrange
			var existingCard1 = new Card(CardValue.Four, CardSuit.Hearts);
			var existingCard2 = new Card(CardValue.Five, CardSuit.Diamonds);
			_instance = new Hand(new List<Card> { existingCard1, existingCard2 });

			//act
			_instance.AddCard(new Card(CardValue.Queen, CardSuit.Spades));

			//assert
			Assert.That(_instance.Cards, Has.One.EqualTo(existingCard1));
			Assert.That(_instance.Cards, Has.One.EqualTo(existingCard2));
		}

		#endregion

		#region AddCards

		[Test]
		public void AddCards_WHERE_adding_number_of_cards_which_would_take_total_number_of_cards_in_hand_over_seven_SHOULD_throw_error()
		{
			//arrange
			_instance.AddCards(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Diamonds),
				new Card(CardValue.Ten, CardSuit.Hearts),
				new Card(CardValue.Seven, CardSuit.Hearts)
			});

			var cardsToAdd = new List<Card>
			{
				new Card(CardValue.Ten, CardSuit.Diamonds),
				new Card(CardValue.Two, CardSuit.Spades),
				new Card(CardValue.Three, CardSuit.Clubs),
				new Card(CardValue.King, CardSuit.Clubs),
				new Card(CardValue.Ace, CardSuit.Hearts)
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.AddCards(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot have more than seven cards"));
		}

		[Test]
		public void AddCards_WHERE_adding_cards_which_are_already_in_hand_SHOULD_throw_error()
		{
			//arrange
			_instance.AddCards(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Diamonds),
				new Card(CardValue.Ten, CardSuit.Hearts),
				new Card(CardValue.Seven, CardSuit.Hearts)
			});

			var cardsToAdd = new List<Card>
			{
				new Card(CardValue.Two, CardSuit.Spades),
				new Card(CardValue.Eight, CardSuit.Diamonds),
				new Card(CardValue.Ten, CardSuit.Hearts)
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.AddCards(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void AddCards_WHERE_cards_to_be_added_contains_duplicates_SHOULD_throw_error()
		{
			//arrange
			_instance.AddCards(new List<Card>
			{
				new Card(CardValue.Ten, CardSuit.Spades),
				new Card(CardValue.Four, CardSuit.Diamonds),
			});

			var cardsToAdd = new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Diamonds),
				new Card(CardValue.Ten, CardSuit.Hearts),
				new Card(CardValue.Eight, CardSuit.Diamonds)
			};

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.AddCards(cardsToAdd));
			Assert.That(actualException.Message, Is.EqualTo("A Hand cannot contain duplicate cards"));
		}

		[Test]
		public void AddCards_SHOULD_add_cards()
		{
			//arrange
			var cardToAdd1 = new Card(CardValue.Four, CardSuit.Spades);
			var cardToAdd2 = new Card(CardValue.Ten, CardSuit.Hearts);
			var cardToAdd3 = new Card(CardValue.Eight, CardSuit.Diamonds);
			var cardsToAdd = new List<Card> { cardToAdd1, cardToAdd2, cardToAdd3 };

			//act
			_instance.AddCards(cardsToAdd);

			//assert
			Assert.That(_instance.Cards, Has.One.EqualTo(cardToAdd1));
			Assert.That(_instance.Cards, Has.One.EqualTo(cardToAdd2));
			Assert.That(_instance.Cards, Has.One.EqualTo(cardToAdd3));
		}

		[Test]
		public void AddCards_SHOULD_leave_existing_cards()
		{
			//arrange
			var existingCard1 = new Card(CardValue.Ten, CardSuit.Spades);
			var existingCard2 = new Card(CardValue.Two, CardSuit.Hearts);
			_instance.AddCards(new List<Card> { existingCard1, existingCard2 });

			var cardsToAdd = new List<Card>
			{
				new Card(CardValue.Four, CardSuit.Spades),
				new Card(CardValue.Ten, CardSuit.Hearts)
			};

			//act
			_instance.AddCards(cardsToAdd);

			//assert
			Assert.That(_instance.Cards, Has.One.EqualTo(existingCard1));
			Assert.That(_instance.Cards, Has.One.EqualTo(existingCard2));
		}

		#endregion

		#endregion

		#region Operator Overloads

		#region + Overload

		[Test]
		public void Addition_overload_SHOULD_combine_cards_from_both_hands()
		{
			//arrange
			var hand1Card1 = new Card(CardValue.Three, CardSuit.Diamonds);
			var hand1Card2 = new Card(CardValue.Seven, CardSuit.Hearts);
			_instance.AddCards(new List<Card> { hand1Card1, hand1Card2 });

			var hand2Card1 = new Card(CardValue.Eight, CardSuit.Diamonds);
			var hand2Card2 = new Card(CardValue.Four, CardSuit.Hearts);
			var hand2 = new Hand(new List<Card> { hand2Card1, hand2Card2 });

			//act
			var actual = _instance + hand2;

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			Assert.That(actual.Cards, Has.One.EqualTo(hand1Card1));
			Assert.That(actual.Cards, Has.One.EqualTo(hand1Card2));
			Assert.That(actual.Cards, Has.One.EqualTo(hand2Card1));
			Assert.That(actual.Cards, Has.One.EqualTo(hand2Card2));
		}

		[Test]
		public void Addition_overload_SHOULD_create_new_hand()
		{
			//arrange
			_instance.AddCards(new List<Card>
			{
				new Card(CardValue.Three, CardSuit.Diamonds),
				new Card(CardValue.Seven, CardSuit.Hearts)
			});

			var hand2 = new Hand(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Diamonds),
				new Card(CardValue.Four, CardSuit.Hearts)
			});

			//act
			var actual = _instance + hand2;

			//assert
			Assert.That(actual, Is.Not.SameAs(_instance));
			Assert.That(actual, Is.Not.SameAs(hand2));
		}

		[Test]
		public void Addition_overload_SHOULD_maintain_card_list_in_a_new_memory_location()
		{
			//arrange
			_instance.AddCards(new List<Card>
			{
				new Card(CardValue.Three, CardSuit.Diamonds),
				new Card(CardValue.Seven, CardSuit.Hearts)
			});

			var hand2 = new Hand(new List<Card>
			{
				new Card(CardValue.Eight, CardSuit.Diamonds),
				new Card(CardValue.Four, CardSuit.Hearts)
			});

			//act
			var actual = _instance + hand2;

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(_instance.Cards));
			Assert.That(actual.Cards, Is.Not.SameAs(hand2.Cards));
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
			_instance.AddCards(handCards);

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

		#endregion

		#endregion
	}
}
