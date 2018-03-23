using System;
using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerCalculator
{
	public class PokerOdds
	{
		#region Properties and Fields

		internal static PokerOdds MethodObject = new PokerOdds(new UtilitiesService());

		protected internal virtual int NumWins { get; set; }
		protected internal virtual int NumDraws { get; set; }
		protected internal virtual int NumLosses { get; set; }
		protected internal virtual int TotalNumHands => NumWins + NumDraws + NumLosses;

		protected internal virtual double WinPercentage => TotalNumHands == 0 ? 0 : (double)NumWins / TotalNumHands;
		protected internal virtual double DrawPercentage => TotalNumHands == 0 ? 0 : (double)NumDraws / TotalNumHands;
		protected internal virtual double LossPercentage => TotalNumHands == 0 ? 0 : (double)NumLosses / TotalNumHands;

		protected internal virtual Dictionary<PokerHand, int> PokerHandFrequencies { get; internal set; }
		protected internal virtual Dictionary<PokerHand, double> PokerHandPercentages => PokerHandFrequencies.ToDictionary(x => x.Key, x => TotalNumHands == 0 ? 0 : (double)x.Value / TotalNumHands);

		#region Aggregated Values

		public virtual PercentageWithError WinPercentageWithError { get; private set; }
		public virtual PercentageWithError DrawPercentageWithError { get; private set; }
		public virtual PercentageWithError LossPercentageWithError { get; private set; }
		public virtual Dictionary<PokerHand, PercentageWithError> PokerHandPercentagesWithErrors { get; internal set; }

		#endregion

		#endregion

		#region Constructor

		public PokerOdds(IUtilitiesService utilitiesService)
		{
			PokerHandFrequencies = utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pokerOdds"></param>
		/// <returns></returns>
		public static PokerOdds AggregatePokerOdds(List<PokerOdds> pokerOdds) => MethodObject.AggregatePokerOddsSlave(pokerOdds);
		protected internal virtual PokerOdds AggregatePokerOddsSlave(List<PokerOdds> pokerOdds)
		{
			if (pokerOdds.Count < 2) throw new ArgumentException("Cannot aggregate less than two PokerOdds.");

			var utilitiesService = ServiceLocator.Current.GetInstance<IUtilitiesService>();
			return new PokerOdds(utilitiesService)
			{
				WinPercentageWithError = new PercentageWithError(pokerOdds.Select(x => x.WinPercentage).ToList()),
				DrawPercentageWithError = new PercentageWithError(pokerOdds.Select(x => x.DrawPercentage).ToList()),
				LossPercentageWithError = new PercentageWithError(pokerOdds.Select(x => x.LossPercentage).ToList()),
				PokerHandPercentagesWithErrors = pokerOdds.SelectMany(x => x.PokerHandPercentages)
														  .GroupBy(x => x.Key)
														  .ToDictionary(x => x.Key, x => new PercentageWithError(x.Select(y => y.Value).ToList()))
			};
		}

		#endregion
	}
}
