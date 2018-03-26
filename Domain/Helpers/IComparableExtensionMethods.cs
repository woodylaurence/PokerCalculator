using System;

namespace PokerCalculator.Domain.Helpers
{
	public static class IComparableExtensionMethods
	{
		internal static IComparableExtensionMethodsConcreteObject MethodObject = new IComparableExtensionMethodsConcreteObject();

		public static bool IsLessThan<T>(this IComparable<T> lhs, T rhs) => MethodObject.IsLessThanSlave(lhs, rhs);
		public static bool IsLessThanOrEqualTo<T>(this IComparable<T> lhs, T rhs) => MethodObject.IsLessThanOrEqualToSlave(lhs, rhs);
		public static bool IsGreaterThan<T>(this IComparable<T> lhs, T rhs) => MethodObject.IsGreaterThanSlave(lhs, rhs);
		public static bool IsGreaterThanOrEqualTo<T>(this IComparable<T> lhs, T rhs) => MethodObject.IsGreaterThanOrEqualToSlave(lhs, rhs);
		public static bool IsEqualTo<T>(this IComparable<T> lhs, T rhs) => MethodObject.IsEqualToSlave(lhs, rhs);
	}

	public class IComparableExtensionMethodsConcreteObject
	{
		public virtual bool IsLessThanSlave<T>(IComparable<T> lhs, T rhs) => lhs.CompareTo(rhs) < 0;
		public virtual bool IsLessThanOrEqualToSlave<T>(IComparable<T> lhs, T rhs) => lhs.CompareTo(rhs) <= 0;
		public virtual bool IsGreaterThanSlave<T>(IComparable<T> lhs, T rhs) => lhs.CompareTo(rhs) > 0;
		public virtual bool IsGreaterThanOrEqualToSlave<T>(IComparable<T> lhs, T rhs) => lhs.CompareTo(rhs) >= 0;
		public virtual bool IsEqualToSlave<T>(IComparable<T> lhs, T rhs) => lhs.CompareTo(rhs) == 0;
	}
}
