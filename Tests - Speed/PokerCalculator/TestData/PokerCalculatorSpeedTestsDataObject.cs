using System.Collections.Generic;

namespace PokerCalculator.Tests.Speed.PokerCalculator.TestData
{
	public class PokerCalculatorSpeedTestsDataObject
	{
		public int VersionOrdinal { get; set; }
		public string VersionName { get; set; }
		public List<PokerCalculatorSpeedTestVersionResult> Results { get; set; }
	}
}