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
	}
}
