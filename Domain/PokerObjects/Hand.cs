using System.Collections.Generic;
using System;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Hand
	{
		public virtual List<Card> Cards { get; set; }

		public static Hand Create (List<Card> cards = null)
		{
			throw new NotImplementedException();
		}

		public virtual void AddCard(Card cardToAdd)
		{
			throw new NotImplementedException();
		}
	}
}
