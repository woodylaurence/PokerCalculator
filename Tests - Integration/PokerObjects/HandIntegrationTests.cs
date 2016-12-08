using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandIntegrationTests
	{
		Hand _instance;
		
		[SetUp]
		public void Setup()
		{
			_instance = Hand.Create();
		}

		#region Properties and Fields

		[Test]
		public void Rank_get_WHERE_backing_field_already_set_SHOULD_return_value_of_backing_field()
		{
			//arrange
			var handRank = HandRank.Create(PokerHand.Pair);
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
			_instance.AddCard(Card.Create(CardValue.Queen, CardSuit.Diamonds));
			_instance.AddCard(Card.Create(CardValue.Queen, CardSuit.Clubs));

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
			var handRank = HandRank.Create(PokerHand.Straight);

			//act
			_instance.Rank = handRank;

			//assert
			Assert.That(_instance.Rank, Is.EqualTo(handRank));
			Assert.That(_instance._rank, Is.EqualTo(handRank));
		}

		#endregion

		#region Create

		[Test]
		public void Create_WHERE_no_cards_supplied_SHOULD_create_empty_hand()
		{
			//act
			var actual = Hand.Create();

			//assert
			Assert.That(actual.Cards, Is.Empty);
		}

		[Test]
		public void Create_WHERE_empty_card_list_supplied_SHOULD_create_empty_hand()
		{
			//act
			var actual = Hand.Create(new List<Card>());

			//assert
			Assert.That(actual.Cards, Is.Empty);
		}

		[Test]
		public void Create_WHERE_cards_supplied_SHOULD_create_hand_with_those_cards()
		{
			//arrange
			var card1 = Card.Create(CardValue.Five, CardSuit.Clubs);
			var card2 = Card.Create(CardValue.Queen, CardSuit.Diamonds);
			var card3 = Card.Create(CardValue.Eight, CardSuit.Spades);

			//act
			var actual = Hand.Create(new List<Card> { card1, card2, card3 });

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
			var card1 = Card.Create(CardValue.Five, CardSuit.Clubs);
			var card2 = Card.Create(CardValue.Queen, CardSuit.Diamonds);
			var card3 = Card.Create(CardValue.Eight, CardSuit.Spades);

			var cards = new List<Card> { card1, card2, card3 };
			var actual = Hand.Create(cards);

			//act
			var cardAddedAfterHandCreated = Card.Create(CardValue.Seven, CardSuit.Clubs);
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
			var cardToAdd = Card.Create(CardValue.Four, CardSuit.Hearts);
			_instance.Rank = HandRank.Create(PokerHand.Flush);

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
			var initialCard = Card.Create(CardValue.Ten, CardSuit.Diamonds);
			_instance.AddCard(initialCard);
			_instance.Rank = HandRank.Create(PokerHand.ThreeOfAKind);

			var cardToAdd = Card.Create(CardValue.Seven, CardSuit.Spades);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(2));
			Assert.That(_instance.Cards, Has.Some.EqualTo(initialCard));
			Assert.That(_instance.Cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(_instance._rank, Is.Null);
		}

		#endregion

		/*
		 * 
		 * Not sure whether it's worth having these in as integration tests, they're helper methods and can probably
		 * be moved to unit tests.

		#region IsFlush

		[Test]
		public void IsFlush_WHERE_more_than_five_cards_of_same_suit_SHOULD_return_true()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Hearts, CardValue.King);
			var card2 = Card.Create(CardSuit.Hearts, CardValue.Four);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Five);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card5 = Card.Create(CardSuit.Hearts, CardValue.Ace);
			var card6 = Card.Create(CardSuit.Hearts, CardValue.Nine);
			var card7 = Card.Create(CardSuit.Hearts, CardValue.Jack);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5, card6, card7 });

			//act
			var actual = _instance.IsFlush();

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsFlush_WHERE_five_cards_of_same_suit_SHOULD_return_true()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Hearts, CardValue.King);
			var card2 = Card.Create(CardSuit.Hearts, CardValue.Four);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Five);
			var card4 = Card.Create(CardSuit.Diamonds, CardValue.Ace);
			var card5 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card6 = Card.Create(CardSuit.Hearts, CardValue.Ace);


			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5, card6 });

			//act
			var actual = _instance.IsFlush();

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsFlush_WHERE_not_enough_cards_for_flush_SHOULD_return_false()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Clubs, CardValue.King);
			var card2 = Card.Create(CardSuit.Hearts, CardValue.Four);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Five);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card5 = Card.Create(CardSuit.Hearts, CardValue.Ace);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5 });

			//act
			var actual = _instance.IsFlush();

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion

		#region GetOrderedFlushValues

		[Test]
		public void GetOrderedFlushValues()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Hearts, CardValue.Eight);
			var card2 = Card.Create(CardSuit.Hearts, CardValue.Four);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Ten);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Six);
			var card5 = Card.Create(CardSuit.Hearts, CardValue.King);
			var card6 = Card.Create(CardSuit.Spades, CardValue.Four);
			var card7 = Card.Create(CardSuit.Hearts, CardValue.Ace);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5, card6, card7 });

			//act
			var actual = _instance.GetOrderedFlushValues();

			//assert
			Assert.That(actual, Has.Count.EqualTo(6));
			Assert.That(actual [0], Is.EqualTo(card7.Value));
			Assert.That(actual [1], Is.EqualTo(card5.Value));
			Assert.That(actual [2], Is.EqualTo(card3.Value));
			Assert.That(actual [3], Is.EqualTo(card1.Value));
			Assert.That(actual [4], Is.EqualTo(card4.Value));
			Assert.That(actual [5], Is.EqualTo(card2.Value));
		}

		#endregion

		#region IsStraight

		[Test]
		public void IsStraight_WHERE_contains_straight_using_ace_as_low_value_SHOULD_return_true()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Hearts, CardValue.Ace);
			var card2 = Card.Create(CardSuit.Diamonds, CardValue.Two);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Three);
			var card4 = Card.Create(CardSuit.Clubs, CardValue.Four);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Five);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5 });

			//act
			var actual = _instance.IsStraight();

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsStraight_WHERE_contains_straight_of_more_than_five_cards_SHOULD_return_true()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Diamonds, CardValue.Five);
			var card2 = Card.Create(CardSuit.Clubs, CardValue.Six);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Eight);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Nine);
			var card6 = Card.Create(CardSuit.Clubs, CardValue.Ten);
			var card7 = Card.Create(CardSuit.Clubs, CardValue.Jack);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5, card6, card7 });

			//act
			var actual = _instance.IsStraight();

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsStraight_WHERE_contains_straight_but_cards_in_wrong_order_SHOULD_return_true()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Hearts, CardValue.Eight);
			var card2 = Card.Create(CardSuit.Diamonds, CardValue.Five);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card4 = Card.Create(CardSuit.Clubs, CardValue.Nine);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Six);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5 });

			//act
			var actual = _instance.IsStraight();

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsStraight_WHERE_hand_contains_wrap_around_straight_SHOULD_return_false()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Diamonds, CardValue.Two);
			var card2 = Card.Create(CardSuit.Clubs, CardValue.Three);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Queen);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.King);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Ace);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5 });

			//act
			var actual = _instance.IsStraight();

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsStraight_WHERE_hand_does_not_contain_straight_SHOULD_return_false()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Diamonds, CardValue.Five);
			var card2 = Card.Create(CardSuit.Clubs, CardValue.Six);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Eight);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Ten);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5 });

			//act
			var actual = _instance.IsStraight();

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsStraight_WHERE_not_enough_cards_for_straight_SHOULD_return_false()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Diamonds, CardValue.Five);
			var card2 = Card.Create(CardSuit.Clubs, CardValue.Six);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Eight);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4 });

			//act
			var actual = _instance.IsStraight();

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion

		#region GetStraightHighestValue

		[Test]
		public void GetStraightHighestValue_WHERE_ace_low_straight_SHOULD_return_straight_high_value_not_ace()
		{
			//arrange
			var card1 = Card.Create(CardSuit.Diamonds, CardValue.Ace);
			var card2 = Card.Create(CardSuit.Clubs, CardValue.Two);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Three);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Four);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Five);
			var card6 = Card.Create(CardSuit.Hearts, CardValue.Six);
			var card7 = Card.Create(CardSuit.Clubs, CardValue.Seven);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5, card6, card7 });

			//act
			var actual = _instance.GetStraightHighestValue();

			//assert
			Assert.That(actual, Is.EqualTo(CardValue.Seven));
		}

		[Test]
		public void GetStraightHighestValue_WHERE_straight_start_is_not_highest_value_card_SHOULD_return_highest_value_card_belonging_to_straight()
		{

			//arrange
			var card1 = Card.Create(CardSuit.Diamonds, CardValue.Five);
			var card2 = Card.Create(CardSuit.Clubs, CardValue.Six);
			var card3 = Card.Create(CardSuit.Hearts, CardValue.Seven);
			var card4 = Card.Create(CardSuit.Hearts, CardValue.Eight);
			var card5 = Card.Create(CardSuit.Clubs, CardValue.Nine);
			var card6 = Card.Create(CardSuit.Hearts, CardValue.Four);
			var card7 = Card.Create(CardSuit.Clubs, CardValue.Jack);

			_instance = Hand.Create(new List<Card> { card1, card2, card3, card4, card5, card6, card7 });

			//act
			var actual = _instance.GetStraightHighestValue();

			//assert
			Assert.That(actual, Is.EqualTo(Value.Nine));
		}

		#endregion

		*/
	}
}
