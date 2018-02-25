using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared.TestData;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandRankIntegrationTests : LocalTestBase
	{
		private HandRank _lhsHandRank;
		private HandRank _rhsHandRank;

		#region Operator Overloads

		#region <

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithLowerPokerHands))]
		public void LessThan_WHERE_lhs_poker_hand_is_greater_than_rhs_SHOULD_return_false(PokerHand lhsPokerHand, PokerHand rhsPokerHand)
		{
			//arrange
			_lhsHandRank = new HandRank(lhsPokerHand);
			_rhsHandRank = new HandRank(rhsPokerHand);

			//act
			var actual = _lhsHandRank < _rhsHandRank;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithHigherPokerHands))]
		public void LessThan_WHERE_lhs_poker_hand_is_less_than_rhs_SHOULD_return_true(PokerHand lhsPokerHand, PokerHand rhsPokerHand)
		{
			//arrange
			_lhsHandRank = new HandRank(lhsPokerHand);
			_rhsHandRank = new HandRank(rhsPokerHand);

			//act
			var actual = _lhsHandRank < _rhsHandRank;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void LessThan_WHERE_both_hand_ranks_have_same_poker_hand_and_same_kickers_SHOULD_return_false(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };
			var rhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };

			_lhsHandRank = new HandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new HandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank < _rhsHandRank;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void LessThan_WHERE_both_hand_ranks_have_same_poker_hand_and_differ_on_fifth_kicker_card_with_lhs_being_higher_SHOULD_return_false(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Five };
			var rhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };

			_lhsHandRank = new HandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new HandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank < _rhsHandRank;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void LessThan_WHERE_both_hand_ranks_have_same_poker_hand_and_differ_on_fifth_kicker_card_with_lhs_being_lower_SHOULD_return_true(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.King, CardValue.Jack, CardValue.Eight, CardValue.Five, CardValue.Three };
			var rhsKickerValues = new List<CardValue> { CardValue.King, CardValue.Jack, CardValue.Eight, CardValue.Five, CardValue.Four };

			_lhsHandRank = new HandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new HandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank < _rhsHandRank;

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region >

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithHigherPokerHands))]
		public void GreaterThan_WHERE_lhs_poker_hand_is_less_than_rhs_SHOULD_return_false(PokerHand lhsPokerHand, PokerHand rhsPokerHand)
		{
			//arrange
			_lhsHandRank = new HandRank(lhsPokerHand);
			_rhsHandRank = new HandRank(rhsPokerHand);

			//act
			var actual = _lhsHandRank > _rhsHandRank;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithLowerPokerHands))]
		public void GreaterThan_WHERE_lhs_poker_hand_is_greater_than_rhs_SHOULD_return_true(PokerHand lhsPokerHand, PokerHand rhsPokerHand)
		{
			//arrange
			_lhsHandRank = new HandRank(lhsPokerHand);
			_rhsHandRank = new HandRank(rhsPokerHand);

			//act
			var actual = _lhsHandRank > _rhsHandRank;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void GreaterThan_WHERE_both_hand_ranks_have_same_poker_hand_and_same_kickers_SHOULD_return_false(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };
			var rhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };

			_lhsHandRank = new HandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new HandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank > _rhsHandRank;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void GreaterThan_WHERE_both_hand_ranks_have_same_poker_hand_and_differ_on_fifth_kicker_card_with_lhs_being_lower_SHOULD_return_false(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Two };
			var rhsKickerValues = new List<CardValue> { CardValue.Ace, CardValue.Ten, CardValue.Eight, CardValue.Seven, CardValue.Five };

			_lhsHandRank = new HandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new HandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank > _rhsHandRank;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void GreaterThan_WHERE_both_hand_ranks_have_same_poker_hand_and_differ_on_fifth_kicker_card_with_lhs_being_higher_SHOULD_return_true(PokerHand pokerHand)
		{
			//arrange
			var lhsKickerValues = new List<CardValue> { CardValue.King, CardValue.Jack, CardValue.Eight, CardValue.Five, CardValue.Four };
			var rhsKickerValues = new List<CardValue> { CardValue.King, CardValue.Jack, CardValue.Eight, CardValue.Five, CardValue.Three };

			_lhsHandRank = new HandRank(pokerHand, lhsKickerValues);
			_rhsHandRank = new HandRank(pokerHand, rhsKickerValues);

			//act
			var actual = _lhsHandRank > _rhsHandRank;

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#endregion
	}
}
