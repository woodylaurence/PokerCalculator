using System.Collections.Generic;
using System;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Hand
	{
		public static Hand Create (List<Card> cards)
		{
			throw new NotImplementedException();
		}

		public virtual void AddCard(Card cardToAdd)
		{
			throw new NotImplementedException();
		}
	}
}
