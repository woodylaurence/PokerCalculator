using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Integration.PokerCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankPokerCalculatorIntegrationTests : BasePokerCalculatorIntegrationTests
	{
		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>().ImplementedBy<PokerHandBasedHandRankCalculator>());
			windsorContainer.Register(Component.For<IPokerCalculator>().ImplementedBy<PokerHandBasedHandRankPokerCalculator>());
		}
	}
}
