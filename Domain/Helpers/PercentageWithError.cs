using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.Helpers
{
	public class PercentageWithError
	{
		#region Properties and Fields

		public double Percentage { get; set; }
		public double Error { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		///
		/// </summary>
		/// <param name="percentage"></param>
		/// <param name="error"></param>
		internal PercentageWithError(double percentage, double error)
		{
			Percentage = percentage;
			Error = error;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="percentages"></param>
		public PercentageWithError(List<double> percentages)
		{
			if (percentages.Count < 2) throw new ArgumentException("Cannot calculate error with less than two percentages");

			var mean = percentages.Average();
			var variance = percentages.Average(x => (x - mean) * (x - mean));
			Percentage = mean;
			Error = Math.Sqrt(variance);
		}

		#endregion

		#region Instance Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Error == 0
						? $"{Percentage * 100}%"
						: $"{Percentage:P1} ± {Error:P1}";
		}

		#endregion
	}
}