using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Domain.PokerCalculator
{
	public interface IPokerCalculator
	{
		PokerOdds CalculatePokerOdds(Deck deck, Hand myHand, Hand boardHand, int numOpponents, int numIterations);
	}
}
