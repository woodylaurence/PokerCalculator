using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using System;
using System.Text.RegularExpressions;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Card
	{
		#region Properties and Fields

		public virtual CardValue Value { get; }
		public virtual CardSuit Suit { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="suit"></param>
		public Card(CardValue value, CardSuit suit)
		{
			Value = value;
			Suit = suit;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cardAsString"></param>
		/// <param name="utilitiesService"></param>
		public Card(string cardAsString, IUtilitiesService utilitiesService)
		{
			if (string.IsNullOrWhiteSpace(cardAsString)) throw new ArgumentException("Must provide string representation of Card");

			var cardRegex = new Regex("^([AKQJ2-9]|10)([SHDC])$", RegexOptions.IgnoreCase);
			if (cardRegex.IsMatch(cardAsString) == false) throw new ArgumentException("Supplied string does not conform to allowed Card values");
			var cardValueAsString = cardRegex.Match(cardAsString).Groups[1].Value.ToUpper();
			var cardSuitAsString = cardRegex.Match(cardAsString).Groups[2].Value.ToUpper();

			Value = utilitiesService.GetEnumValueFromDescription<CardValue>(cardValueAsString);
			Suit = utilitiesService.GetEnumValueFromDescription<CardSuit>(cardSuitAsString);
		}

		#endregion
	}
}
