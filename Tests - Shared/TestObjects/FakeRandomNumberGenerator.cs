using PokerCalculator.Domain.Helpers;
using System;
using System.Configuration;

namespace PokerCalculator.Tests.Shared.TestObjects
{
	public class FakeRandomNumberGenerator : IRandomNumberGenerator
	{
		private readonly Random _randomInstance;
		private static int RandomSeedingValue => int.Parse(ConfigurationManager.AppSettings["PokerCalculator.Helpers.FakeRandomNumberGenerator.RandomSeedingValue"]);

		public FakeRandomNumberGenerator()
		{
			_randomInstance = new Random(RandomSeedingValue);
		}

		public int Next(int maxValue)
		{
			return _randomInstance.Next(maxValue);
		}
	}
}
