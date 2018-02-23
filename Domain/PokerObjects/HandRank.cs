using PokerCalculator.Domain.PokerEnums;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Domain.PokerObjects
{
	public class HandRank : IComparable<HandRank>
	{
		#region Properties and Fields

		public virtual PokerHand PokerHand { get; }
		public virtual List<CardValue> KickerCardValues { get; }

		#endregion

		#region Constructor

		public HandRank(PokerHand pokerHand, List<CardValue> kickerCardValues = null)
		{
			PokerHand = pokerHand;
			KickerCardValues = kickerCardValues ?? new List<CardValue>();
		}

		#endregion

		#region IComparable

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="otherHandRank"></param>
		/// <returns></returns>
		public virtual int CompareTo(HandRank otherHandRank)
		{
			if (ReferenceEquals(this, otherHandRank)) return 0;
			if (ReferenceEquals(null, otherHandRank)) return 1;
			if (PokerHand < otherHandRank.PokerHand) return -1;
			if (PokerHand > otherHandRank.PokerHand) return 1;
			return CompareKickers(otherHandRank);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="otherHandRank"></param>
		/// <returns></returns>
		protected internal virtual int CompareKickers(HandRank otherHandRank)
		{
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

		#region Operator Overloads

		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator <(HandRank left, HandRank right)
		{
			return left.CompareTo(right) == -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator >(HandRank left, HandRank right)
		{
			return left.CompareTo(right) == 1;
		}

		#endregion
	}
}
