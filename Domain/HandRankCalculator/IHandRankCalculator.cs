using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Domain.HandRankCalculator
{
	public interface IHandRankCalculator<out THandRank, TRank> where THandRank : IHandRank<TRank>
	{
		THandRank CalculateHandRank(Hand hand);
	}
}
