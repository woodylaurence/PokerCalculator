using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace PokerCalculator.Tests.Shared
{
	public class AbstractUnitTestBase
	{
		protected internal IWindsorContainer WindsorContainer { get; set; }

		[SetUp]
		protected virtual void Setup()
		{
			WindsorContainer = SetupWindsorContainer();
			RegisterComponentsToWindsor(WindsorContainer);
		}

		private IWindsorContainer SetupWindsorContainer()
		{
			var container = new WindsorContainer();
			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
			return container;
		}

		protected virtual void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{

		}
	}
}
