using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;

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
			container.Register(Component.For<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>().ImplementedBy<PokerHandBasedHandRankCalculator>().LifestyleTransient());
			container.Register(Component.For<IPokerCalculator>().ImplementedBy<PokerHandBasedHandRankPokerCalculator>().LifestyleTransient());
		}
	}
}
