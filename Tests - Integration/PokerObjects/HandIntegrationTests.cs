using Castle.MicroKernel.Registration;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandIntegrationTests : LocalTestBase
	{
		private Hand _instance;

		[SetUp]
		public new void Setup()
		{
			WindsorContainer.Register(Component.For<IHandRankCalculator>().ImplementedBy<Domain.HandRankCalculator.HandRankCalculator>());

			_instance = new Hand(new List<Card>());
		}

		#region Properties and Fields

		[Test]
		public void Rank_get_WHERE_backing_field_already_set_SHOULD_return_value_of_backing_field()
		{
			//arrange
			var handRank = new HandRank(PokerHand.Pair);
			_instance._rank = handRank;

			//act
			var actual = _instance.Rank;

			//assert
			Assert.That(actual, Is.EqualTo(handRank));
		}

		[Test]
		public void Rank_get_WHERE_backing_field_is_null_SHOULD_calculate_value_and_set_backing_field_and_return_rank()
		{
			//arrange
			_instance._rank = null;
			_instance.AddCard(new Card(CardValue.Queen, CardSuit.Diamonds));
			_instance.AddCard(new Card(CardValue.Queen, CardSuit.Clubs));

			//act
			var actual = _instance.Rank;

			//assert
			Assert.That(actual, Is.Not.Null);
			Assert.That(_instance._rank, Is.Not.Null);
		}

		[Test]
		public void Rank_set_SHOULD_set_backing_field_value()
		{
			//arrange
			var handRank = new HandRank(PokerHand.Straight);

			//act
			_instance.Rank = handRank;

			//assert
			Assert.That(_instance.Rank, Is.EqualTo(handRank));
			Assert.That(_instance._rank, Is.EqualTo(handRank));
		}

		#endregion

		#region Constructor

		[Test]
		public void Constructor_WHERE_empty_card_list_supplied_SHOULD_create_empty_hand()
		{
			//act
			var actual = new Hand(new List<Card>());

			//assert
			Assert.That(actual.Cards, Is.Empty);
		}

		[Test]
		public void Constructor_WHERE_cards_supplied_SHOULD_create_hand_with_those_cards()
		{
			//arrange
			var card1 = new Card(CardValue.Five, CardSuit.Clubs);
			var card2 = new Card(CardValue.Queen, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Eight, CardSuit.Spades);

			//act
			var actual = new Hand(new List<Card> { card1, card2, card3 });

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
		}

		[Test]
		public void Create_SHOULD_assign_new_list_rather_than_doing_memory_copy_to_stop_changes_to_list_later_affecting_hand()
		{
			//arrange
			var card1 = new Card(CardValue.Five, CardSuit.Clubs);
			var card2 = new Card(CardValue.Queen, CardSuit.Diamonds);
			var card3 = new Card(CardValue.Eight, CardSuit.Spades);

			var cards = new List<Card> { card1, card2, card3 };
			var actual = new Hand(cards);

			//act
			var cardAddedAfterHandCreated = new Card(CardValue.Seven, CardSuit.Clubs);
			cards.Add(cardAddedAfterHandCreated);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
			Assert.That(actual.Cards, Has.None.EqualTo(cardAddedAfterHandCreated));
		}

		#endregion

		#region AddCard

		[Test]
		public void AddCard_WHERE_hand_is_initially_empty_SHOULD_add_card()
		{
			//arrange
			var cardToAdd = new Card(CardValue.Four, CardSuit.Hearts);
			_instance.Rank = new HandRank(PokerHand.Flush);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(1));
			Assert.That(_instance.Cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(_instance._rank, Is.Null);
		}

		[Test]
		public void AddCard_WHERE_hand_already_has_cards_SHOULD_add_card_and_maintain_existing_cards()
		{
			//arrange
			var initialCard = new Card(CardValue.Ten, CardSuit.Diamonds);
			_instance.AddCard(initialCard);
			_instance.Rank = new HandRank(PokerHand.ThreeOfAKind);

			var cardToAdd = new Card(CardValue.Seven, CardSuit.Spades);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(2));
			Assert.That(_instance.Cards, Has.Some.EqualTo(initialCard));
			Assert.That(_instance.Cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(_instance._rank, Is.Null);
		}

		#endregion
	}
}
