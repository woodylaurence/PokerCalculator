using NUnit.Framework;
using PokerCalculator.Domain;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class MyRandomIntegrationTests : LocalTestBase
	{
		#region GenerateRandomNumber

		[Test]
		public void GenerateRandomNumber_with_min_max_values_SHOULD_return_number_between_min_and_max_values()
		{
			//arrange
			const int minValue = 15;
			const int maxValue = 97;

			//act
			var actual = MyRandom.GenerateRandomNumber(minValue, maxValue);

			//assert
			Assert.That(actual, Is.GreaterThanOrEqualTo(minValue));
			Assert.That(actual, Is.LessThanOrEqualTo(maxValue));
		}

		[Test]
		public void GenerateRandomNumber_with_max_value_SHOULD_return_number_between_zero_and_max_values()
		{
			//arrange
			const int maxValue = 497;

			//act
			var actual = MyRandom.GenerateRandomNumber(maxValue);

			//assert
			Assert.That(actual, Is.GreaterThanOrEqualTo(0));
			Assert.That(actual, Is.LessThanOrEqualTo(maxValue));
		}

		#endregion
	}
}
