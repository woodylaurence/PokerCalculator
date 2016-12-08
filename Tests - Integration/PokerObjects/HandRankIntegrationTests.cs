using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandRankIntegrationTests : LocalTestBase
	{
		#region Create

		[Test]
		public void Create_WHERE_kicker_cards_empty_SHOULD_create_hand_rank_with_empty_kicker_cards()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.Flush;

			//act
			var actual = HandRank.Create(pokerHand);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCardValues, Is.Empty);
		}

		[Test]
		public void Create()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.TwoPair;
			var kickers = new List<CardValue> { CardValue.Six };

			//act
			var actual = HandRank.Create(pokerHand, kickers);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCardValues, Is.EqualTo(kickers));			            
		}

		#endregion
	}
}
