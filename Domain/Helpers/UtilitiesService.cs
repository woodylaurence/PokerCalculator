using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.Helpers
{
	public class UtilitiesService : IUtilitiesService
	{
		#region GetEnumValues

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public List<T> GetEnumValues<T>() where T : struct
		{
			return Enum.GetValues(typeof(T)).Cast<T>().ToList();
		}

		#endregion

		#region GetTicksAsStringWithUnit

		public string GetTicksAsStringWithUnit(double ticks)
		{
			if (ticks >= 10 * TimeSpan.TicksPerSecond) return $"{ticks / TimeSpan.TicksPerSecond:N1}s";
			if (ticks >= TimeSpan.TicksPerMillisecond) return $"{ticks / TimeSpan.TicksPerMillisecond:N1}ms";
			if (ticks * 1000 >= TimeSpan.TicksPerMillisecond) return $"{ticks * 1000 / TimeSpan.TicksPerMillisecond:N1}μs";
			return $"{ticks * 1000000 / TimeSpan.TicksPerMillisecond:N1}ns";
		}

		#endregion
	}
}
