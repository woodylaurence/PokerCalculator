using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Tests.Shared.TestData;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.HandRankCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankIntegrationTests : LocalTestBase
	{
		private PokerHandBasedHandRank _lhsHandRank;
		private PokerHandBasedHandRank _rhsHandRank;

		#region CompareTo

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithLowerPokerHands))]
		public void CompareTo_WHERE_lhs_poker_hand_is_greater_than_rhs_SHOULD_return_one(PokerHand lhsPokerHand, PokerHand rhsPokerHand)
		{
			//arrange
			_lhsHandRank = new PokerHandBasedHandRank(lhsPokerHand);
			_rhsHandRank = new PokerHandBasedHandRank(rhsPokerHand);

			//act
			var actual = _lhsHandRank.CompareTo(_rhsHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithHigherPokerHands))]
		public void CompareTo_WHERE_lhs_poker_hand_is_less_than_rhs_SHOULD_return_negative_one(PokerHand lhsPokerHand, PokerHand rhsPokerHand)
		{
			//arrange
			_lhsHandRank = new PokerHandBasedHandRank(lhsPokerHand);
			_rhsHandRank = new PokerHandBasedHandRank(rhsPokerHand);

			//act
			var actual = _lhsHandRank.CompareTo(_rhsHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_both_hand_ranks_have_same_poker_hand_and_same_kickers_SHOULD_return_zero(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };
			var rhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };

			_lhsHandRank = new PokerHandBasedHandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new PokerHandBasedHandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank.CompareTo(_rhsHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_both_hand_ranks_have_same_poker_hand_and_differ_on_fifth_kicker_card_with_lhs_being_higher_SHOULD_return_false(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Five };
			var rhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };

			_lhsHandRank = new PokerHandBasedHandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new PokerHandBasedHandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank.CompareTo(_rhsHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_both_hand_ranks_have_same_poker_hand_and_differ_on_fifth_kicker_card_with_lhs_being_lower_SHOULD_return_true(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.King, CardValue.Jack, CardValue.Eight, CardValue.Five, CardValue.Three };
			var rhsKickerValues = new List<CardValue> { CardValue.King, CardValue.Jack, CardValue.Eight, CardValue.Five, CardValue.Four };

			_lhsHandRank = new PokerHandBasedHandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new PokerHandBasedHandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank.CompareTo(_rhsHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		#endregion
	}
}
