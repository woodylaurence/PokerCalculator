using System;
using System.Collections.Generic;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Deck : List<Card>
	{
		public static Deck Create()
		{
			throw new NotImplementedException();
		}

		public static Deck CreateShuffledDeck()
		{
			throw new NotImplementedException();
		}

		public virtual void Shuffle()
		{
			throw new NotImplementedException();
		}
	}
}
