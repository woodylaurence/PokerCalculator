using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.Domain.TestObjects;
using System;
using System.Diagnostics;

namespace PokerCalculator.Tests.Unit.Domain.Helpers
{
	[TestFixture]
	public class UtilitiesUnitTests : AbstractUnitTestBase
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

		#region GetEnumValueFromDescription

		[Test]
		public void GetEnumValueFromDescription_WHERE_type_to_return_is_not_enum_SHOULD_throw_error()
		{
			//act + assert
			var actualException = Assert.Throws<InvalidOperationException>(() => Utilities.GetEnumValueFromDescription<int>("some string"));
			Assert.That(actualException.Message, Is.EqualTo("Type to return must be an enum."));
		}

		[Test]
		public void GetEnumValueFromDescription_WHERE_no_enum_has_name_or_description_matching_supplied_string_SHOULD_throw_error()
		{
			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => Utilities.GetEnumValueFromDescription<TestEnum>("some string"));
			Assert.That(actualException.Message, Is.EqualTo("Cannot find enum value by supplied string"));
		}

		[Test]
		public void GetEnumValueFromDescription_WHERE_enum_doesnt_exist_with_correct_description_but_one_does_exist_with_correct_name_SHOULD_return_enum_value()
		{
			//act
			var actual = Utilities.GetEnumValueFromDescription<TestEnum>("SecondValue");

			//assert
			Assert.That(actual, Is.EqualTo(TestEnum.SecondValue));
		}

		[Test]
		public void GetEnumValueFromDescription_WHERE_enum_exists_with_correct_description_SHOULD_return_enum_value()
		{
			//act
			var actual = Utilities.GetEnumValueFromDescription<TestEnum>("The Final Value");

			//assert
			Assert.That(actual, Is.EqualTo(TestEnum.ValueThree));
		}

		#endregion

		#region GetTicksAsStringWithUnit

		[TestCase(10, "10.0s")]
		[TestCase(159.8, "159.8s")]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_greater_than_10_seconds_SHOULD_return_formatted_value_with_seconds_as_unit(double frequencyMultiplier, string expected)
		{
			//arrange
			var ticks = Stopwatch.Frequency * frequencyMultiplier;

			//act
			var actual = Utilities.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(0.001, "1.0ms")]
		[TestCase(0.0022, "2.2ms")]
		[TestCase(7.2567, "7,256.7ms")]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_between_1_millisecond_and_10_seconds_SHOULD_return_formatted_value_with_milliseconds_as_unit(double frequencyMultiplier, string expected)
		{
			//arrange
			var ticks = Stopwatch.Frequency * frequencyMultiplier;

			//act
			var actual = Utilities.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(0.000001, "1.0μs")]
		[TestCase(0.0000022, "2.2μs")]
		[TestCase(0.0001597, "159.7μs")]
		[TestCase(0.00099991, "999.9μs")]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_between_1_microsecond_and_1_millisecond_SHOULD_return_formatted_value_with_microseconds_as_unit(double frequencyMultiplier, string expected)
		{
			//arrange
			var ticks = Stopwatch.Frequency * frequencyMultiplier;

			//act
			var actual = Utilities.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(0.0000009, "900.0ns")]
		[TestCase(0.00000012, "120.0ns")]
		[TestCase(0.000000001, "1.0ns")]
		[TestCase(0.000000000159, "0.2ns")]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_less_than_1_microsecond_SHOULD_return_formatted_value_with_nanseconds_as_unit(double frequencyMultiplier, string expected)
		{
			//arrange
			var ticks = Stopwatch.Frequency * frequencyMultiplier;

			//act
			var actual = Utilities.GetTicksAsStringWithUnit(ticks);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
