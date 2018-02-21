namespace PokerCalculator.Domain.Helpers
{
	public interface IRandomNumberGenerator
	{
		int Next(int maxValue);
	}
}
