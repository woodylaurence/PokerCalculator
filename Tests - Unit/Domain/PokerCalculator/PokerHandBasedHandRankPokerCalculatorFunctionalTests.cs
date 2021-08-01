using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerCalculator;

namespace PokerCalculator.Tests.Unit.Domain.PokerCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankPokerCalculatorFunctionalTests : BasePokerCalculatorFunctionalTests
	{
		protected override IPokerCalculator SetupPokerCalculator()
		{
			return new PokerHandBasedHandRankPokerCalculator(new PokerHandBasedHandRankCalculator());
		}
	}
}