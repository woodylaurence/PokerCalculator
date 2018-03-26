using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerCalculator
{
	[TestFixture]
	public class PokerOddsUnitTests : AbstractUnitTestBase
	{
		private PokerOdds _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			Utilities.MethodObject = MockRepository.GenerateStrictMock<Utilities>();
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<PokerHand>()).Return(new List<PokerHand>()).Repeat.Times(2);

			_instance = MockRepository.GeneratePartialMock<PokerOdds>();

			PokerOdds.MethodObject = MockRepository.GenerateStrictMock<PokerOdds>();
		}

		[TearDown]
		protected void TearDown()
		{
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<PokerHand>()).Return(new List<PokerHand>()).Repeat.Once();
			PokerOdds.MethodObject = new PokerOdds();
			Utilities.MethodObject = new Utilities();
		}

		#region Properties and Fields

		#region TotalNumHands

		[Test]
		public void TotalNumHands()
		{
			//arrange
			const int numWins = 634;
			const int numDraws = 1867;
			const int numLosses = 167;

			_instance.Stub(x => x.NumWins).Return(numWins);
			_instance.Stub(x => x.NumDraws).Return(numDraws);
			_instance.Stub(x => x.NumLosses).Return(numLosses);

			//act
			var actual = _instance.TotalNumHands;

			//assert
			Assert.That(actual, Is.EqualTo(numWins + numDraws + numLosses));
		}

		#endregion

		#region WinPercentage

		[Test]
		public void WinPercentage_WHERE_total_num_hands_is_zero_SHOULD_return_zero()
		{
			//arrange
			_instance.Stub(x => x.TotalNumHands).Return(0);

			//act
			var actual = _instance.WinPercentage;

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void WinPercentage_WHERE_total_num_hands_is_non_zero_SHOULD_return_num_wins_divided_by_total_num_hands()
		{
			//arrange
			const int totalNumHands = 11674;
			const int numWins = 8673;
			_instance.Stub(x => x.TotalNumHands).Return(totalNumHands);
			_instance.Stub(x => x.NumWins).Return(numWins);

			//act
			var actual = _instance.WinPercentage;

			//assert
			Assert.That(actual, Is.EqualTo((double)numWins / totalNumHands));
		}

		#endregion

		#region DrawPercentage

		[Test]
		public void DrawPercentage_WHERE_total_num_hands_is_zero_SHOULD_return_zero()
		{
			//arrange
			_instance.Stub(x => x.TotalNumHands).Return(0);

			//act
			var actual = _instance.DrawPercentage;

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void DrawPercentage_WHERE_total_num_hands_is_non_zero_SHOULD_return_num_draws_divided_by_total_num_hands()
		{
			//arrange
			const int totalNumHands = 11674;
			const int numDraws = 8673;
			_instance.Stub(x => x.TotalNumHands).Return(totalNumHands);
			_instance.Stub(x => x.NumDraws).Return(numDraws);

			//act
			var actual = _instance.DrawPercentage;

			//assert
			Assert.That(actual, Is.EqualTo((double)numDraws / totalNumHands));
		}

		#endregion

		#region LossPercentage

		[Test]
		public void LossPercentage_WHERE_total_num_hands_is_zero_SHOULD_return_zero()
		{
			//arrange
			_instance.Stub(x => x.TotalNumHands).Return(0);

			//act
			var actual = _instance.LossPercentage;

			//assert
			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void LossPercentage_WHERE_total_num_hands_is_non_zero_SHOULD_return_num_losses_divided_by_total_num_hands()
		{
			//arrange
			const int totalNumHands = 11674;
			const int numLosses = 8673;
			_instance.Stub(x => x.TotalNumHands).Return(totalNumHands);
			_instance.Stub(x => x.NumLosses).Return(numLosses);

			//act
			var actual = _instance.LossPercentage;

			//assert
			Assert.That(actual, Is.EqualTo((double)numLosses / totalNumHands));
		}

		#endregion

		#region PokerHandPercentages

		[Test]
		public void PokerHandPercentages_WHERE_total_num_hands_is_zero_SHOULD_return_dictionary_with_percentage_chance_set_to_zero_for_each_poker_hand()
		{
			//arrange
			_instance.Stub(x => x.TotalNumHands).Return(0);
			_instance.Stub(x => x.PokerHandFrequencies).Return(new Dictionary<PokerHand, int>
			{
				{ PokerHand.FourOfAKind, 14 },
				{ PokerHand.Pair, 9 },
				{ PokerHand.RoyalFlush, 8 }
			});

			//act
			var actual = _instance.PokerHandPercentages;

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[PokerHand.FourOfAKind], Is.EqualTo(0));
			Assert.That(actual[PokerHand.Pair], Is.EqualTo(0));
			Assert.That(actual[PokerHand.RoyalFlush], Is.EqualTo(0));
		}

		[Test]
		public void PokerHandPercentages_WHERE_total_num_hands_is_non_zero_SHOULD_return_dictionary_with_percentage_chance_set_for_each_poker_hand()
		{
			//arrange
			const int totalNumHands = 744;
			const int numFourOfAKinds = 14;
			const int numPairs = 9;
			const int numRoyalFlushes = 8;

			_instance.Stub(x => x.TotalNumHands).Return(totalNumHands);
			_instance.Stub(x => x.PokerHandFrequencies).Return(new Dictionary<PokerHand, int>
			{
				{ PokerHand.FourOfAKind, numFourOfAKinds },
				{ PokerHand.Pair, numPairs },
				{ PokerHand.RoyalFlush, numRoyalFlushes }
			});

			//act
			var actual = _instance.PokerHandPercentages;

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[PokerHand.FourOfAKind], Is.EqualTo((double)numFourOfAKinds / totalNumHands));
			Assert.That(actual[PokerHand.Pair], Is.EqualTo((double)numPairs / totalNumHands));
			Assert.That(actual[PokerHand.RoyalFlush], Is.EqualTo((double)numRoyalFlushes / totalNumHands));
		}

		#endregion

		#endregion

		#region Constructor

		[Test]
		public void Constructor()
		{
			//arrange
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<PokerHand>()).Return(new List<PokerHand>
			{
				PokerHand.Flush,
				PokerHand.Pair,
				PokerHand.StraightFlush
			});

			//act
			var actual = new PokerOdds();

			//assert
			Assert.That(actual.PokerHandFrequencies, Has.Count.EqualTo(3));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.Flush));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.Pair));
			Assert.That(actual.PokerHandFrequencies.ContainsKey(PokerHand.StraightFlush));
			Assert.That(actual.PokerHandFrequencies.Values, Has.All.EqualTo(0));
		}

		#endregion

		#region Static Methods

		[Test]
		public void AggregatePokerOdds_calls_slave()
		{
			//arrange
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<PokerHand>()).Return(new List<PokerHand>());
			var pokerOdds = new List<PokerOdds> { new PokerOdds() };
			PokerOdds.MethodObject.Stub(x => x.AggregatePokerOddsSlave(pokerOdds)).Return(_instance);

			//act
			var actual = PokerOdds.AggregatePokerOdds(pokerOdds);

			//assert
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void AggregatePokerOdds_WHERE_single_item_supplied_SHOULD_throw_error()
		{
			//arrange
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<PokerHand>()).Return(new List<PokerHand>());
			var pokerOdds = MockRepository.GenerateStrictMock<PokerOdds>();

			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.AggregatePokerOddsSlave(new List<PokerOdds> { pokerOdds }));
			Assert.That(actualException.Message, Is.EqualTo("Cannot aggregate less than two PokerOdds."));
		}

		#endregion
	}
}