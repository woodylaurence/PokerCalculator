using PokerCalculator.Domain.Helpers;
using System;

namespace PokerCalculator.Tests.Shared.TestObjects
{
	public class FakeRandomNumberGenerator : IRandomNumberGenerator
	{
		private readonly Random _randomInstance;
		public const int DefaultRandomSeedingValue = 1337;

		public FakeRandomNumberGenerator(int randomSeedingValue = DefaultRandomSeedingValue)
		{
			_randomInstance = new Random(randomSeedingValue);
		}

		public int Next(int maxValue)
		{
			return _randomInstance.Next(maxValue);
		}
	}
}
