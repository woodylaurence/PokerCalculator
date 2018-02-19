using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Domain.PokerObjects
{
	public class HandRank
	{
		public virtual PokerHand PokerHand { get; set; }
		public List<CardValue> KickerCardValues { get; set; }

		public HandRank(PokerHand pokerHand, List<CardValue> kickerCardValues = null)
		{
			PokerHand = pokerHand;
			KickerCardValues = kickerCardValues ?? new List<CardValue>();
		}
	}
}
