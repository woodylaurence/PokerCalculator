using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Tests.Shared.TestData
{
	public static class CardTestCaseData
	{
		private static readonly IUtilitiesService _utilitiesService = new UtilitiesService();

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

		private static IEnumerable<string> CardValuesAsStrings => new List<string> { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
		private static IEnumerable<string> CardSuitsAsStrings => new List<string> { "S", "H", "D", "C" };
		public static IEnumerable<TestCaseData> AllCardsAsString
		{
			get
			{
				return CardValuesAsStrings.SelectMany(x => CardSuitsAsStrings, (value, suit) =>
				{
					var cardValueEnum = _utilitiesService.GetEnumValueFromDescription<CardValue>(value);
					var cardSuitEnum = _utilitiesService.GetEnumValueFromDescription<CardSuit>(suit);
					var cardAsString = $"{value}{suit}";
					return new TestCaseData(cardAsString, cardValueEnum, cardSuitEnum).SetName(cardAsString);
				});
			}
		}
	}
}
