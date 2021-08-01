using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Unit.Domain.TestData
{
	public static class CardTestCaseData
	{
		public static IEnumerable<TestCaseData> AllCardsTestCaseData => AllCards.Select(x => new TestCaseData(x.Value, x.Suit));

		public static List<Card> AllCards
		{
			get
			{
				var cardSuits = Utilities.GetEnumValues<CardSuit>();
				var cardValues = Utilities.GetEnumValues<CardValue>();
				return cardSuits.SelectMany(cardSuit => cardValues.Select(cardValue => new Card(cardValue, cardSuit))).ToList();
			}
		}

		private static IEnumerable<string> CardValuesAsStrings => new List<string> { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
		private static IEnumerable<string> CardSuitsAsStrings => new List<string> { "S", "H", "D", "C" };
		public static IEnumerable<TestCaseData> AllCardsAsString
		{
			get
			{
				return CardValuesAsStrings.SelectMany(x => CardSuitsAsStrings, (value, suit) =>
				{
					var cardValueEnum = Utilities.GetEnumValueFromDescription<CardValue>(value);
					var cardSuitEnum = Utilities.GetEnumValueFromDescription<CardSuit>(suit);
					var cardAsString = $"{value}{suit}";
					return new TestCaseData(cardAsString, cardValueEnum, cardSuitEnum).SetName(cardAsString);
				});
			}
		}
	}
}
