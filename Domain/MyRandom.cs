using System;

namespace PokerCalculator.Domain
{
	public class MyRandom
	{
		internal static MyRandom MethodObject = new MyRandom();

		internal static Random _randomInstance { get; }
		protected internal virtual Random RandomInstance => _randomInstance;

		static MyRandom()
		{
			_randomInstance = new Random();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int GenerateRandomNumber(int minValue, int maxValue) { return MethodObject.GenerateRandomNumberSlave(minValue, maxValue); }
		protected internal virtual int GenerateRandomNumberSlave(int minValue, int maxValue)
		{
			return RandomInstance.Next(minValue, maxValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int GenerateRandomNumber(int maxValue) { return MethodObject.GenerateRandomNumberSlave(maxValue); }
		protected internal virtual int GenerateRandomNumberSlave(int maxValue)
		{
			return RandomInstance.Next(maxValue);
		}
	}
}
