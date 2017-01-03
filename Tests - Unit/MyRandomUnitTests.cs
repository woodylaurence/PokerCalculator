using System;
using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;

namespace PokerCalculator.Tests.Unit
{
	[TestFixture]
	public class MyRandomUnitTests : AbstractUnitTestBase
	{
		private MyRandom _instance;

		[SetUp]
		public new void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<MyRandom>();

			MyRandom.MethodObject = MockRepository.GenerateStrictMock<MyRandom>();
		}

		[TearDown]
		public void TearDown()
		{
			MyRandom.MethodObject = new MyRandom();
		}

		#region GenerateRandomNumber with Min/Max Values

		[Test]
		public void GenerateRandomNumber_with_min_max_values_calls_slave()
		{
			//arrange
			const int minValue = 61;
			const int maxValue = 991;
			const int expected = 77;
			MyRandom.MethodObject.Expect(x => x.GenerateRandomNumberSlave(minValue, maxValue)).Return(expected);

			//act
			var actual = MyRandom.GenerateRandomNumber(minValue, maxValue);

			//assert
			MyRandom.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GenerateRandomNumberSlave_with_min_max_values()
		{
			//arrange
			var randomInstance = MockRepository.GenerateStrictMock<Random>();
			_instance.Stub(x => x.RandomInstance).Return(randomInstance);

			const int minValue = 61;
			const int maxValue = 497;
			const int expected = 477;
			randomInstance.Stub(x => x.Next(minValue, maxValue)).Return(expected);

			//act
			var actual = _instance.GenerateRandomNumberSlave(minValue, maxValue);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region GenerateRandomNumber with Min/Max Value

		[Test]
		public void GenerateRandomNumber_with_max_value_calls_slave()
		{
			//arrange
			const int maxValue = 991;
			const int expected = 77;
			MyRandom.MethodObject.Expect(x => x.GenerateRandomNumberSlave(maxValue)).Return(expected);

			//act
			var actual = MyRandom.GenerateRandomNumber(maxValue);

			//assert
			MyRandom.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GenerateRandomNumberSlave_with_max_value()
		{
			//arrange
			var randomInstance = MockRepository.GenerateStrictMock<Random>();

			var generateStrictMock = MockRepository.GenerateStrictMock<MyRandom>();
			generateStrictMock.Stub(x => x.RandomInstance).Return(randomInstance);

			_instance.Stub(x => x.RandomInstance).Return(randomInstance);

			const int maxValue = 497;
			const int expected = 477;
			randomInstance.Stub(x => x.Next(maxValue)).Return(expected);

			//act
			var actual = _instance.GenerateRandomNumberSlave(maxValue);

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}
		
		#endregion
	}
}
