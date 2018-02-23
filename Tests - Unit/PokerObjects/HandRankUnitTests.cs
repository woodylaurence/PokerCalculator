using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;
using Rhino.Mocks;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class HandRankUnitTests : AbstractUnitTestBase
	{
		private HandRank _instance;

		[SetUp]
		public override void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<HandRank>(null, null);
		}

		#region Constructor

		[Test]
		public void Constructor_WHERE_kicker_cards_empty_SHOULD_construct_hand_rank_with_empty_kicker_cards()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.Flush;

			//act
			var actual = new HandRank(pokerHand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCardValues, Is.Empty);
		}

		[Test]
		public void Constructor()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.TwoPair;
			var kickers = new List<CardValue> { CardValue.Six };

			//act
			var actual = new HandRank(pokerHand, kickers);

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
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

			_instance.Stub(x => x.PokerHand).Return(thisPokerHand);
			otherHandRank.Stub(x => x.PokerHand).Return(otherPokerHand);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(-1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.PokerHandWithLowerPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_lower_SHOULD_return_one(PokerHand thisPokerHand, PokerHand otherPokerHand)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

			_instance.Stub(x => x.PokerHand).Return(thisPokerHand);
			otherHandRank.Stub(x => x.PokerHand).Return(otherPokerHand);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(1));
		}

		[Test, TestCaseSource(typeof(PokerHandComparisonTestCaseData), nameof(PokerHandComparisonTestCaseData.AllPokerHands))]
		public void CompareTo_WHERE_other_hand_ranks_poker_hand_is_identical_SHOULD_return_result_of_CompareKickers(PokerHand pokerHand)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

			_instance.Stub(x => x.PokerHand).Return(pokerHand);
			otherHandRank.Stub(x => x.PokerHand).Return(pokerHand);

			const int expected = 4;
			_instance.Stub(x => x.CompareKickers(otherHandRank)).Return(expected);

			//act
			var actual = _instance.CompareTo(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#region CompareKickers

		[Test]
		public void CompareKickers_WHERE_no_kickers_SHOULD_return_zero()
		{
			//arrange
			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue>());

			//act
			var actual = _instance.CompareKickers(null);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void CompareKickers_WHERE_some_kickers_and_other_hand_rank_has_higher_first_kicker_SHOULD_return_minus_one()
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

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
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

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
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

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
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

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
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);

			_instance.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Four, CardValue.Seven });
			otherHandRank.Stub(x => x.KickerCardValues).Return(new List<CardValue> { CardValue.Four, CardValue.Seven });

			//act
			var actual = _instance.CompareKickers(otherHandRank);

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		#endregion

		#endregion

		#region Operator Overloads

		[TestCase(-1, true)]
		[TestCase(0, false)]
		[TestCase(1, false)]
		public void LessThan(int compareToResult, bool expected)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			_instance.Stub(x => x.CompareTo(otherHandRank)).Return(compareToResult);

			//act
			var actual = _instance < otherHandRank;

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(-1, false)]
		[TestCase(0, false)]
		[TestCase(1, true)]
		public void GreaterThan(int compareToResult, bool expected)
		{
			//arrange
			var otherHandRank = MockRepository.GenerateStrictMock<HandRank>(null, null);
			_instance.Stub(x => x.CompareTo(otherHandRank)).Return(compareToResult);

			//act
			var actual = _instance > otherHandRank;

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
