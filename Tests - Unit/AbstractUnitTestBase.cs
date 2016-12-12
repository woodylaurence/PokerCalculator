using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace PokerCalculator.Tests.Unit
{
	[TestFixture]
	public class AbstractUnitTestBase
	{
		protected internal IWindsorContainer WindsorContainer { get; set; }

		[SetUp]
		public void Setup()
		{
			WindsorContainer = SetupWindsorContainer();
		}

		protected internal virtual IWindsorContainer SetupWindsorContainer()
		{
			var container = new WindsorContainer();
			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
			return container;
		}
	}
}
