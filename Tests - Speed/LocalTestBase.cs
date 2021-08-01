using Microsoft.Extensions.DependencyInjection;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;

namespace PokerCalculator.Tests.Speed
{
	public class LocalTestBase : AbstractUnitTestBase
	{
		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddSingleton<IRandomNumberGenerator, FakeRandomNumberGenerator>();
		}
	}
}
