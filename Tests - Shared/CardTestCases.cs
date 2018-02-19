using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Tests.Shared
{
	public static class CardTestCases
	{
		public static IEnumerable<TestCaseData> AllCardsTestCaseData => AllCards.Select(x => new TestCaseData(x.Value, x.Suit));

		public static List<Card> AllCards
		{
			get
			{
				var utilities = new UtilitiesService();
				var cardSuits = utilities.GetEnumValues<CardSuit>();
				var cardValues = utilities.GetEnumValues<CardValue>();
				return cardSuits.SelectMany(cardSuit => cardValues.Select(cardValue => new Card(cardValue, cardSuit))).ToList();
			}
		}
	}
}
