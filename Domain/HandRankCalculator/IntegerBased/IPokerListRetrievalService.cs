using System.Collections.Generic;

namespace PokerCalculator.Domain.HandRankCalculator.IntegerBased
{
	public interface IPokerListRetrievalService
	{
		List<int> FlushesList { get; }
	}
}
