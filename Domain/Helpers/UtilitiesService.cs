using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public string GetTicksAsStringWithUnit(double ticks)
		{
			var microseconds = 1000000 * ticks / Stopwatch.Frequency;
			switch (microseconds)
			{
				case double ms when ms >= 10000000: return $"{microseconds / 1000000.0:N1}s";
				case double ms when ms >= 1000: return $"{microseconds / 1000.0:N1}ms";
				case double ms when ms >= 1: return $"{microseconds:N1}μs";
				default: return $"{microseconds * 1000:N1}ns"; ;
			}
		}

		#endregion
	}
}
