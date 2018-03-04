using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared.TestObjects;
using System;

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

		#region GetEnumValueFromDescription

		[Test]
		public void GetEnumValueFromDescription_WHERE_type_to_return_is_not_enum_SHOULD_throw_error()
		{
			//act + assert
			var actualException = Assert.Throws<InvalidOperationException>(() => _instance.GetEnumValueFromDescription<int>("some string"));
			Assert.That(actualException.Message, Is.EqualTo("Type to return must be an enum."));
		}

		[Test]
		public void GetEnumValueFromDescription_WHERE_no_enum_has_name_or_description_matching_supplied_string_SHOULD_throw_error()
		{
			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.GetEnumValueFromDescription<TestEnum>("some string"));
			Assert.That(actualException.Message, Is.EqualTo("Cannot find enum value by supplied string"));
		}

		[Test]
		public void GetEnumValueFromDescription_WHERE_enum_doesnt_exist_with_correct_description_but_one_does_exist_with_correct_name_SHOULD_return_enum_value()
		{
			//act
			var actual = _instance.GetEnumValueFromDescription<TestEnum>("SecondValue");

			//assert
			Assert.That(actual, Is.EqualTo(TestEnum.SecondValue));
		}

		[Test]
		public void GetEnumValueFromDescription_WHERE_enum_exists_with_correct_description_SHOULD_return_enum_value()
		{
			//act
			var actual = _instance.GetEnumValueFromDescription<TestEnum>("The Final Value");

			//assert
			Assert.That(actual, Is.EqualTo(TestEnum.ValueThree));
		}

		#endregion

		#region GetTicksAsStringWithUnit

		[TestCase(100000000, 10)]
		[TestCase(1597536951, 159.8)]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_greater_than_10_seconds_SHOULD_return_formatted_value_with_seconds_as_unit(double ticks, double expected)
		{
			//act
			var actual = _instance.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo($"{expected:N1}s"));
		}

		[TestCase(10000, 1)]
		[TestCase(22000, 2.2)]
		[TestCase(72566753, 7256.7)]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_between_1_millisecond_and_10_seconds_SHOULD_return_formatted_value_with_milliseconds_as_unit(double ticks, double expected)
		{
			//act
			var actual = _instance.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo($"{expected:N1}ms"));
		}

		[TestCase(10, 1)]
		[TestCase(22, 2.2)]
		[TestCase(1597, 159.7)]
		[TestCase(9999.1, 999.9)]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_between_1_microsecond_and_1_millisecond_SHOULD_return_formatted_value_with_microseconds_as_unit(double ticks, double expected)
		{
			//act
			var actual = _instance.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo($"{expected:N1}μs"));
		}

		[TestCase(9, 900)]
		[TestCase(1.2, 120)]
		[TestCase(0.01, 1)]
		[TestCase(0.00159, 0.2)]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_less_than_1_microsecond_SHOULD_return_formatted_value_with_nanseconds_as_unit(double ticks, double expected)
		{
			//act
			var actual = _instance.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo($"{expected:N1}ns"));
		}

		#endregion
	}
}
