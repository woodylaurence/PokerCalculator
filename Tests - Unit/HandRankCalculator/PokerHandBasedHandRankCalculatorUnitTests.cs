﻿using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.HandRankCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankCalculatorUnitTests : AbstractUnitTestBase
	{
		private PokerHandBasedHandRankCalculator _instance;
		private Hand _hand;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			var cardComparer = MockRepository.GenerateStrictMock<IEqualityComparer<Card>>();
			_instance = MockRepository.GeneratePartialMock<PokerHandBasedHandRankCalculator>();

			_hand = MockRepository.GenerateStrictMock<Hand>(new List<Card>(), cardComparer);
		}

		#region CalculateHandRank

		[Test]
		public void CalculateHandRank_WHERE_have_five_flush_values_SHOULD_return_result_of_GetFlushBasedHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			_hand.Stub(x => x.Cards).Return(cards);

			var flushCardValues = new List<CardValue>
			{
				CardValue.Six,
				CardValue.King,
				CardValue.Three,
				CardValue.Four,
				CardValue.Nine
			};

			_instance.Stub(x => x.GetFlushValues(cards)).Return(flushCardValues);

			var expected = new PokerHandBasedHandRank(PokerHand.FourOfAKind);
			_instance.Stub(x => x.GetFlushBasedHandRank(flushCardValues)).Return(expected);

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CalculateHandRank_WHERE_have_more_than_five_flush_values_SHOULD_return_result_of_GetFlushBasedHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			_hand.Stub(x => x.Cards).Return(cards);

			var flushCardValues = new List<CardValue>
			{
				CardValue.Three,
				CardValue.Jack,
				CardValue.Two,
				CardValue.Seven,
				CardValue.Four,
				CardValue.Ace
			};

			_instance.Stub(x => x.GetFlushValues(cards)).Return(flushCardValues);

			var expected = new PokerHandBasedHandRank(PokerHand.Pair);
			_instance.Stub(x => x.GetFlushBasedHandRank(flushCardValues)).Return(expected);

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CalculateHandRank_WHERE_have_less_than_five_flush_values_but_five_straight_values_SHOULD_return_straight_hand_rank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			_hand.Stub(x => x.Cards).Return(cards);

			var flushCardValues = new List<CardValue> { CardValue.Three, CardValue.Jack };
			_instance.Stub(x => x.GetFlushValues(cards)).Return(flushCardValues);

			const CardValue highestStraightValue = CardValue.King;
			var straightValues = new List<CardValue>
			{
				highestStraightValue, CardValue.Three, CardValue.Jack, CardValue.Seven, CardValue.Two
			};
			_instance.Stub(x => x.GetStraightValues(cards)).Return(straightValues);

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Straight));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(highestStraightValue));
		}

		[Test]
		public void CalculateHandRank_WHERE_have_less_than_five_flush_values_but_more_than_five_straight_valus_SHOULD_return_straight_hand_rank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			_hand.Stub(x => x.Cards).Return(cards);

			var flushCardValues = new List<CardValue> { CardValue.Three, CardValue.Jack };
			_instance.Stub(x => x.GetFlushValues(cards)).Return(flushCardValues);

			const CardValue highestStraightValue = CardValue.Ace;
			var straightValues = new List<CardValue>
			{
				highestStraightValue, CardValue.Three, CardValue.King, CardValue.Six, CardValue.Eight, CardValue.Queen
			};
			_instance.Stub(x => x.GetStraightValues(cards)).Return(straightValues);

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Straight));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(highestStraightValue));
		}

		[Test]
		public void CalculateHandRank_WHERE_have_less_than_five_flush_values_and_less_than_five_straight_valus_SHOULD_return_result_of_GetMultiCardOrHighCardHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			_hand.Stub(x => x.Cards).Return(cards);

			var flushCardValues = new List<CardValue> { CardValue.Three, CardValue.Jack };
			_instance.Stub(x => x.GetFlushValues(cards)).Return(flushCardValues);

			var straightValues = new List<CardValue> { CardValue.King, CardValue.Six };
			_instance.Stub(x => x.GetStraightValues(cards)).Return(straightValues);

			var expected = new PokerHandBasedHandRank(PokerHand.FourOfAKind);
			_instance.Stub(x => x.GetMultiCardOrHighCardHandRank(cards)).Return(expected);

			//act
			var actual = _instance.CalculateHandRank(_hand);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region GetFlushBasedHandRank

		[Test]
		public void GetFlushBasedHandRank_WHERE_hand_has_flush_and_flush_cards_are_a_straight_and_straight_highest_value_is_ace_SHOULD_return_royal_flush()
		{
			//assert
			var flushValues = new List<CardValue> { CardValue.Three, CardValue.Four };

			_instance.Stub(x => x.GetStraightValues(flushValues)).Return(new List<CardValue>
			{
				CardValue.Ace, CardValue.King, CardValue.Queen, CardValue.Jack, CardValue.Ten
			});

			//act
			var actual = _instance.GetFlushBasedHandRank(flushValues);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.RoyalFlush));
			Assert.That(actual.KickerCardValues, Is.Empty);
		}

		[Test]
		public void GetFlushBasedHandRank_WHERE_hand_has_flush_and_flush_cards_are_a_straight_and_straight_highest_value_is_not_ace_SHOULD_return_straight_flush()
		{
			//assert
			var flushValues = new List<CardValue> { CardValue.Ten, CardValue.Ace };

			const CardValue highestStraightValue = CardValue.Seven;
			_instance.Stub(x => x.GetStraightValues(flushValues)).Return(new List<CardValue>
			{
				highestStraightValue, CardValue.Four, CardValue.Nine, CardValue.King, CardValue.Eight
			});

			//act
			var actual = _instance.GetFlushBasedHandRank(flushValues);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.StraightFlush));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(highestStraightValue));
		}

		[Test]
		public void GetFlushBasedHandRank_WHERE_hand_has_flush_and_flush_cards_are_not_a_straight_and_have_more_than_five_flush_cards_SHOULD_return_flush_with_highest_five_flush_cards()
		{
			//assert
			const CardValue flushCardValue1 = CardValue.Ace;
			const CardValue flushCardValue2 = CardValue.Ten;
			const CardValue flushCardValue3 = CardValue.Nine;
			const CardValue flushCardValue4 = CardValue.Eight;
			const CardValue flushCardValue5 = CardValue.Six;
			const CardValue flushCardValue6 = CardValue.Three;
			var flushValues = new List<CardValue> { flushCardValue1, flushCardValue2, flushCardValue3, flushCardValue4, flushCardValue5, flushCardValue6 };

			_instance.Stub(x => x.GetStraightValues(flushValues)).Return(new List<CardValue>
			{
				CardValue.Nine, CardValue.Two
			});

			//act
			var actual = _instance.GetFlushBasedHandRank(flushValues);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Flush));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(flushCardValue1));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(flushCardValue2));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(flushCardValue3));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(flushCardValue4));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(flushCardValue5));
		}

		[Test]
		public void GetFlushBasedHandRank_WHERE_hand_has_flush_and_flush_cards_are_not_a_straight_and_have_five_flush_cards_SHOULD_return_flush()
		{
			//assert
			const CardValue flushCardValue1 = CardValue.Ace;
			const CardValue flushCardValue2 = CardValue.Ten;
			const CardValue flushCardValue3 = CardValue.Nine;
			const CardValue flushCardValue4 = CardValue.Eight;
			const CardValue flushCardValue5 = CardValue.Six;
			var flushValues = new List<CardValue> { flushCardValue1, flushCardValue2, flushCardValue3, flushCardValue4, flushCardValue5 };

			_instance.Stub(x => x.GetStraightValues(flushValues)).Return(new List<CardValue>
			{
				CardValue.Nine, CardValue.Two
			});

			//act
			var actual = _instance.GetFlushBasedHandRank(flushValues);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Flush));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(flushCardValue1));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(flushCardValue2));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(flushCardValue3));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(flushCardValue4));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(flushCardValue5));
		}

		#endregion

		#region GetMultiCardOrHighCardHandRank

		[Test]
		public void GetMultiCardOrHighCardHandRank_WHERE_first_group_has_a_group_of_four_cards_SHOULD_return_result_of_GetFourOfAKindHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };

			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(4, CardValue.Five),
				new KeyValuePair<int, CardValue>(1, CardValue.Nine)
			};
			_instance.Stub(x => x.GetOrderedCardGroups(cards)).Return(cardGroups);

			var expected = new PokerHandBasedHandRank(PokerHand.FullHouse);
			_instance.Stub(x => x.GetFourOfAKindHandRank(cardGroups)).Return(expected);

			//act
			var actual = _instance.GetMultiCardOrHighCardHandRank(cards);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetMultiCardOrHighCardHandRank_WHERE_first_group_has_a_group_of_three_cards_SHOULD_return_result_of_GetFullHouseOrThreeOfAKindHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };

			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, CardValue.King),
				new KeyValuePair<int, CardValue>(1, CardValue.Seven)
			};
			_instance.Stub(x => x.GetOrderedCardGroups(cards)).Return(cardGroups);

			var expected = new PokerHandBasedHandRank(PokerHand.HighCard);
			_instance.Stub(x => x.GetFullHouseOrThreeOfAKindHandRank(cardGroups)).Return(expected);

			//act
			var actual = _instance.GetMultiCardOrHighCardHandRank(cards);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetMultiCardOrHighCardHandRank_WHERE_first_group_has_a_group_of_two_cards_SHOULD_return_result_of_GetPairBasedHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, CardValue.Three),
				new KeyValuePair<int, CardValue>(1, CardValue.Ace)
			};
			_instance.Stub(x => x.GetOrderedCardGroups(cards)).Return(cardGroups);

			var expected = new PokerHandBasedHandRank(PokerHand.StraightFlush);
			_instance.Stub(x => x.GetPairBasedHandRank(cardGroups)).Return(expected);

			//act
			var actual = _instance.GetMultiCardOrHighCardHandRank(cards);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetMultiCardOrHighCardHandRank_WHERE_first_group_has_a_group_of_one_card_SHOULD_return_result_of_GetHighCardHandRank()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };
			_hand.Stub(x => x.Cards).Return(cards);

			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(1, CardValue.Seven),
				new KeyValuePair<int, CardValue>(1, CardValue.Two)
			};
			_instance.Stub(x => x.GetOrderedCardGroups(cards)).Return(cardGroups);

			var expected = new PokerHandBasedHandRank(PokerHand.Pair);
			_instance.Stub(x => x.GetHighCardHandRank(cardGroups)).Return(expected);

			//act
			var actual = _instance.GetMultiCardOrHighCardHandRank(cards);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetMultiCardOrHighCardHandRank_WHERE_first_group_has_a_group_of_zero_cards_SHOULD_throw_exception()
		{
			//arrange
			var cards = new List<Card> { new Card(CardValue.Queen, CardSuit.Diamonds) };

			_instance.Stub(x => x.GetOrderedCardGroups(cards)).Return(new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(0, CardValue.Seven)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.GetMultiCardOrHighCardHandRank(cards));
			Assert.That(actualException.Message, Is.EqualTo("Unexpected Card group"));
		}

		#region GetFourOfAKindHandRank

		[Test]
		public void GetFourOfAKindHandRank_WHERE_hand_has_only_four_cards_SHOULD_return_four_of_a_kind_with_single_kicker_set_to_four_of_a_kind_value()
		{
			//arrange
			const CardValue fourOfAKindValue = CardValue.Nine;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(4, fourOfAKindValue)
			};

			//act
			var actual = _instance.GetFourOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FourOfAKind));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(fourOfAKindValue));
		}

		[Test]
		public void GetFourOfAKindHandRank_WHERE_hand_has_four_of_a_kind_and_other_cards_SHOULD_return_four_of_a_kind_with_single_kicker_set_to_four_of_a_kind_value_and_second_kicker_set_to_highest_remaining_value()
		{
			//arrange
			const CardValue fourOfAKindValue = CardValue.Nine;
			const CardValue higherPairValue = CardValue.King;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(4, fourOfAKindValue),
				new KeyValuePair<int, CardValue>(3, CardValue.Jack),
				new KeyValuePair<int, CardValue>(2, higherPairValue)
			};

			//act
			var actual = _instance.GetFourOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FourOfAKind));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(fourOfAKindValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(higherPairValue));
		}

		#endregion

		#region GetFullHouseOrThreeOfAKindHandRank

		[Test]
		public void GetFullHouseOrThreeOfAKindHandRank_WHERE_have_three_of_a_kind_and_nothing_else_SHOULD_return_three_of_a_kind_with_kicker_of_three_of_a_kind_value()
		{
			//arrange
			const CardValue threeOfAKindValue = CardValue.Two;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, threeOfAKindValue)
			};

			//act
			var actual = _instance.GetFullHouseOrThreeOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.ThreeOfAKind));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(threeOfAKindValue));
		}

		[Test]
		public void GetFullHouseOrThreeOfAKindHandRank_WHERE_have_three_of_a_kind_and_single_other_card_SHOULD_return_three_of_a_kind_with_kicker_of_three_of_a_kind_value_and_secondary_kicker_set_to_highest_remaining_card()
		{
			//arrange
			const CardValue threeOfAKindValue = CardValue.Eight;
			const CardValue kickerCardValue = CardValue.Jack;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, threeOfAKindValue),
				new KeyValuePair<int, CardValue>(1, kickerCardValue)
			};

			//act
			var actual = _instance.GetFullHouseOrThreeOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.ThreeOfAKind));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(threeOfAKindValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(kickerCardValue));
		}

		[Test]
		public void GetFullHouseOrThreeOfAKindHandRank_WHERE_have_three_of_a_kind_and_multiple_other_cards_SHOULD_return_three_of_a_kind_with_kicker_of_three_of_a_kind_value_and_secondary_and_tertiary_kickers_set_to_highest_remaining_two_cards()
		{
			//arrange
			const CardValue threeOfAKindValue = CardValue.Eight;
			const CardValue highestRemainingCardValue = CardValue.Jack;
			const CardValue secondHighestRemainingCardValue = CardValue.Seven;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, threeOfAKindValue),
				new KeyValuePair<int, CardValue>(1, highestRemainingCardValue),
				new KeyValuePair<int, CardValue>(1, secondHighestRemainingCardValue),
				new KeyValuePair<int, CardValue>(1, CardValue.Two)
			};

			//act
			var actual = _instance.GetFullHouseOrThreeOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.ThreeOfAKind));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(threeOfAKindValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(highestRemainingCardValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(secondHighestRemainingCardValue));
		}

		[Test]
		public void GetFullHouseOrThreeOfAKindHandRank_WHERE_have_three_of_a_kind_and_pair_SHOULD_return_full_house_with_kickers_set_to_three_of_a_kind_and_pair_values()
		{
			//arrange
			const CardValue threeOfAKindValue = CardValue.Four;
			const CardValue pairValue = CardValue.Jack;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, threeOfAKindValue),
				new KeyValuePair<int, CardValue>(2, pairValue),
				new KeyValuePair<int, CardValue>(1, CardValue.Ace)
			};

			//act
			var actual = _instance.GetFullHouseOrThreeOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(threeOfAKindValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(pairValue));
		}

		[Test]
		public void GetFullHouseOrThreeOfAKindHandRank_WHERE_have_three_of_a_kind_and_pairs_SHOULD_return_full_house_with_kickers_set_to_three_of_a_kind_and_highest_pair_values()
		{
			const CardValue threeOfAKindValue = CardValue.Four;
			const CardValue firstPairValue = CardValue.Jack;
			const CardValue secondPairValue = CardValue.Three;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, threeOfAKindValue),
				new KeyValuePair<int, CardValue>(2, firstPairValue),
				new KeyValuePair<int, CardValue>(2, secondPairValue)
			};

			//act
			var actual = _instance.GetFullHouseOrThreeOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(threeOfAKindValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(firstPairValue));
		}

		[Test]
		public void GetFullHouseOrThreeOfAKindHandRank_WHERE_have_two_three_of_a_kinds_SHOULD_return_full_house_with_kickers_set_to_highest_three_of_a_kind_and_lower_three_of_a_kind()
		{
			const CardValue firstThreeOfAKindValue = CardValue.Queen;
			const CardValue secondThreeOfAKindValue = CardValue.Two;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(3, firstThreeOfAKindValue),
				new KeyValuePair<int, CardValue>(3, secondThreeOfAKindValue),
				new KeyValuePair<int, CardValue>(1, CardValue.Ace)
			};

			//act
			var actual = _instance.GetFullHouseOrThreeOfAKindHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.FullHouse));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(firstThreeOfAKindValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(secondThreeOfAKindValue));
		}

		#endregion

		#region GetPairBasedHandRank

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_pair_but_nothing_else_SHOULD_return_pair_with_single_kicker_value_of_pair_value()
		{
			//arrange
			const CardValue pairValue = CardValue.Seven;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, pairValue)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Pair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(1));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(pairValue));
		}

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_pair_and_single_other_card_SHOULD_return_pair_with_kicker_value_of_pair_value_and_then_remaining_card_value()
		{
			//arrange
			const CardValue pairValue = CardValue.Seven;
			const CardValue highestRemainingCardValue = CardValue.Eight;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, pairValue),
				new KeyValuePair<int, CardValue>(1, highestRemainingCardValue)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Pair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(pairValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(highestRemainingCardValue));
		}

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_pair_and_multiple_other_cards_SHOULD_return_pair_with_kicker_value_of_pair_value_and_then_three_highest_remaining_card_values()
		{
			//arrange
			const CardValue pairValue = CardValue.Seven;
			const CardValue highestRemainingCardValue = CardValue.King;
			const CardValue secondHighestRemainingCardValue = CardValue.Eight;
			const CardValue thirdHighestRemainingCardValue = CardValue.Four;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, pairValue),
				new KeyValuePair<int, CardValue>(1, highestRemainingCardValue),
				new KeyValuePair<int, CardValue>(1, secondHighestRemainingCardValue),
				new KeyValuePair<int, CardValue>(1, thirdHighestRemainingCardValue),
				new KeyValuePair<int, CardValue>(1, CardValue.Two)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.Pair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(4));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(pairValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(highestRemainingCardValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(secondHighestRemainingCardValue));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(thirdHighestRemainingCardValue));
		}

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_two_pairs_only_SHOULD_return_two_pair_with_kicker_value_of_higher_pair_value_and_second_kicker_to_lower_pair_value()
		{
			//arrange
			const CardValue higherPairValue = CardValue.Queen;
			const CardValue lowerPairValue = CardValue.Three;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, higherPairValue),
				new KeyValuePair<int, CardValue>(2, lowerPairValue)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(2));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(higherPairValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(lowerPairValue));
		}

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_two_pairs_and_other_single_cards_SHOULD_return_two_pair_with_kicker_value_of_higher_pair_value_and_second_kicker_to_lower_pair_value_and_third_kicker_to_highest_remaining_value()
		{
			//arrange
			const CardValue higherPairValue = CardValue.Queen;
			const CardValue lowerPairValue = CardValue.Seven;
			const CardValue otherCardValue = CardValue.Ace;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, higherPairValue),
				new KeyValuePair<int, CardValue>(2, lowerPairValue),
				new KeyValuePair<int, CardValue>(1, otherCardValue),
				new KeyValuePair<int, CardValue>(1, CardValue.King)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(higherPairValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(lowerPairValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(otherCardValue));
		}

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_three_pairs_SHOULD_return_two_pair_with_kicker_value_of_higher_pair_value_and_second_kicker_to_lower_pair_value_and_third_kicker_to_highest_remaining_value()
		{
			//arrange
			const CardValue higherPairValue = CardValue.Queen;
			const CardValue secondHighestPairValue = CardValue.Seven;
			const CardValue lowerPairValue = CardValue.Six;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, higherPairValue),
				new KeyValuePair<int, CardValue>(2, secondHighestPairValue),
				new KeyValuePair<int, CardValue>(2, lowerPairValue),
				new KeyValuePair<int, CardValue>(1, CardValue.Three)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(higherPairValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(secondHighestPairValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(lowerPairValue));
		}

		[Test]
		public void GetPairBasedHandRank_WHERE_hand_has_three_pairs_and_other_higher_value_single_card_SHOULD_return_two_pair_with_kicker_value_of_higher_pair_value_and_second_kicker_to_lower_pair_value_and_third_kicker_to_highest_remaining_value()
		{
			//arrange
			const CardValue higherPairValue = CardValue.Queen;
			const CardValue secondHighestPairValue = CardValue.Seven;
			const CardValue highestRemainingValue = CardValue.Ace;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(2, higherPairValue),
				new KeyValuePair<int, CardValue>(2, secondHighestPairValue),
				new KeyValuePair<int, CardValue>(2, CardValue.Six),
				new KeyValuePair<int, CardValue>(1, highestRemainingValue)
			};

			//act
			var actual = _instance.GetPairBasedHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.TwoPair));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(higherPairValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(secondHighestPairValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(highestRemainingValue));
		}

		#endregion

		[Test]
		public void GetHighCardHandRank_WHERE_less_than_five_cards_SHOULD_return_HighCard_with_kickers_set_to_cards_ordered_by_value()
		{
			//arrange
			const CardValue highestValue = CardValue.Queen;
			const CardValue secondHighestValue = CardValue.Seven;
			const CardValue thirdHighestValue = CardValue.Two;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(1, highestValue),
				new KeyValuePair<int, CardValue>(1, secondHighestValue),
				new KeyValuePair<int, CardValue>(1, thirdHighestValue)
			};

			//act
			var actual = _instance.GetHighCardHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.HighCard));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(3));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(highestValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(secondHighestValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(thirdHighestValue));
		}

		[Test]
		public void GetHighCardHandRank_WHERE_more_than_five_cards_SHOULD_return_HighCard_with_kickers_set_to_highest_five_cards_ordered_by_value()
		{
			//arrange
			const CardValue highestValue = CardValue.Queen;
			const CardValue secondHighestValue = CardValue.Jack;
			const CardValue thirdHighestValue = CardValue.Nine;
			const CardValue fourthHighestValue = CardValue.Eight;
			const CardValue fifthHighestValue = CardValue.Four;
			var cardGroups = new List<KeyValuePair<int, CardValue>>
			{
				new KeyValuePair<int, CardValue>(1, highestValue),
				new KeyValuePair<int, CardValue>(1, secondHighestValue),
				new KeyValuePair<int, CardValue>(1, thirdHighestValue),
				new KeyValuePair<int, CardValue>(1, fourthHighestValue),
				new KeyValuePair<int, CardValue>(1, fifthHighestValue),
				new KeyValuePair<int, CardValue>(1, CardValue.Two)
			};

			//act
			var actual = _instance.GetHighCardHandRank(cardGroups);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(PokerHand.HighCard));
			Assert.That(actual.KickerCardValues, Has.Count.EqualTo(5));
			Assert.That(actual.KickerCardValues[0], Is.EqualTo(highestValue));
			Assert.That(actual.KickerCardValues[1], Is.EqualTo(secondHighestValue));
			Assert.That(actual.KickerCardValues[2], Is.EqualTo(thirdHighestValue));
			Assert.That(actual.KickerCardValues[3], Is.EqualTo(fourthHighestValue));
			Assert.That(actual.KickerCardValues[4], Is.EqualTo(fifthHighestValue));
		}

		#endregion

		#region GetFlushValues

		[Test]
		public void GetFlushValues_SHOULD_group_cards_together_by_suit_and_return_list_of_card_values_ordered_by_value_descending_for_suit_with_most_cards()
		{
			//arrange
			const CardValue card1Value = CardValue.Six;
			const CardValue card3Value = CardValue.Four;
			const CardValue card5Value = CardValue.Nine;

			var card1 = new Card(card1Value, CardSuit.Clubs);
			var card2 = new Card(CardValue.Ace, CardSuit.Hearts);
			var card3 = new Card(card3Value, CardSuit.Clubs);
			var card4 = new Card(CardValue.Jack, CardSuit.Diamonds);
			var card5 = new Card(card5Value, CardSuit.Clubs);
			var card6 = new Card(CardValue.Two, CardSuit.Diamonds);

			var cards = new List<Card> { card1, card2, card3, card4, card5, card6 };

			//act
			var actual = _instance.GetFlushValues(cards);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(card5Value));
			Assert.That(actual[1], Is.EqualTo(card1Value));
			Assert.That(actual[2], Is.EqualTo(card3Value));
		}

		#endregion

		#region GetStraightValues

		[Test]
		public void GetStraightValues_without_card_list_SHOULD_pass_hand_cards_to_GetStraightValues()
		{
			const CardValue cardValue = CardValue.Seven;
			var cardInHand = new Card(cardValue, CardSuit.Diamonds);
			var cards = new List<Card> { cardInHand };

			var expected = new List<CardValue> { CardValue.Nine };
			_instance.Expect(x => x.GetStraightValues(new List<CardValue> { cardValue })).Return(expected);

			//act
			var actual = _instance.GetStraightValues(cards);

			//assert
			_instance.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_no_consecutive_values_SHOULD_return_highest_value()
		{
			//Cards Values: K J 9 8 4 6 2

			//arrange
			const CardValue highestCardValue = CardValue.King;
			var cardValue = new List<CardValue>
			{
				CardValue.Eight,
				CardValue.Jack,
				highestCardValue,
				CardValue.Two,
				CardValue.Four,
				CardValue.Six
			};

			//act
			var actual = _instance.GetStraightValues(cardValue);

			//assert
			Assert.That(actual, Has.Count.EqualTo(1));
			Assert.That(actual[0], Is.EqualTo(highestCardValue));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_straight_but_has_longer_straight_starting_at_lower_value_SHOULD_return_longer_straight_with_cards_ordered_by_value()
		{
			//Cards Values: K 10 9 5 4 3 2 

			//arrange
			const CardValue straightCardValue1 = CardValue.Five;
			const CardValue straightCardValue2 = CardValue.Four;
			const CardValue straightCardValue3 = CardValue.Three;
			const CardValue straightCardValue4 = CardValue.Two;

			var cardValues = new List<CardValue>
			{
				CardValue.King,
				straightCardValue2,
				CardValue.Nine,
				CardValue.Ten,
				straightCardValue3,
				straightCardValue1,
				straightCardValue4
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
			Assert.That(actual[3], Is.EqualTo(straightCardValue4));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_two_straights_of_same_length_SHOULD_return_higher_straight_with_cards_ordered_by_value()
		{
			//Cards Values: J 10 9 6 4 3 2 

			//arrange
			const CardValue straightCardValue1 = CardValue.Jack;
			const CardValue straightCardValue2 = CardValue.Ten;
			const CardValue straightCardValue3 = CardValue.Nine;

			var cardValues = new List<CardValue>
			{
				CardValue.Six,
				straightCardValue2,
				CardValue.Four,
				CardValue.Three,
				straightCardValue3,
				straightCardValue1,
				CardValue.Two
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_hand_has_ace_and_have_longer_straight_using_ace_as_low_than_ace_as_high_SHOULD_return_longer_straight_with_ace_used_as_low_and_cards_ordered_by_value()
		{
			//Cards Values: K Q 9 4 3 2 A

			//arrange
			const CardValue straightCardValue1 = CardValue.Four;
			const CardValue straightCardValue2 = CardValue.Three;
			const CardValue straightCardValue3 = CardValue.Two;
			const CardValue straightCardValue4 = CardValue.Ace;

			var cardValues = new List<CardValue>
			{
				CardValue.Queen,
				straightCardValue1,
				CardValue.King,
				straightCardValue2,
				straightCardValue4,
				CardValue.Nine,
				straightCardValue3
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
			Assert.That(actual[3], Is.EqualTo(straightCardValue4));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_hand_has_ace_and_have_longer_straight_using_ace_as_high_than_ace_as_low_SHOULD_return_longer_straight_with_ace_used_as_high_and_cards_ordered_by_value()
		{
			//Cards Values: K Q J 7 3 2 A

			//arrange
			const CardValue straightCardValue1 = CardValue.Ace;
			const CardValue straightCardValue2 = CardValue.King;
			const CardValue straightCardValue3 = CardValue.Queen;
			const CardValue straightCardValue4 = CardValue.Jack;

			var cardValues = new List<CardValue>
			{
				CardValue.Seven,
				straightCardValue3,
				straightCardValue2,
				CardValue.Two,
				straightCardValue4,
				straightCardValue1,
				CardValue.Three
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
			Assert.That(actual[3], Is.EqualTo(straightCardValue4));
		}

		[Test]
		public void GetStraightValues_with_card_list_WHERE_hand_has_ace_and_have_higher_straight_using_ace_as_high_than_ace_as_low_SHOULD_return_higher_straight_with_ace_used_as_high_and_cards_ordered_by_value()
		{
			//Cards Values: K Q 9 7 3 2 A

			//arrange
			const CardValue straightCardValue1 = CardValue.Ace;
			const CardValue straightCardValue2 = CardValue.King;
			const CardValue straightCardValue3 = CardValue.Queen;

			var cardValues = new List<CardValue>
			{
				straightCardValue3,
				CardValue.Seven,
				straightCardValue2,
				CardValue.Two,
				straightCardValue1,
				CardValue.Three,
				CardValue.Nine
			};

			//act
			var actual = _instance.GetStraightValues(cardValues);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(straightCardValue1));
			Assert.That(actual[1], Is.EqualTo(straightCardValue2));
			Assert.That(actual[2], Is.EqualTo(straightCardValue3));
		}

		#endregion

		#region GetOrderedCardGroups

		[Test]
		public void GetOrderedCardGroups_WHERE_no_multi_cards_SHOULD_return_groups_of_one_ordered_by_card_value_descending()
		{
			//arrange
			const CardValue card1Value = CardValue.Eight;
			const CardValue card2Value = CardValue.Four;
			const CardValue card3Value = CardValue.King;
			const CardValue card4Value = CardValue.Ace;
			const CardValue card5Value = CardValue.Ten;
			const CardValue card6Value = CardValue.Seven;
			const CardValue card7Value = CardValue.Two;

			var card1 = new Card(card1Value, CardSuit.Diamonds);
			var card2 = new Card(card2Value, CardSuit.Hearts);
			var card3 = new Card(card3Value, CardSuit.Spades);
			var card4 = new Card(card4Value, CardSuit.Hearts);
			var card5 = new Card(card5Value, CardSuit.Spades);
			var card6 = new Card(card6Value, CardSuit.Clubs);
			var card7 = new Card(card7Value, CardSuit.Clubs);

			var cards = new List<Card> { card1, card2, card3, card4, card5, card6, card7 };

			//act
			var actual = _instance.GetOrderedCardGroups(cards);

			//assert
			Assert.That(actual, Has.Count.EqualTo(7));
			Assert.That(actual[0].Key, Is.EqualTo(1));
			Assert.That(actual[0].Value, Is.EqualTo(card4Value));

			Assert.That(actual[1].Key, Is.EqualTo(1));
			Assert.That(actual[1].Value, Is.EqualTo(card3Value));

			Assert.That(actual[2].Key, Is.EqualTo(1));
			Assert.That(actual[2].Value, Is.EqualTo(card5Value));

			Assert.That(actual[3].Key, Is.EqualTo(1));
			Assert.That(actual[3].Value, Is.EqualTo(card1Value));

			Assert.That(actual[4].Key, Is.EqualTo(1));
			Assert.That(actual[4].Value, Is.EqualTo(card6Value));

			Assert.That(actual[5].Key, Is.EqualTo(1));
			Assert.That(actual[5].Value, Is.EqualTo(card2Value));

			Assert.That(actual[6].Key, Is.EqualTo(1));
			Assert.That(actual[6].Value, Is.EqualTo(card7Value));
		}

		[Test]
		public void GetOrderedCardGroups_WHERE_have_a_three_of_a_kind_and_pair_SHOULD_return_groups_of_multi_cards_ordered_by_descending_quantity()
		{
			//arrange
			const CardValue threeOfAKindCardValue = CardValue.Nine;
			const CardValue pairCardValue = CardValue.Jack;
			const CardValue highCardValue = CardValue.King;

			var card1 = new Card(highCardValue, CardSuit.Spades);
			var card2 = new Card(threeOfAKindCardValue, CardSuit.Diamonds);
			var card3 = new Card(pairCardValue, CardSuit.Hearts);
			var card4 = new Card(threeOfAKindCardValue, CardSuit.Clubs);
			var card5 = new Card(pairCardValue, CardSuit.Spades);
			var card6 = new Card(threeOfAKindCardValue, CardSuit.Hearts);

			var cards = new List<Card> { card1, card2, card3, card4, card5, card6 };

			//act
			var actual = _instance.GetOrderedCardGroups(cards);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0].Key, Is.EqualTo(3));
			Assert.That(actual[0].Value, Is.EqualTo(threeOfAKindCardValue));

			Assert.That(actual[1].Key, Is.EqualTo(2));
			Assert.That(actual[1].Value, Is.EqualTo(pairCardValue));

			Assert.That(actual[2].Key, Is.EqualTo(1));
			Assert.That(actual[2].Value, Is.EqualTo(highCardValue));
		}

		[Test]
		public void GetOrderedCardGroups_WHERE_have_three_pairs_SHOULD_return_groups_of_multi_cards_ordered_by_descending_value()
		{
			//arrange
			const CardValue pair1CardValue = CardValue.Nine;
			const CardValue pair2CardValue = CardValue.Jack;
			const CardValue pair3CardValue = CardValue.King;

			var card1 = new Card(pair3CardValue, CardSuit.Clubs);
			var card2 = new Card(pair1CardValue, CardSuit.Spades);
			var card3 = new Card(pair2CardValue, CardSuit.Spades);
			var card4 = new Card(pair1CardValue, CardSuit.Diamonds);
			var card5 = new Card(pair2CardValue, CardSuit.Hearts);
			var card6 = new Card(pair3CardValue, CardSuit.Hearts);

			var cards = new List<Card> { card1, card2, card3, card4, card5, card6 };

			//act
			var actual = _instance.GetOrderedCardGroups(cards);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0].Key, Is.EqualTo(2));
			Assert.That(actual[0].Value, Is.EqualTo(pair3CardValue));

			Assert.That(actual[1].Key, Is.EqualTo(2));
			Assert.That(actual[1].Value, Is.EqualTo(pair2CardValue));

			Assert.That(actual[2].Key, Is.EqualTo(2));
			Assert.That(actual[2].Value, Is.EqualTo(pair1CardValue));
		}

		#endregion
	}
}
