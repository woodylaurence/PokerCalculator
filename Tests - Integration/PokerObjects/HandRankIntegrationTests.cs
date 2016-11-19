﻿using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandRankIntegrationTests
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
			Assert.That(actual.KickerCards, Is.Empty);
		}

		[Test]
		public void Create()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.TwoPair;
			var kickerCard = Card.Create(CardValue.Six, CardSuit.Clubs);
			var kickers = new List<Card> { kickerCard };

			//act
			var actual = HandRank.Create(pokerHand, kickers);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCards, Is.EqualTo(kickers));			            
		}

		#endregion
	}
}