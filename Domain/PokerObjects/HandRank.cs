using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Domain.PokerObjects
{
	public class HandRank
	{
		internal static HandRank MethodObject = new HandRank();

		public virtual PokerHand PokerHand { get; set; }
		public List<Card> KickerCards { get; set; }

		public static HandRank Create(PokerHand pokerHand, List<Card> kickerCards = null) { return MethodObject.CreateSlave(pokerHand, kickerCards); }
		protected internal virtual HandRank CreateSlave(PokerHand pokerHand, List<Card> kickerCards)
		{
			return new HandRank { PokerHand = pokerHand, KickerCards = kickerCards ?? new List<Card>() };
		}
	}
}
