using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;
using Rhino.Mocks;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.Helpers
{
	[TestFixture]
	public class UtilitiesUnitTests : AbstractUnitTestBase
	{
		[SetUp]
		protected override void Setup()
		{
			Utilities.MethodObject = MockRepository.GenerateStrictMock<Utilities>();
		}

		[TearDown]
		protected void TearDown()
		{
			Utilities.MethodObject = new Utilities();
		}

		[Test]
		public void GetEnumValues_SHOULD_call_slave()
		{
			//arrange
			var expected = new List<TestEnum> { TestEnum.EnumValue1, TestEnum.ValueThree };
			Utilities.MethodObject.Stub(x => x.GetEnumValuesSlave<TestEnum>()).Return(expected);

			//act
			var actual = Utilities.GetEnumValues<TestEnum>();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetEnumvalueFromDescription_SHOULD_calls_slave()
		{
			//arrange
			const string enumDescription = "kjashj askljasdf";
			const TestEnum expected = TestEnum.SecondValue;

			Utilities.MethodObject.Stub(x => x.GetEnumValueFromDescriptionSlave<TestEnum>(enumDescription)).Return(expected);

			//act
			var actual = Utilities.GetEnumValueFromDescription<TestEnum>(enumDescription);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GetTicksAsStringWithUnit_SHOULD_call_slave()
		{
			//arrange
			const double ticks = 21523.14;
			const string expected = "kljhas akjhasdf";

			Utilities.MethodObject.Stub(x => x.GetTicksAsStringWithUnitSlave(ticks)).Return(expected);

			//act
			var actual = Utilities.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}
