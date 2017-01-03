using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class LocalTestBase : AbstractUnitTestBase
	{
		protected internal IEqualityComparer<Card> CardComparer = new CardComparer();
	}
}
