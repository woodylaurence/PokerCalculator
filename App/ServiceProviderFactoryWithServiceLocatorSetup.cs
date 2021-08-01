using System;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using PokerCalculator.Domain.Helpers;

namespace PokerCalculator.App
{
	public class ServiceProviderFactoryWithServiceLocatorSetup : IServiceProviderFactory<IServiceCollection>
	{
		private readonly ServiceProviderOptions _options;

		public ServiceProviderFactoryWithServiceLocatorSetup() : this(new ServiceProviderOptions()) { }
		public ServiceProviderFactoryWithServiceLocatorSetup(ServiceProviderOptions options)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public IServiceCollection CreateBuilder(IServiceCollection services) => services;
		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
		{
			var serviceProvider = containerBuilder.BuildServiceProvider(_options);
			ServiceLocator.SetLocatorProvider(() => new ServiceProviderBackedServiceLocator(serviceProvider));
			return serviceProvider;
		}
	}
}