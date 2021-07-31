using Microsoft.Extensions.DependencyInjection;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Speed
{
	public class LocalTestBase : AbstractUnitTestBase
	{
		protected IEqualityComparer<Card> CardComparer = new CardComparer();

		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddSingleton(CardComparer);
			services.AddSingleton<IRandomNumberGenerator, FakeRandomNumberGenerator>();
		}
	}
}
