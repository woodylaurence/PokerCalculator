using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;

namespace PokerCalculator.Domain.Helpers
{
	public class CardComparer : IEqualityComparer<Card>
	{
		/// <summary>
		/// Equals the specified x and y.
		/// </summary>
		/// <param name="x">The first Card</param>
		/// <param name="y">The second Card</param>
		public virtual bool Equals(Card x, Card y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (x == null || y == null) return false;
			return x.Value == y.Value && x.Suit == y.Suit;
		}

		/// <summary>
		/// Gets the hash code.
		/// </summary>
		/// <returns>The hash code.</returns>
		/// <param name="card">Card.</param>
		public virtual int GetHashCode(Card card)
		{
			return (31 * card.Value.GetHashCode()) + (97 * card.Suit.GetHashCode());
		}
	}
}
