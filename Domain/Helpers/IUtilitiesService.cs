using System.Collections.Generic;

namespace PokerCalculator.Domain.Helpers
{
	public interface IUtilitiesService
	{
		List<T> GetEnumValues<T>() where T : struct;
		T GetEnumValueFromDescription<T>(string description) where T : struct;
		string GetTicksAsStringWithUnit(double ticks);
	}
}
