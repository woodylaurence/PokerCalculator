using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Card
	{
		internal static Card MethodObject = new Card();

		public virtual CardValue Value { get; set; }
		public virtual CardSuit Suit { get; set; }

		/// <summary>
		/// Create the specified value and suit.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="suit">Suit.</param>
		public static Card Create(CardValue value, CardSuit suit) { return MethodObject.CreateSlave(value, suit); }
		protected internal virtual Card CreateSlave(CardValue value, CardSuit suit)
		{
			return new Card
			{
				Value = value,
				Suit = suit
			};
		}
	}
}
