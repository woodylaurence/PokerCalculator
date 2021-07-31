using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using System;

namespace PokerCalculator.Tests.Shared
{
	public class AbstractUnitTestBase
	{
		protected IServiceProvider ServiceProvider { get; private set; }

		[SetUp]
		protected virtual void Setup()
		{
			var services = new ServiceCollection();
			RegisterServices(services);

			ServiceProvider = services.BuildServiceProvider();
			ServiceLocator.SetLocatorProvider(() => new ServiceProviderBackedServiceLocator(ServiceProvider));
		}

		protected virtual void RegisterServices(IServiceCollection services)
		{

		}
	}
}
