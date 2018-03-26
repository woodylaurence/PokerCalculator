using Castle.MicroKernel.Registration;
using Castle.Windsor;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Speed
{
	public class LocalTestBase : AbstractUnitTestBase
	{
		protected IEqualityComparer<Card> CardComparer = new CardComparer();

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IEqualityComparer<Card>>().Instance(CardComparer));
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().ImplementedBy<FakeRandomNumberGenerator>());
		}
	}
}
