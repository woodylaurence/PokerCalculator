using Moq;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.TestData;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.HandRankCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankUnitTests : AbstractUnitTestBase
	{
		private PokerHandBasedHandRank _instance;

		#region Properties and Fields

		[Test]
		public void PokerHand_getter_SHOULD_return_rank()
		{
			//arrange
			_instance = new PokerHandBasedHandRank(PokerHand.Straight);

			//act
			var actual = _instance.PokerHand;

			//assert
			Assert.That(actual, Is.EqualTo(PokerHand.Straight));
		}

		#endregion

		#region Constructor

		[Test]
		public void Constructor_WHERE_kicker_cards_empty_SHOULD_construct_hand_rank_with_empty_kicker_cards()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.Flush;

			//act
			var actual = new PokerHandBasedHandRank(pokerHand);

			//assert
			Assert.That(actual.Rank, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCardValues, Is.Empty);
		}

		[Test]
		public void Constructor()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.TwoPair;
			var kickers = new List<CardValue> { CardValue.Six };

			//act
			var actual = new PokerHandBasedHandRank(pokerHand, kickers);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCardValues, Is.EqualTo(kickers));
		}

		#endregion

		#region CompareTo

		[Test]
		public void CompareTo_WHERE_other_hand_rank_is_same_reference_SHOULD_return_zero()
		{
			//arrange
			_instance = new PokerHandBasedHandRank(PokerHand.FullHouse);

			//act
			var actual = _instance.CompareTo(_instance);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void CompareTo_WHERE_other_hand_rank_is_null_SHOULD_return_one()
		{
			//arrange
			_instance = new PokerHandBasedHandRank(PokerHand.FullHouse);

			//act
			var actual = _instance.CompareTo(null);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithHigherPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_higher_SHOULD_return_minus_one(PokerHand thisPokerHand, PokerHand otherPokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(thisPokerHand);
			var otherHandRank = new PokerHandBasedHandRank(otherPokerHand);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithLowerPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_lower_SHOULD_return_one(PokerHand thisPokerHand, PokerHand otherPokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(thisPokerHand);
			var otherHandRank = new PokerHandBasedHandRank(otherPokerHand);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_other_hand_rank_is_not_PokerHandBasedHandRank_SHOULD_throw_error(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand);

			var otherHandRank = new Mock<IHandRank<PokerHand>>();
			otherHandRank.Setup(x => x.Rank).Returns(pokerHand);

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.CompareTo(otherHandRank.Object));
			Assert.That(actualException.Message, Is.EqualTo("Other HandRank is not PokerHandBasedHandRank"));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_has_different_number_of_kickers_to_current_hand_rank_SHOULD_throw_error(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Four });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.King, CardValue.Three, CardValue.Two });

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.CompareTo(otherHandRank));
			Assert.That(actualException.Message, Is.EqualTo("Cannot compare hand ranks, kickers have different lengths"));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_are_identical_SHOULD_return_zero(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Four, CardValue.Three });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Four, CardValue.Three });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.Zero);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_no_kickers_SHOULD_return_zero(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand);
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue>());

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.Zero);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_differ_in_first_kicker_with_other_hand_ranks_kicker_being_lower_SHOULD_return_one(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Eight });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_differ_in_first_kicker_with_other_hand_ranks_kicker_being_higher_SHOULD_return_minus_one(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Queen });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_differ_in_third_kicker_with_other_hand_ranks_kicker_being_lower_SHOULD_return_one(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Eight, CardValue.Six });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Eight, CardValue.Four });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_differ_in_third_kicker_with_other_hand_ranks_kicker_being_higher_SHOULD_return_minus_one(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Eight, CardValue.Six });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.Ten, CardValue.Eight, CardValue.Seven });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_differ_in_fifth_kicker_with_other_hand_ranks_kicker_being_lower_SHOULD_return_one(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.King, CardValue.Ten, CardValue.Nine, CardValue.Seven, CardValue.Three });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.King, CardValue.Ten, CardValue.Nine, CardValue.Seven, CardValue.Two });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_kickers_differ_in_fifth_kicker_with_other_hand_ranks_kicker_being_higher_SHOULD_return_minus_one(PokerHand pokerHand)
		{
			//arrange
			_instance = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.King, CardValue.Ten, CardValue.Nine, CardValue.Seven, CardValue.Three });
			var otherHandRank = new PokerHandBasedHandRank(pokerHand, new List<CardValue> { CardValue.King, CardValue.Ten, CardValue.Nine, CardValue.Seven, CardValue.Five });

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		#endregion
	}
}
