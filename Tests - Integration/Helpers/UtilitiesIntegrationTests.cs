using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared.TestObjects;

namespace PokerCalculator.Tests.Integration.Helpers
{
	[TestFixture]
	public class UtilitiesIntegrationTests : LocalTestBase
	{
		private UtilitiesService _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = ServiceLocator.Current.GetInstance<IUtilitiesService>() as UtilitiesService;
		}

		#region GetEnumValues

		[Test]
		public void GetEnumValues()
		{
			//act
			var actual = _instance.GetEnumValues<TestEnum>();

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(TestEnum.EnumValue1));
			Assert.That(actual[1], Is.EqualTo(TestEnum.SecondValue));
			Assert.That(actual[2], Is.EqualTo(TestEnum.ValueThree));
		}

		#endregion
	}
}
