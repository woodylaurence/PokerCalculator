using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Speed.HandRankCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankCalculatorSpeedTests : BaseHandRankCalculatorSpeedTests<PokerHandBasedHandRank, PokerHand>
	{
		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>().ImplementedBy<PokerHandBasedHandRankCalculator>());
		}
	}
}
