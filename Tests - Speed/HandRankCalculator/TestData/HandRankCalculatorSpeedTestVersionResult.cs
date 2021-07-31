namespace PokerCalculator.Tests.Speed.HandRankCalculator.TestData
{
	public class HandRankCalculatorSpeedTestVersionResult
	{
		public int Frequency { get; set; }
		public long TotalCalculationTicks { get; set; }
		public double AverageCalculationTicks => TotalCalculationTicks / (double)Frequency;

		public void AddCalculationData(long calculationTicks)
		{
			Frequency++;
			TotalCalculationTicks += calculationTicks;
		}
	}
}