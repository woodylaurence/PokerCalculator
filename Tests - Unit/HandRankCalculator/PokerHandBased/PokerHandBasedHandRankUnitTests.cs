using System;
using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.HandRankCalculator.PokerHandBased;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;
using Rhino.Mocks;

namespace PokerCalculator.Tests.Unit.HandRankCalculator.PokerHandBased
{
	[TestFixture]
	public class PokerHandBasedHandRankUnitTests : AbstractUnitTestBase
	{
		private PokerHandBasedHandRank _instance;

		[SetUp]
		protected override void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<PokerHandBasedHandRank>(null, null);
		}

		#region Properties and Fields

		[Test]
		public void PokerHand_getter()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.Straight;
			_instance.Stub(x => x.Rank).Return(pokerHand);

			//act
			var actual = _instance.PokerHand;

			//assert
			Assert.That(actual, Is.EqualTo(pokerHand));
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
			//act
			var actual = _instance.CompareTo(_instance);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void CompareTo_WHERE_other_hand_rank_is_null_SHOULD_return_one()
		{
			//act
			var actual = _instance.CompareTo(null);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithHigherPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_higher_SHOULD_return_minus_one(PokerHand thisPokerHand, PokerHand otherPokerHand)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.Rank).Return(thisPokerHand);
			otherHandRank.Stub(x => x.Rank).Return(otherPokerHand);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithLowerPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_lower_SHOULD_return_one(PokerHand thisPokerHand, PokerHand otherPokerHand)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.Rank).Return(thisPokerHand);
			otherHandRank.Stub(x => x.Rank).Return(otherPokerHand);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_other_hand_rank_is_not_PokerHandBasedHandRank_SHOULD_throw_error(PokerHand pokerHand)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<IHandRank<PokerHand>>();

			_instance.Stub(x => x.Rank).Return(pokerHand);
			otherHandRank.Stub(x => x.Rank).Return(pokerHand);

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.CompareTo(otherHandRank));
			Assert.That(actualException.Message, Is.EqualTo("Other HandRank is not PokerHandBasedHandRank"));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_and_other_hand_rank_is_PokerHandBasedHandRank_SHOULD_return_result_of_CompareKickers(PokerHand pokerHand)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.Rank).Return(pokerHand);
			otherHandRank.Stub(x => x.Rank).Return(pokerHand);

			const int expected = 4;
			_instance.Stub(x => x.CompareKickers(otherHandRank)).Return(expected);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#region CompareKickers

		[Test]
		public void CompareKickers_WHERE_this_hand_rank_kickers_is_different_length_to_other_hand_rank_kickers_SHOULD_throw_error()
		{
			//arrange
			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Jack, CardValue.Eight });

			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Seven, CardValue.Four, CardValue.Queen });

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.CompareKickers(otherHandRank));
			Assert.That(actualException.Message, Is.EqualTo("Kickers have different lengths."));
		}

		[Test]
		public void CompareKickers_WHERE_no_kickers_SHOULD_return_zero()
		{
			//arrange
			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue>());

			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue>());

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void CompareKickers_WHERE_some_kickers_and_other_hand_rank_has_higher_first_kicker_SHOULD_return_minus_one()
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Five });
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Nine });

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test]
		public void CompareKickers_WHERE_some_kickers_and_other_hand_rank_has_lower_first_kicker_SHOULD_return_one()
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Nine });
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Five });

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test]
		public void CompareKickers_WHERE_some_kickers_and_other_hand_rank_differs_in_third_kicker_with_higher_value_SHOULD_return_minus_one()
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Nine, CardValue.Jack, CardValue.King });
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Nine, CardValue.Jack, CardValue.Ace });

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test]
		public void CompareKickers_WHERE_some_kickers_and_other_hand_rank_differs_in_third_kicker_with_lower_value_SHOULD_return_one()
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Nine, CardValue.Jack, CardValue.Ace });
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Nine, CardValue.Jack, CardValue.King });

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test]
		public void CompareKickers_WHERE_some_kickers_and_kickers_match_SHOULD_return_zero()
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<PokerHandBasedHandRank>(null, null);

			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Four, CardValue.Seven });
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Four, CardValue.Seven });

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		#endregion

		#endregion
	}
}
