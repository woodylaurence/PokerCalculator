using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.App
{
	public class WindsorContainerUtilities
	{
		public static void SetupAndConfigureWindsorContainer()
		{
			var windsorContainer = new WindsorContainer();
			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(windsorContainer));
			RegisterComponentsToWindsor(windsorContainer);
		}

		private static void RegisterComponentsToWindsor(IWindsorContainer container)
		{
			container.Register(Component.For<IUtilitiesService>().ImplementedBy<UtilitiesService>().LifestyleTransient());
			container.Register(Component.For<IEqualityComparer<Card>>().ImplementedBy<CardComparer>().LifestyleTransient());
			container.Register(Component.For<IRandomNumberGenerator>().ImplementedBy<RandomNumberGenerator>().LifestyleTransient());
			container.Register(Component.For<IHandRankCalculator>().ImplementedBy<HandRankCalculator>().LifestyleTransient());
			container.Register(Component.For<IPokerCalculator>().ImplementedBy<Domain.PokerCalculator.PokerCalculator>().LifestyleTransient());
		}
	}
}
