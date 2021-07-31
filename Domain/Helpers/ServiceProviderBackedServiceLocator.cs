using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Domain.Helpers
{
	public class ServiceProviderBackedServiceLocator : ServiceLocatorImplBase
	{
		private readonly IServiceProvider _serviceProvider;

		public ServiceProviderBackedServiceLocator(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		protected override object DoGetInstance(Type serviceType, string key) => _serviceProvider.GetService(serviceType);
		protected override IEnumerable<object> DoGetAllInstances(Type serviceType) => _serviceProvider.GetServices(serviceType);
	}
}
