using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokerCalculator.App.Configuration;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;

namespace PokerCalculator.App
{
	class Program
	{
		static void Main(string[] args)
		{
			Host.CreateDefaultBuilder()
				.ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json", false, true))
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<IEqualityComparer<Card>, CardComparer>();
					services.AddTransient<IRandomNumberGenerator, RandomNumberGenerator>();
					services.AddTransient<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>, PokerHandBasedHandRankCalculator>();
					services.AddTransient<IPokerCalculator, PokerHandBasedHandRankPokerCalculator>();
					services.Configure<AppSettings>(context.Configuration.GetSection(nameof(AppSettings)));

					services.AddHostedService<PokerCalculatorConsoleAppHostedService>();
				})
				.UseDefaultServiceProvider(x => x.ValidateOnBuild = true)
				.UseServiceProviderFactory(new ServiceProviderFactoryWithServiceLocatorSetup())
				.RunConsoleAsync();
		}
	}
}
