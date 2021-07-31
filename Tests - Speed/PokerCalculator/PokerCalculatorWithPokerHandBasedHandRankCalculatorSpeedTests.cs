using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Speed.PokerCalculator
{
	[TestFixture]
	public class PokerCalculatorWithPokerHandBasedHandRankCalculatorSpeedTests : BasePokerCalculatorSpeedTests
	{
		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddSingleton<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>, PokerHandBasedHandRankCalculator>();
			services.AddSingleton<IPokerCalculator, PokerHandBasedHandRankPokerCalculator>();
		}
	}
}
