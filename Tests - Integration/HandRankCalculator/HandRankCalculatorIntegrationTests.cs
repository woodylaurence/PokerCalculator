using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;

namespace PokerCalculator.Tests.Integration.HandRankCalculator
{
	[TestFixture]
	public class HandRankCalculatorIntegrationTests : BaseHandRankCalculatorIntegrationTests
	{
		[SetUp]
		protected override void Setup()
		{
			base.Setup();
			_instance = ServiceLocator.Current.GetInstance<IHandRankCalculator>();
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IHandRankCalculator>().ImplementedBy<Domain.HandRankCalculator.HandRankCalculator>());
		}
	}
}
