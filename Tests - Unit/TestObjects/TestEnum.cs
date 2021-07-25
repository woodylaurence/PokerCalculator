using System.ComponentModel;

namespace PokerCalculator.Tests.Unit.TestObjects
{
	public enum TestEnum
	{
		EnumValue1 = 1,
		SecondValue = 2,
		[Description("The Final Value")]
		ValueThree
	}
}
