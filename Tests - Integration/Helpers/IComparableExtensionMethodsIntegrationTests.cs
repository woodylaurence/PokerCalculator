using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared.TestObjects;

namespace PokerCalculator.Tests.Integration.Helpers
{
	[TestFixture]
	public class IComparableExtensionMethodsIntegrationTests : LocalTestBase
	{
		private ComparableObject _comparableObject;

		[SetUp]
		protected override void Setup()
		{
			_comparableObject = new ComparableObject { IntegerField = 10 };
		}


		#region IsLessThan

		[Test]
		public void IsLessThan_WHERE_two_objects_are_same_object_SHOULD_return_false()
		{
			//act
			var actual = _comparableObject.IsLessThan(_comparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsLessThan_WHERE_comparing_to_null_SHOULD_return_false()
		{
			//act
			var actual = _comparableObject.IsLessThan(null);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsLessThan_WHERE_comparing_to_comparable_object_which_is_greater_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 18 };

			//act
			var actual = _comparableObject.IsLessThan(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsLessThan_WHERE_comparing_to_comparable_object_which_is_equal_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = _comparableObject.IntegerField };

			//act
			var actual = _comparableObject.IsLessThan(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsLessThan_WHERE_comparing_to_comparable_object_which_is_less_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 1 };

			//act
			var actual = _comparableObject.IsLessThan(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion

		#region IsLessThanOrEqualTo

		[Test]
		public void IsLessThanOrEqualTo_WHERE_two_objects_are_same_object_SHOULD_return_true()
		{
			//act
			var actual = _comparableObject.IsLessThanOrEqualTo(_comparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsLessThanOrEqualTo_WHERE_comparing_to_null_SHOULD_return_false()
		{
			//act
			var actual = _comparableObject.IsLessThanOrEqualTo(null);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsLessThanOrEqualTo_WHERE_comparing_to_comparable_object_which_is_greater_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 18 };

			//act
			var actual = _comparableObject.IsLessThanOrEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsLessThanOrEqualTo_WHERE_comparing_to_comparable_object_which_is_equal_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = _comparableObject.IntegerField };

			//act
			var actual = _comparableObject.IsLessThanOrEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsLessThanOrEqualTo_WHERE_comparing_to_comparable_object_which_is_less_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 1 };

			//act
			var actual = _comparableObject.IsLessThanOrEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion

		#region IsGreaterThan

		[Test]
		public void IsGreaterThan_WHERE_two_objects_are_same_object_SHOULD_return_false()
		{
			//act
			var actual = _comparableObject.IsGreaterThan(_comparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsGreaterThan_WHERE_comparing_to_null_SHOULD_return_true()
		{
			//act
			var actual = _comparableObject.IsGreaterThan(null);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsGreaterThan_WHERE_comparing_to_comparable_object_which_is_greater_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 18 };

			//act
			var actual = _comparableObject.IsGreaterThan(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsGreaterThan_WHERE_comparing_to_comparable_object_which_is_equal_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = _comparableObject.IntegerField };

			//act
			var actual = _comparableObject.IsGreaterThan(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsGreaterThan_WHERE_comparing_to_comparable_object_which_is_less_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 1 };

			//act
			var actual = _comparableObject.IsGreaterThan(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region IsGreaterThanOrEqualTo

		[Test]
		public void IsGreaterThanOrEqualTo_WHERE_two_objects_are_same_object_SHOULD_return_true()
		{
			//act
			var actual = _comparableObject.IsGreaterThanOrEqualTo(_comparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsGreaterThanOrEqualTo_WHERE_comparing_to_null_SHOULD_return_true()
		{
			//act
			var actual = _comparableObject.IsGreaterThanOrEqualTo(null);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsGreaterThanOrEqualTo_WHERE_comparing_to_comparable_object_which_is_greater_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 18 };

			//act
			var actual = _comparableObject.IsGreaterThanOrEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsGreaterThanOrEqualTo_WHERE_comparing_to_comparable_object_which_is_equal_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = _comparableObject.IntegerField };

			//act
			var actual = _comparableObject.IsGreaterThanOrEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsGreaterThanOrEqualTo_WHERE_comparing_to_comparable_object_which_is_less_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 1 };

			//act
			var actual = _comparableObject.IsGreaterThanOrEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region IsEqualTo

		[Test]
		public void IsEqualTo_WHERE_two_objects_are_same_object_SHOULD_return_true()
		{
			//act
			var actual = _comparableObject.IsEqualTo(_comparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsEqualTo_WHERE_comparing_to_null_SHOULD_return_false()
		{
			//act
			var actual = _comparableObject.IsEqualTo(null);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsEqualTo_WHERE_comparing_to_comparable_object_which_is_greater_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 18 };

			//act
			var actual = _comparableObject.IsEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsEqualTo_WHERE_comparing_to_comparable_object_which_is_equal_SHOULD_return_true()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = _comparableObject.IntegerField };

			//act
			var actual = _comparableObject.IsEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void IsEqualTo_WHERE_comparing_to_comparable_object_which_is_less_SHOULD_return_false()
		{
			//arrange
			var otherComparableObject = new ComparableObject { IntegerField = 1 };

			//act
			var actual = _comparableObject.IsEqualTo(otherComparableObject);

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion
	}
}
