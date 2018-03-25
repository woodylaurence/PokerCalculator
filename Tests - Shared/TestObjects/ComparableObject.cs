using System;

namespace PokerCalculator.Tests.Shared.TestObjects
{
	public class ComparableObject : IComparable<ComparableObject>
	{
		public int IntegerField { get; set; }

		public int CompareTo(ComparableObject other)
		{
			if (ReferenceEquals(null, other)) return 1;
			if (ReferenceEquals(this, other)) return 0;
			return IntegerField.CompareTo(other.IntegerField);
		}
	}
}
