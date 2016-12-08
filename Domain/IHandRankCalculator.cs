using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Domain
{
	public interface IHandRankCalculator
	{
		HandRank CalculateHandRank(Hand hand);
	}
}
