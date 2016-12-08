using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Domain.HandRankCalculator
{
	public interface IHandRankCalculator
	{
		HandRank CalculateHandRank(Hand hand);
	}
}
