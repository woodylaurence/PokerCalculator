﻿using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class LocalTestBase
	{
		protected internal virtual IWindsorContainer WindsorContainer { get; set; }

		[SetUp]
		public void Setup()
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
