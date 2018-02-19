using System.Collections.Generic;

namespace PokerCalculator.Domain.Helpers
{
	public interface IUtilitiesService
	{
		List<T> GetEnumValues<T>() where T : struct;
	}
}
