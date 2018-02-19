using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class LocalTestBase : AbstractUnitTestBase
	{
		protected IEqualityComparer<Card> CardComparer = new CardComparer();
		protected IUtilitiesService UtilitiesService = new UtilitiesService();

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IEqualityComparer<Card>>().Instance(CardComparer));
			windsorContainer.Register(Component.For<IUtilitiesService>().Instance(UtilitiesService));
		}
	}
}
