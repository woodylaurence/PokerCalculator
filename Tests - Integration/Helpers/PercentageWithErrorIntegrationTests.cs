using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.Helpers
{
	[TestFixture]
	public class PercentageWithErrorIntegrationTests : LocalTestBase
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
			const double percentage1 = 0.562;
			const double percentage2 = 0.534;
			const double percentage3 = 0.581;
			const double percentage4 = 0.701;
			const double percentage5 = 0.498;

			var percentages = new List<double> { percentage1, percentage2, percentage3, percentage4, percentage5 };

			//act
			var actual = new PercentageWithError(percentages);

			//assert
			Assert.That(actual.Percentage, Is.EqualTo(0.5752));
			Assert.That(actual.Error, Is.EqualTo(0.0688342).Within(0.0000001));
		}

		[Test]
		public void Constructor_values()
		{
			//arrange
			const double percentage = 5.15;
			const double error = 0.574;

			//act
			var actual = new PercentageWithError(percentage, error);

			//assert
			Assert.That(actual.Percentage, Is.EqualTo(percentage));
			Assert.That(actual.Error, Is.EqualTo(error));
		}

		#endregion

		#region Instance Methods

		#region ToString

		[TestCase(100, "10000%")]
		[TestCase(0.156968, "15.6968%")]
		[TestCase(0.1, "10%")]
		public void ToString_WHERE_error_is_zero_SHOULD_output_percentage_without_error_with_all_decimal_places(double percentage, string expected)
		{
			//arrange
			var instance = new PercentageWithError(percentage, 0);

			//act
			var actual = instance.ToString();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(0.7864, 0.0671, "78.6% ± 6.7%")]
		[TestCase(0.795, 0.0015, "79.5% ± 0.2%")]
		public void ToString_WHERE_error_is_non_zero_SHOULD_output_percentage_with_error_to_1_decimal_place(double percentage, double error, string expected)
		{
			//arrange
			var instance = new PercentageWithError(percentage, error);

			//act
			var actual = instance.ToString();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#endregion
	}
}
