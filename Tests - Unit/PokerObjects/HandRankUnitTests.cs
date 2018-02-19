using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class HandRankUnitTests : AbstractUnitTestBase
	{
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
	}
}
