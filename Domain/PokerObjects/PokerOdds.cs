using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerObjects
{
	public class PokerOdds
	{
		public double WinPercentage { get; set; }
		public double DrawPercentage { get; set; }
		public double LosePercentage { get; set; }

		public Dictionary<PokerEnums.PokerHand, double> PokerHandPercentages { get; } = Utilities.GetEnumValues<PokerEnums.PokerHand>().ToDictionary(x => x, x => 0d);
	}
}
