using PokerCalculator.Domain.PokerEnums;
using System;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Card : IEquatable<Card>
	{
		public CardValue Value { get; }
		public CardSuit Suit { get; }

		public Card(CardValue value, CardSuit suit)
		{
			Value = value;
			Suit = suit;
		}

		#region IEquatable Implementation

		public bool Equals(Card other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other == null) return false;
			return Value == other.Value && Suit == other.Suit;
		}

		public override bool Equals(object obj) => obj is Card card && Equals(card);
		public override int GetHashCode() => HashCode.Combine((int)Value, (int)Suit);

		#endregion

		#region Equality Operators

		public static bool operator ==(Card lhs, Card rhs) => lhs?.Equals(rhs) ?? false;
		public static bool operator !=(Card lhs, Card rhs) => (lhs == rhs) == false;

		#endregion
	}
}
