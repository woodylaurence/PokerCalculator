using System;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Card
	{
		#region Properties and Fields

		public virtual CardValue Value { get; }
		public virtual CardSuit Suit { get; }

		#endregion

		#region Constructors

		public Card(CardValue value, CardSuit suit)
		{
			Value = value;
			Suit = suit;
		}

		public Card(string cardAsString)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
