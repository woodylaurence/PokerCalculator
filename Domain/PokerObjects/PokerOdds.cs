using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerObjects
{
	public class PokerOdds
	{
		private IUtilitiesService _utilitiesService { get; }

		public double WinPercentage { get; set; }
		public double DrawPercentage { get; set; }
		public double LosePercentage { get; set; }

		public virtual Dictionary<PokerHand, double> PokerHandPercentages { get; }

		public PokerOdds(IUtilitiesService utilitiesService)
		{
			_utilitiesService = utilitiesService;
			PokerHandPercentages = _utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0.0);
		}
	}
}
