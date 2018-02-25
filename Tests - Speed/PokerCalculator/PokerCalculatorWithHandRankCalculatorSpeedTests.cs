using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerCalculator;

namespace PokerCalculator.Tests.Speed.PokerCalculator
{
	[TestFixture]
	public class PokerCalculatorWithHandRankCalculatorSpeedTests : BasePokerCalculatorSpeedTests
	{
		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IHandRankCalculator>().ImplementedBy<Domain.HandRankCalculator.HandRankCalculator>());
			windsorContainer.Register(Component.For<IPokerCalculator>().ImplementedBy<Domain.PokerCalculator.PokerCalculator>());
		}
	}
}
