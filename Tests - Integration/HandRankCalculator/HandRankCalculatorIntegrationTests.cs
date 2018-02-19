using NUnit.Framework;

namespace PokerCalculator.Tests.Integration.HandRankCalculator
{
	[TestFixture]
	public class HandRankCalculatorIntegrationTests : BaseHandRankCalculatorIntegrationTests
	{
		[SetUp]
		public override void Setup()
		{
			_instance = new Domain.HandRankCalculator.HandRankCalculator();

			base.Setup();
		}
	}
}
