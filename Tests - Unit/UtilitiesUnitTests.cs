using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;
using Rhino.Mocks;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit
{
	[TestFixture]
	public class UtilitiesUnitTests : AbstractUnitTestBase
	{
		[SetUp]
		public override void Setup()
		{
			base.Setup();

			Utilities.MethodObject = MockRepository.GenerateMock<Utilities>();
		}

		[TearDown]
		public void TearDown()
		{
			Utilities.MethodObject = new Utilities();
		}

		#region GetEnumValues

		[Test]
		public void GetEnumValues_calls_slave()
		{
			//arrange
			var expected = new List<TestEnum> { TestEnum.SecondValue };
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<TestEnum>()).Return(expected);

			//act
			var actual = Utilities.GetEnumValues<TestEnum>();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
