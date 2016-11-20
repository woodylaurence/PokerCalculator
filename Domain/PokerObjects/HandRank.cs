using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Domain.PokerObjects
{
	public class HandRank
	{
		internal static HandRank MethodObject = new HandRank();

		public virtual PokerHand PokerHand { get; set; }
		public List<CardValue> KickerCardValues { get; set; }

		/// <summary>
		/// Create the specified pokerHand and kickerCardValues.
		/// </summary>
		/// <param name="pokerHand">Poker hand.</param>
		/// <param name="kickerCardValues">Kicker card values.</param>
		public static HandRank Create(PokerHand pokerHand, List<CardValue> kickerCardValues = null) { return MethodObject.CreateSlave(pokerHand, kickerCardValues); }
		protected internal virtual HandRank CreateSlave(PokerHand pokerHand, List<CardValue> kickerCardValues)
		{
			return new HandRank { PokerHand = pokerHand, KickerCardValues = kickerCardValues ?? new List<CardValue>() };
		}
	}
}
