using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System;
using PokerCalculator.Domain.Extensions;

namespace PokerCalculator.Tests.Unit.Helpers
{
	[TestFixture]
	public class IComparableExtensionMethodsUnitTests : AbstractUnitTestBase
	{
		private IComparable<object> _lhsComparableObject;
		private IComparable<object> _rhsComparableObject;
		private IComparableExtensionMethodsConcreteObject _concreteObject;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_concreteObject = MockRepository.GeneratePartialMock<IComparableExtensionMethodsConcreteObject>();
			_lhsComparableObject = MockRepository.GenerateStrictMock<IComparable<object>>();
			_rhsComparableObject = MockRepository.GenerateStrictMock<IComparable<object>>();

			IComparableExtensionMethods.MethodObject = MockRepository.GenerateStrictMock<IComparableExtensionMethodsConcreteObject>();
		}

		[TearDown]
		protected void TearDown()
		{
			IComparableExtensionMethods.MethodObject = new IComparableExtensionMethodsConcreteObject();
		}

		#region IsLessThan

		[TestCase(true)]
		[TestCase(false)]
		public void IsLessThan_SHOULD_call_slave(bool expected)
		{
			//arrange
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsLessThanSlave(_lhsComparableObject, _rhsComparableObject)).Return(expected);

			//act
			var actual = IComparableExtensionMethods.IsLessThan(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(1, false)]
		[TestCase(0, false)]
		[TestCase(-1, true)]
		public void IsLessThanSlave(int comparisonResult, bool expected)
		{
			//arrange
			_lhsComparableObject.Stub(x => x.CompareTo(_rhsComparableObject)).Return(comparisonResult);

			//act
			var actual = _concreteObject.IsLessThanSlave(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region IsLessThanOrEqualTo

		[TestCase(true)]
		[TestCase(false)]
		public void IsLessThanOrEqualTo_SHOULD_call_slave(bool expected)
		{
			//arrange
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsLessThanOrEqualToSlave(_lhsComparableObject, _rhsComparableObject)).Return(expected);

			//act
			var actual = IComparableExtensionMethods.IsLessThanOrEqualTo(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(1, false)]
		[TestCase(0, true)]
		[TestCase(-1, true)]
		public void IsLessThanOrEqualToSlave(int comparisonResult, bool expected)
		{
			//arrange
			_lhsComparableObject.Stub(x => x.CompareTo(_rhsComparableObject)).Return(comparisonResult);

			//act
			var actual = _concreteObject.IsLessThanOrEqualToSlave(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region IsGreaterThan

		[TestCase(true)]
		[TestCase(false)]
		public void IsGreaterThan_SHOULD_call_slave(bool expected)
		{
			//arrange
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsGreaterThanSlave(_lhsComparableObject, _rhsComparableObject)).Return(expected);

			//act
			var actual = IComparableExtensionMethods.IsGreaterThan(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(1, true)]
		[TestCase(0, false)]
		[TestCase(-1, false)]
		public void IsGreaterThanSlave(int comparisonResult, bool expected)
		{
			//arrange
			_lhsComparableObject.Stub(x => x.CompareTo(_rhsComparableObject)).Return(comparisonResult);

			//act
			var actual = _concreteObject.IsGreaterThanSlave(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region IsGreaterThanOrEqualTo

		[TestCase(true)]
		[TestCase(false)]
		public void IsGreaterThanOrEqualTo_SHOULD_call_slave(bool expected)
		{
			//arrange
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsGreaterThanOrEqualToSlave(_lhsComparableObject, _rhsComparableObject)).Return(expected);

			//act
			var actual = IComparableExtensionMethods.IsGreaterThanOrEqualTo(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(1, true)]
		[TestCase(0, true)]
		[TestCase(-1, false)]
		public void IsGreaterThanOrEqualToSlave(int comparisonResult, bool expected)
		{
			//arrange
			_lhsComparableObject.Stub(x => x.CompareTo(_rhsComparableObject)).Return(comparisonResult);

			//act
			var actual = _concreteObject.IsGreaterThanOrEqualToSlave(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region IsEqualTo

		[TestCase(true)]
		[TestCase(false)]
		public void IsEqualTo_SHOULD_call_slave(bool expected)
		{
			//arrange
			IComparableExtensionMethods.MethodObject.Stub(x => x.IsEqualToSlave(_lhsComparableObject, _rhsComparableObject)).Return(expected);

			//act
			var actual = IComparableExtensionMethods.IsEqualTo(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(1, false)]
		[TestCase(0, true)]
		[TestCase(-1, false)]
		public void IsEqualToSlave(int comparisonResult, bool expected)
		{
			//arrange
			_lhsComparableObject.Stub(x => x.CompareTo(_rhsComparableObject)).Return(comparisonResult);

			//act
			var actual = _concreteObject.IsEqualToSlave(_lhsComparableObject, _rhsComparableObject);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
