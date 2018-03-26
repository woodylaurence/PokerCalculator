using PokerCalculator.Domain.PokerEnums;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Domain.HandRankCalculator
{
	public class PokerHandBasedHandRank : IHandRank<PokerHand>
	{
		#region Properties and Fields

		public virtual PokerHand Rank { get; }
		public virtual PokerHand PokerHand => Rank;
		public virtual List<CardValue> KickerCardValues { get; }

		#endregion

		#region Constructor

		public PokerHandBasedHandRank(PokerHand pokerHand, List<CardValue> kickerCardValues = null)
		{
			Rank = pokerHand;
			KickerCardValues = kickerCardValues ?? new List<CardValue>();
		}

		#endregion

		#region IComparable Implementation

		/// <inheritdoc />
		public virtual int CompareTo(IHandRank<PokerHand> otherHandRank)
		{
			if (ReferenceEquals(this, otherHandRank)) return 0;
			if (ReferenceEquals(null, otherHandRank)) return 1;
			if (Rank < otherHandRank.Rank) return -1;
			if (Rank > otherHandRank.Rank) return 1;

			if (otherHandRank is PokerHandBasedHandRank pokerHandBasedHandRank) return CompareKickers(pokerHandBasedHandRank);
			throw new ArgumentException("Other HandRank is not PokerHandBasedHandRank");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="otherHandRank"></param>
		/// <returns></returns>
		protected internal virtual int CompareKickers(PokerHandBasedHandRank otherHandRank)
		{
			if (KickerCardValues.Count != otherHandRank.KickerCardValues.Count) throw new Exception("Kickers have different lengths.");
			for (var i = 0; i < KickerCardValues.Count; i++)
			{
				var kicker = KickerCardValues[i];
				var otherKicker = otherHandRank.KickerCardValues[i];
				if (kicker < otherKicker) return -1;
				if (kicker > otherKicker) return 1;
			}
			return 0;
		}

		#endregion
	}
}
