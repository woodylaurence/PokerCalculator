using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain
{
	public class Utilities
	{
		internal static Utilities MethodObject = new Utilities();

		#region GetEnumValues

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<T> GetEnumValues<T>() where T : struct => MethodObject.GetEnumValuesSlave<T>();
		protected internal virtual List<T> GetEnumValuesSlave<T>() where T : struct
		{
			return Enum.GetValues(typeof(T)).Cast<T>().ToList();
		}

		#endregion
	}
}
