using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Integration.HandRankCalculator
{
	[TestFixture]
	public abstract class BaseHandRankCalculatorIntegrationTests : LocalTestBase
	{
		protected IHandRankCalculator _instance;
		private Hand _hand;

		#region CalculateHandRank

		#region Royal Flush

		[Test]
		public void CalculateHandRank_WHERE_royal_flush_SHOULD_return_straight_flush_with_empty_tie_breaker_values()
		{
			//	  ROYAL FLUSH	 -	 OTHERS
			// {AS KS QS JS 10S} - {4H} - {7H}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Ace, CardSuit.Spades),
				Card.Create(CardValue.Jack, CardSuit.Spades),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.Queen, CardSuit.Spades),
				Card.Create(CardValue.King, CardSuit.Spades),
				Card.Create(CardValue.Four, CardSuit.Hearts)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.RoyalFlush));
			Assert.That(actual.KickerCardValues, Is.Empty);
		}

		#endregion

		#region Straight Flush

		[Test]
		public void CalculateHandRank_WHERE_straight_flush_SHOULD_return_straight_flush_with_primary_value_set_to_highest_straight_value()
		{
			//	STRAIGHT FLUSH	 -	 OTHERS
			// {8H 7H 6H 5H 4H} - {10S} - {2C}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Four, CardSuit.Hearts),
				Card.Create(CardValue.Six, CardSuit.Hearts),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Eight, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.Five, CardSuit.Hearts),
				Card.Create(CardValue.Two, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.StraightFlush));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Eight));
		}

		[Test]
		public void CalculateHandRank_WHERE_straight_flush_with_higher_extra_card_of_same_suit_SHOULD_return_straight_flush_with_primary_value_set_to_highest_straight_value()
		{
			//	STRAIGHT FLUSH	 -	 OTHERS
			// {JH 10H 9H 8H 7H} - {AH} - {4H}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Ten, CardSuit.Hearts),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Ace, CardSuit.Hearts),
				Card.Create(CardValue.Nine, CardSuit.Hearts),
				Card.Create(CardValue.Jack, CardSuit.Hearts),
				Card.Create(CardValue.Eight, CardSuit.Hearts),
				Card.Create(CardValue.Four, CardSuit.Hearts)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.StraightFlush));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Jack));

		}

		[Test]
		public void CalculateHandRank_WHERE_straight_flush_with_higher_extra_card_of_different_suit_SHOULD_return_straight_flush_with_primary_value_set_to_highest_straight_flush_value()
		{
			//	STRAIGHT FLUSH	 -	 OTHERS
			// {JH 10H 9H 8H 7H} - {QC} - {2D}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Ten, CardSuit.Hearts),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Queen, CardSuit.Clubs),
				Card.Create(CardValue.Nine, CardSuit.Hearts),
				Card.Create(CardValue.Jack, CardSuit.Hearts),
				Card.Create(CardValue.Eight, CardSuit.Hearts),
				Card.Create(CardValue.Two, CardSuit.Diamonds)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.StraightFlush));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Jack));
		}

		#endregion

		#region Four of a Kind

		[Test]
		public void CalculateHandRank_WHERE_four_of_a_kind_SHOULD_return_four_of_a_kind_with_primary_value_set_to_four_of_a_kind_value()
		{
			//	 FOUR OF A KIND	 -	 OTHERS
			// {10C 10D 10H 10S} - {9D 6H 3S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Nine, CardSuit.Diamonds),
				Card.Create(CardValue.Ten, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.Three, CardSuit.Spades),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Ten, CardSuit.Clubs),
				Card.Create(CardValue.Six, CardSuit.Hearts)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FourOfAKind));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Nine));
		}

		[Test]
		public void CalculateHandRank_WHERE_four_of_a_kind_and_higher_three_of_a_kind_SHOULD_return_four_of_a_kind_with_primary_value_set_to_four_of_a_kind_value()
		{
			//	 FOUR OF A KIND	 -	 OTHERS
			// {10C 10D 10H 10S} - {KC KH KS}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.King, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.King, CardSuit.Spades),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Ten, CardSuit.Clubs),
				Card.Create(CardValue.King, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FourOfAKind));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.King));
		}

		#endregion

		#region Full House

		[Test]
		public void CalculateHandRank_WHERE_full_house_SHOULD_return_full_house_with_primary_value_set_to_three_of_a_kind_value_and_secondary_value_set_to_pair_value()
		{
			//	    FULL HOUSE	  -  OTHERS
			// {7C 7D 7H} {2H 2C} - {9D 4C}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Seven, CardSuit.Clubs),
				Card.Create(CardValue.Four, CardSuit.Clubs),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Nine, CardSuit.Diamonds),
				Card.Create(CardValue.Two, CardSuit.Clubs),
				Card.Create(CardValue.Two, CardSuit.Hearts),
				Card.Create(CardValue.Seven, CardSuit.Diamonds)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Seven));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Two));
		}

		[Test]
		public void CalculateHandRank_WHERE_one_three_of_a_kind_and_two_pairs_SHOULD_return_full_house_with_primary_value_set_to_three_of_a_kind_value_and_secondary_value_set_to_pair_value()
		{
			//	  	 FULL HOUSE		-  OTHERS
			// {8C 8S 8H} {10C 10S} - {4D 4C}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Eight, CardSuit.Clubs),
				Card.Create(CardValue.Four, CardSuit.Clubs),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.Eight, CardSuit.Spades),
				Card.Create(CardValue.Four, CardSuit.Diamonds),
				Card.Create(CardValue.Eight, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Eight));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Ten));
		}

		[Test]
		public void CalculateHandRank_WHERE_two_three_of_a_kinds_and_high_extra_card_SHOULD_return_full_house_with_primary_value_set_to_higher_three_of_a_kind_and_secondary_value_set_to_lower_three_of_a_kind_value()
		{
			//	  	 FULL HOUSE		  -  OTHERS
			// {10D 10C 10S} {8C 8S}  - {8H AD}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Eight, CardSuit.Clubs),
				Card.Create(CardValue.Ace, CardSuit.Diamonds),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.Eight, CardSuit.Spades),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Eight, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Eight));
		}

		[Test]
		public void CalculateHandRank_WHERE_two_three_of_a_kinds_and_low_extra_card_SHOULD_return_full_house_with_primary_value_set_to_higher_three_of_a_kind_and_secondary_and_tertiary_values_set_to_lower_three_of_a_kind()
		{
			//	  	 FULL HOUSE		  -  OTHERS
			// {10D 10C 10S} {8C 8S}  - {8H 5D}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Eight, CardSuit.Clubs),
				Card.Create(CardValue.Ten, CardSuit.Spades),
				Card.Create(CardValue.Eight, CardSuit.Spades),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Eight, CardSuit.Hearts),
				Card.Create(CardValue.Five, CardSuit.Diamonds),
				Card.Create(CardValue.Ten, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Eight));
		}

		#endregion

		#region Flush

		[Test]
		public void CalculateHandRank_WHERE_flush_SHOULD_return_flush_with_values_set_to_flush_descending_values()
		{
			//	  	 FLUSH		 -  OTHERS
			// {KC 9C 7C 3C 2C}  - {4S AD}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.King, CardSuit.Clubs),
				Card.Create(CardValue.Two, CardSuit.Clubs),
				Card.Create(CardValue.Four, CardSuit.Spades),
				Card.Create(CardValue.Seven, CardSuit.Clubs),
				Card.Create(CardValue.Three, CardSuit.Clubs),
				Card.Create(CardValue.Ace, CardSuit.Diamonds),
				Card.Create(CardValue.Nine, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Flush));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.King));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Nine));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Seven));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(CardValue.Three));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(CardValue.Two));
		}

		[Test]
		public void CalculateHandRank_WHERE_flush_and_straight_but_not_straight_flush_SHOULD_return_flush_with_values_set_to_flush_descending_values()
		{
			//	  	 FLUSH		  -  OTHERS
			// {10C 9C 7C 6C 2C}  - {JS 8D}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Six, CardSuit.Clubs),
				Card.Create(CardValue.Nine, CardSuit.Clubs),
				Card.Create(CardValue.Eight, CardSuit.Diamonds),
				Card.Create(CardValue.Jack, CardSuit.Spades),
				Card.Create(CardValue.Ten, CardSuit.Clubs),
				Card.Create(CardValue.Two, CardSuit.Clubs),
				Card.Create(CardValue.Seven, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Flush));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Nine));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Seven));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(CardValue.Six));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(CardValue.Two));
		}

		[Test]
		public void CalculateHandRank_WHERE_flush_and_three_of_a_kind_SHOULD_return_flush_with_values_set_to_flush_descending_values()
		{
			//	  	 FLUSH		  -  OTHERS
			// {AC 10C 8C 7C 5C}  - {10H 10D}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Seven, CardSuit.Clubs),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Ten, CardSuit.Clubs),
				Card.Create(CardValue.Ten, CardSuit.Hearts),
				Card.Create(CardValue.Five, CardSuit.Clubs),
				Card.Create(CardValue.Eight, CardSuit.Clubs),
				Card.Create(CardValue.Ace, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Flush));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ace));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Eight));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(CardValue.Seven));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(CardValue.Five));
		}

		#endregion

		#region Straight

		[Test]
		public void CalculateHandRank_WHERE_straight_SHOULD_return_straight_with_primary_value_set_to_highest_straight_value()
		{
			//	   STRAIGHT		  -  OTHERS
			// {KS QD JH 10C 9C}  - {2D 6H}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Queen, CardSuit.Diamonds),
				Card.Create(CardValue.Two, CardSuit.Diamonds),
				Card.Create(CardValue.King, CardSuit.Spades),
				Card.Create(CardValue.Nine, CardSuit.Clubs),
				Card.Create(CardValue.Six, CardSuit.Hearts),
				Card.Create(CardValue.Jack, CardSuit.Hearts),
				Card.Create(CardValue.Ten, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Straight));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.King));
		}

		[Test]
		public void CalculateHandRank_WHERE_straight_with_ace_low_SHOULD_return_straight_with_primary_value_not_set_to_ace()
		{
			//	   STRAIGHT		 -  OTHERS
			// {5H 4D 3S 2H AC}  - {10D 7H}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Three, CardSuit.Spades),
				Card.Create(CardValue.Two, CardSuit.Hearts),
				Card.Create(CardValue.Ace, CardSuit.Clubs),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Five, CardSuit.Hearts),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Four, CardSuit.Diamonds)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Straight));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Five));
		}

		[Test]
		public void CalculateHandRank_WHERE_straight_of_length_greater_than_five_SHOULD_return_straight_with_primary_value_set_to_straight_high_value()
		{
			//	   STRAIGHT		 -  OTHERS
			// {10D 9H 8D 7H 6S}  - {5H 4C}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Nine, CardSuit.Hearts),
				Card.Create(CardValue.Eight, CardSuit.Diamonds),
				Card.Create(CardValue.Six, CardSuit.Spades),
				Card.Create(CardValue.Four, CardSuit.Clubs),
				Card.Create(CardValue.Ten, CardSuit.Diamonds),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Five, CardSuit.Hearts)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Straight));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ten));
		}

		[Test]
		public void CalculateHandRank_WHERE_straight_and_three_of_a_kind_SHOULD_return_straight_with_primary_value_set_to_straight_high_value()
		{
			//	   STRAIGHT		 -  OTHERS
			// {8D 7H 6D 5H 4C}  - {6H 6S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Six, CardSuit.Diamonds),
				Card.Create(CardValue.Five, CardSuit.Hearts),
				Card.Create(CardValue.Seven, CardSuit.Hearts),
				Card.Create(CardValue.Six, CardSuit.Hearts),
				Card.Create(CardValue.Four, CardSuit.Clubs),
				Card.Create(CardValue.Six, CardSuit.Spades),
				Card.Create(CardValue.Eight, CardSuit.Diamonds)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Straight));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Eight));
		}

		#endregion

		#region Three of a Kind

		[Test]
		public void CalculateHandRank_WHERE_three_of_a_kind_SHOULD_return_ThreeOfAKind_with_primary_value_set_to_triple_value_and_secondary_value_set_to_highest_remaining_card_value_and_third_value_set_to_second_highest_remaining_card_value()
		{
			// THREE OF A KIND	-  	  OTHERS
			//   {6H 6S 6C}  	- {QC 7D 4H 3S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Six ,CardSuit.Hearts),
				Card.Create(CardValue.Seven ,CardSuit.Diamonds),
				Card.Create(CardValue.Four ,CardSuit.Hearts),
				Card.Create(CardValue.Six ,CardSuit.Spades),
				Card.Create(CardValue.Queen ,CardSuit.Diamonds),
				Card.Create(CardValue.Three ,CardSuit.Spades),
				Card.Create(CardValue.Six ,CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.ThreeOfAKind));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Six));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Queen));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Seven));
		}

		#endregion

		#region TwoPair

		[Test]
		public void CalculateHandRank_WHERE_multiple_pairs_and_highest_value_card_after_two_pairs_is_from_another_pair_SHOULD_return_TwoPair_with_primary_value_set_to_highest_pair_secondary_value_set_to_next_highest_pair_and_tertiary_value_set_to_highest_value_card_from_final_pair()
		{
			// 	 	 TWO PAIR	  -    OTHERS
			//   {AH AS} {QC QH}  -  {6C 6D 3S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Six ,CardSuit.Clubs),
				Card.Create(CardValue.Queen ,CardSuit.Hearts),
				Card.Create(CardValue.Ace ,CardSuit.Hearts),
				Card.Create(CardValue.Three ,CardSuit.Spades),
				Card.Create(CardValue.Queen ,CardSuit.Clubs),
				Card.Create(CardValue.Six ,CardSuit.Diamonds),
				Card.Create(CardValue.Ace ,CardSuit.Spades)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Ace));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Queen));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Six));
		}

		[Test]
		public void CalculateHandRank_WHERE_multiple_pairs_and_highest_value_card_after_two_pairs_is_from_card_not_in_pair_SHOULD_return_TwoPair_with_primary_value_set_to_highest_pair_secondary_value_set_to_next_highest_pair_and_tertiary_value_set_to_next_highest_card()
		{
			// 	 	 TWO PAIR	  -    OTHERS
			//   {QC QH} {6C 6D}  -  {AS 3H 3S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Six ,CardSuit.Clubs),
				Card.Create(CardValue.Ace ,CardSuit.Spades),
				Card.Create(CardValue.Queen ,CardSuit.Clubs),
				Card.Create(CardValue.Queen ,CardSuit.Hearts),
				Card.Create(CardValue.Three ,CardSuit.Hearts),
				Card.Create(CardValue.Six ,CardSuit.Diamonds),
				Card.Create(CardValue.Three ,CardSuit.Spades)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Queen));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Six));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Ace));
		}

		[Test]
		public void CalculateHandRank_WHERE_two_pairs_SHOULD_return_TwoPair_with_primary_value_set_to_highest_pair_secondary_value_set_to_next_pair_and_tertiary_set_to_highest_remaining_card()
		{
			// 	 	 TWO PAIR	  -    OTHERS
			//   {QC QH} {3H 3S}  -  {AS 8D 6C}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Three ,CardSuit.Spades),
				Card.Create(CardValue.Queen ,CardSuit.Clubs),
				Card.Create(CardValue.Three ,CardSuit.Hearts),
				Card.Create(CardValue.Six ,CardSuit.Clubs),
				Card.Create(CardValue.Queen ,CardSuit.Hearts),
				Card.Create(CardValue.Eight ,CardSuit.Diamonds),
				Card.Create(CardValue.Ace ,CardSuit.Spades)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Queen));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Three));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Ace));
		}

		#endregion

		#region Pair

		[Test]
		public void CalculateHandRank_WHERE_pair_and_pair_value_lower_than_highest_card_SHOULD_return_Pair_with_primary_value_set_to_pair_value_then_next_three_tie_breaker_values_set_to_descending_high_cards()
		{
			// 	   PAIR	  -    	  OTHERS
			//   {6H 6C}  -	 {QC JS 10C 9H 3S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Nine, CardSuit.Hearts),
				Card.Create(CardValue.Six, CardSuit.Clubs),
				Card.Create(CardValue.Jack, CardSuit.Spades),
				Card.Create(CardValue.Six, CardSuit.Hearts),
				Card.Create(CardValue.Three, CardSuit.Spades),
				Card.Create(CardValue.Ten, CardSuit.Clubs),
				Card.Create(CardValue.Queen, CardSuit.Clubs)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Pair));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(4));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Six));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Queen));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Jack));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(CardValue.Ten));
		}

		#endregion

		#region High Card

		[Test]
		public void CalculateHandRank_WHERE_highCard_SHOULD_return_HighCard_with_tie_breaker_values_set_to_descending_values()
		{
			// 	 HIGH CARD	-    	  OTHERS
			//     {QC}  	-	{JS 10C 9H 7C 6H 3S}

			//arrange
			_hand = Hand.Create(new List<Card>
			{
				Card.Create(CardValue.Ten, CardSuit.Clubs),
				Card.Create(CardValue.Three, CardSuit.Spades),
				Card.Create(CardValue.Nine, CardSuit.Hearts),
				Card.Create(CardValue.Queen, CardSuit.Clubs),
				Card.Create(CardValue.Seven, CardSuit.Clubs),
				Card.Create(CardValue.Jack, CardSuit.Spades),
				Card.Create(CardValue.Six, CardSuit.Hearts)
			});

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.HighCard));

			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(CardValue.Queen));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(CardValue.Jack));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(CardValue.Ten));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(CardValue.Nine));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(CardValue.Seven));
		}

		#endregion

		#endregion
	}
}
