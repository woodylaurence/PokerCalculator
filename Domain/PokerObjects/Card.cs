using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Card
	{
		public virtual CardValue Value { get; }
		public virtual CardSuit Suit { get; }

		public Card(CardValue value, CardSuit suit)
		{
			Value = value;
			Suit = suit;
		}
	}
}
