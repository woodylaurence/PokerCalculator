using PokerCalculator.Domain.PokerEnums;
using System;

namespace PokerCalculator.Domain.HandRankCalculator
{
	public interface IHandRank<THandRank> : IComparable<IHandRank<THandRank>>
	{
		THandRank Rank { get; }
		PokerHand PokerHand { get; }
	}
}
