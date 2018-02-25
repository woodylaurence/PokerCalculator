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
			throw new NotImplementedException("need to be careful to cut kickers down to correct length, we're getting differing number of kickers for the same hand rank");
			KickerCardValues = kickerCardValues ?? new List<CardValue>();
		}

		#endregion

		#region Operator Overloads

		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator <(HandRank left, HandRank right) => left.Operator_LessThan(right);
		protected internal virtual bool Operator_LessThan(HandRank otherHandRank) => CompareTo(otherHandRank) == -1;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator >(HandRank left, HandRank right) => left.Operator_GreaterThan(right);
		protected internal virtual bool Operator_GreaterThan(HandRank otherHandRank) => CompareTo(otherHandRank) == 1;

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
			throw new NotImplementedException("Decide whether we want the below line");
			if (KickerCardValues.Count != otherHandRank.KickerCardValues.Count)
				throw new Exception($"Kickers have different lengths - mine : {PokerHand}, theirs : {otherHandRank.PokerHand}");
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

		#endregion
	}
}
