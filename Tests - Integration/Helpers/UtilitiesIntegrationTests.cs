using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared.TestObjects;
using System;
using System.Diagnostics;

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

		[Test]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_greater_than_10_seconds_SHOULD_return_formatted_value_with_seconds_as_unit()
		{
			//arrange
			var ticks1 = Stopwatch.Frequency * 10;
			var ticks2 = Stopwatch.Frequency * 159.8;

			//act
			var actual1 = _instance.GetTicksAsStringWithUnit(ticks1);
			var actual2 = _instance.GetTicksAsStringWithUnit(ticks2);

			//assert
			Assert.That(actual1, Is.EqualTo("10.0s"));
			Assert.That(actual2, Is.EqualTo("159.8s"));
		}

		[Test]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_between_1_millisecond_and_10_seconds_SHOULD_return_formatted_value_with_milliseconds_as_unit()
		{
			//arrange
			var ticks1 = Stopwatch.Frequency * 0.001;
			var ticks2 = Stopwatch.Frequency * 0.0022;
			var ticks3 = Stopwatch.Frequency * 7.2567;

			//act
			var actual1 = _instance.GetTicksAsStringWithUnit(ticks1);
			var actual2 = _instance.GetTicksAsStringWithUnit(ticks2);
			var actual3 = _instance.GetTicksAsStringWithUnit(ticks3);

			//assert
			Assert.That(actual1, Is.EqualTo("1.0ms"));
			Assert.That(actual2, Is.EqualTo("2.2ms"));
			Assert.That(actual3, Is.EqualTo("7,256.7ms"));
		}

		[Test]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_between_1_microsecond_and_1_millisecond_SHOULD_return_formatted_value_with_microseconds_as_unit()
		{
			//arrange
			var ticks1 = Stopwatch.Frequency * 0.000001;
			var ticks2 = Stopwatch.Frequency * 0.0000022;
			var ticks3 = Stopwatch.Frequency * 0.0001597;
			var ticks4 = Stopwatch.Frequency * 0.00099991;

			//act
			var actual1 = _instance.GetTicksAsStringWithUnit(ticks1);
			var actual2 = _instance.GetTicksAsStringWithUnit(ticks2);
			var actual3 = _instance.GetTicksAsStringWithUnit(ticks3);
			var actual4 = _instance.GetTicksAsStringWithUnit(ticks4);

			//assert
			Assert.That(actual1, Is.EqualTo("1.0μs"));
			Assert.That(actual2, Is.EqualTo("2.2μs"));
			Assert.That(actual3, Is.EqualTo("159.7μs"));
			Assert.That(actual4, Is.EqualTo("999.9μs"));
		}

		[Test]
		public void GetTicksAsStringWithUnit_WHERE_ticks_is_less_than_1_microsecond_SHOULD_return_formatted_value_with_nanseconds_as_unit()
		{
			//arrange
			var ticks1 = Stopwatch.Frequency * 0.0000009;
			var ticks2 = Stopwatch.Frequency * 0.00000012;
			var ticks3 = Stopwatch.Frequency * 0.000000001;
			var ticks4 = Stopwatch.Frequency * 0.000000000159;

			//act
			var actual1 = _instance.GetTicksAsStringWithUnit(ticks1);
			var actual2 = _instance.GetTicksAsStringWithUnit(ticks2);
			var actual3 = _instance.GetTicksAsStringWithUnit(ticks3);
			var actual4 = _instance.GetTicksAsStringWithUnit(ticks4);

			//assert
			Assert.That(actual1, Is.EqualTo("900.0ns"));
			Assert.That(actual2, Is.EqualTo("120.0ns"));
			Assert.That(actual3, Is.EqualTo("1.0ns"));
			Assert.That(actual4, Is.EqualTo("0.2ns"));
		}

		#endregion
	}
}
