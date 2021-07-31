namespace PokerCalculator.Tests.Speed.PokerCalculator
{
	public static class StringFormattingExtensions
	{
		public static string CenterString(this string stringToCenter, int totalLength)
		{
			return stringToCenter.PadLeft((totalLength - stringToCenter.Length) / 2 + stringToCenter.Length).PadRight(totalLength);
		}
	}
}