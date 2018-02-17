using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace PokerCalculator.Tests.Shared
{
	[TestFixture]
	public class AbstractUnitTestBase
	{
		protected internal IWindsorContainer WindsorContainer { get; set; }

		[SetUp]
		public virtual void Setup()
		{
			WindsorContainer = SetupWindsorContainer();
			RegisterComponentsToWindsor(WindsorContainer);
		}

		protected internal virtual IWindsorContainer SetupWindsorContainer()
		{
			var container = new WindsorContainer();
			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
			return container;
		}

		protected internal virtual void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{

		}
	}
}
