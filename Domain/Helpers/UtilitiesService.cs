using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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

		#region GetEnumValueFromDescription

		public T GetEnumValueFromDescription<T>(string description) where T : struct
		{
			var enumType = typeof(T);
			if (enumType.IsEnum == false) throw new InvalidOperationException("Type to return must be an enum.");

			var enumValueMatchingDescription = enumType.GetFields()
													   .FirstOrDefault(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description == description ||
																			x.Name == description);

			if (enumValueMatchingDescription == null) throw new ArgumentException("Cannot find enum value by supplied string");
			return (T)enumValueMatchingDescription.GetValue(null);
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
