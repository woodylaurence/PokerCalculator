using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;

namespace PokerCalculator.Tests.Speed.HandRankCalculator
{
	[TestFixture]
	public class HandRankCalculatorSpeedTests : BaseHandRankCalculatorSpeedTests
	{
		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IHandRankCalculator>().ImplementedBy<Domain.HandRankCalculator.HandRankCalculator>());
		}
	}
}
