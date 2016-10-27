using System;
using System.Collections.Generic;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Domain
{
	public class CardComparer : IEqualityComparer<Card>
	{
		public virtual bool Equals(Card x, Card y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (x == null || y == null) return false;
			return x.Value == y.Value && x.Suit == y.Suit;
		}

		public virtual int GetHashCode(Card card)
		{
			return (31 * card.Value.GetHashCode()) + (97 * card.Suit.GetHashCode());
		}
	}
}
