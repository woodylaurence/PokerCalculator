using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Speed.HandRankCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankCalculatorSpeedTests : BaseHandRankCalculatorSpeedTests<PokerHandBasedHandRank, PokerHand>
	{
		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddSingleton<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>, PokerHandBasedHandRankCalculator>();
		}
	}
}
