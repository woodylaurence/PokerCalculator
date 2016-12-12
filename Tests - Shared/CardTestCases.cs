using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Shared
{
	public static class CardTestCases
	{
		public static IEnumerable<TestCaseData> AllCardsTestCaseData
		{
			get
			{
				return AllCards.Select(x => new TestCaseData(x.Value, x.Suit));
			}
		}

		public static List<Card> AllCards
		{
			get
			{
				var cardSuits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToList();
				var cardValues = Enum.GetValues(typeof(CardValue)).Cast<CardValue>().ToList();

				return cardSuits.SelectMany(cardSuit => cardValues.Select(cardValue => Card.Create(cardValue, cardSuit))).ToList();
			}
		}
	}
}
