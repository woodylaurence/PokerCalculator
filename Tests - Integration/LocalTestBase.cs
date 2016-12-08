using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class LocalTestBase
	{
		[SetUp]
		public void Setup()
		{
			var windsorContainer = SetupWindsorContainer();
			RegisterComponentsToWindsor(windsorContainer);
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
