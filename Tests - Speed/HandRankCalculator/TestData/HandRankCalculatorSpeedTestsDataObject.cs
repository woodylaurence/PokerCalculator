using System.Collections.Generic;
using System.Linq;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Speed.HandRankCalculator.TestData
{
	public class HandRankCalculatorSpeedTestsDataObject
	{
		public int VersionOrdinal { get; set; }
		public string VersionName { get; set; }
		public Dictionary<PokerHand, HandRankCalculatorSpeedTestVersionResult> PokerHandResults { get; set; }

		public HandRankCalculatorSpeedTestsDataObject()
		{
			PokerHandResults = Utilities.GetEnumValues<PokerHand>().ToDictionary(x => x, x => new HandRankCalculatorSpeedTestVersionResult());
		}
	}
}