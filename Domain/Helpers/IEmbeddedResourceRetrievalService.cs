using System.Collections.Generic;

namespace PokerCalculator.Domain.Helpers
{
	public interface IEmbeddedResourceRetrievalService
	{
		List<int> RetrieveIntegerListFromEmbeddedResource(string assemblyName, string resourceName);
	}
}