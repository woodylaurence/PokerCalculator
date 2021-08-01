using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.Domain.Helpers
{
	[TestFixture]
	public class PercentageWithErrorUnitTests : AbstractUnitTestBase
	{

		#region Constructor

		[Test]
		public void Constructor_WHERE_supplying_empty_list_of_percentages_SHOULD_throw_error()
		{
			//act
			var actualException = Assert.Throws<ArgumentException>(() => new PercentageWithError(new List<double>()));
			Assert.That(actualException.Message, Is.EqualTo("Cannot calculate error with less than two percentages"));
		}

		[Test]
		public void Constructor_WHERE_supplying_list_with_single_percentage_SHOULD_throw_error()
		{
			//act
			var actualException = Assert.Throws<ArgumentException>(() => new PercentageWithError(new List<double> { 5.1 }));
			Assert.That(actualException.Message, Is.EqualTo("Cannot calculate error with less than two percentages"));
		}

		[Test]
		public void Constructor_list_of_percentages()
		{
			//arrange
			var percentages = new List<double> { 0.562, 0.534, 0.581, 0.701, 0.498 };

			//act
			var actual = new PercentageWithError(percentages);

			//assert
			Assert.That(actual.Percentage, Is.EqualTo(0.5752));
			Assert.That(actual.Error, Is.EqualTo(0.0688342).Within(0.0000001));
		}

		#endregion

		#region ToString

		[Test]
		public void ToString_WHERE_error_is_zero_SHOULD_output_percentage_without_error_with_all_decimal_places()
		{
			//arrange
			const double percentage = 0.5816521131;
			var instance = new PercentageWithError(new List<double> { percentage, percentage, percentage });

			//act
			var actual = instance.ToString();

			//assert
			Assert.That(actual, Is.EqualTo($"{percentage * 100}%"));
		}

		[TestCase(0.1679, 0.1749, 0.1669, "17.0% ± 0.4%")]
		[TestCase(0.6798, 0.6437, 0.6669, "66.3% ± 1.5%")]
		public void ToString_WHERE_error_is_non_zero_SHOULD_output_percentage_with_error_to_one_decimal_place(double percentageValue1, double percentageValue2, double percentageValue3, string expected)
		{
			//arrange
			var instance = new PercentageWithError(new List<double> { percentageValue1, percentageValue2, percentageValue3 });

			//act
			var actual = instance.ToString();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
