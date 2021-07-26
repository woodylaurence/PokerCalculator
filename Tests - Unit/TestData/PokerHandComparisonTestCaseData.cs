using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Tests.Unit.TestData
{
	public class PokerHandComparisonTestCaseData : TestCaseData
	{
		//todo maybe extract this to PokerHandTestCaseData?
		public static readonly List<PokerHand> AllPokerHands = Utilities.GetEnumValues<PokerHand>();
		public static readonly IEnumerable<TestCaseData> PokerHandWithHigherPokerHands = AllPokerHands.SelectMany(x => AllPokerHands.Where(y => y > x), (lhs, rhs) => new TestCaseData(lhs, rhs).SetName($"{lhs}-{rhs}"));
		public static readonly IEnumerable<TestCaseData> PokerHandWithLowerPokerHands = AllPokerHands.SelectMany(x => AllPokerHands.Where(y => y < x), (lhs, rhs) => new TestCaseData(lhs, rhs).SetName($"{lhs}-{rhs}"));
	}
}