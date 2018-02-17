using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Tests.Shared.TestObjects;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class UtilitiesIntegrationTests : LocalTestBase
	{
		#region GetEnumValues

		[Test]
		public void GetEnumValues()
		{
			//act
			var actual = Utilities.GetEnumValues<TestEnum>();

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(TestEnum.EnumValue1));
			Assert.That(actual[1], Is.EqualTo(TestEnum.SecondValue));
			Assert.That(actual[2], Is.EqualTo(TestEnum.ValueThree));
		}

		#endregion
	}
}
