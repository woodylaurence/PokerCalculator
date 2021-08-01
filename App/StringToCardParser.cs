using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Text.RegularExpressions;

namespace PokerCalculator.App
{
	public class StringToCardParser
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cardAsString"></param>
		/// <returns></returns>
		public Card ParseCard(string cardAsString)
		{
			if (string.IsNullOrWhiteSpace(cardAsString)) throw new ArgumentException("Must provide string representation of Card");

			var cardRegex = new Regex("^([AKQJ2-9]|10)([SHDC])$", RegexOptions.IgnoreCase);
			if (cardRegex.IsMatch(cardAsString) == false) throw new ArgumentException("Supplied string does not conform to allowed Card values");
			var cardValueAsString = cardRegex.Match(cardAsString).Groups[1].Value.ToUpper();
			var cardSuitAsString = cardRegex.Match(cardAsString).Groups[2].Value.ToUpper();

			var value = Utilities.GetEnumValueFromDescription<CardValue>(cardValueAsString);
			var suit = Utilities.GetEnumValueFromDescription<CardSuit>(cardSuitAsString);

			return new Card(value, suit);
		}
	}
}
