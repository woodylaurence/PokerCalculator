using System.Collections.Generic;
using System;
using System.Linq;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Hand
	{
		internal static Hand MethodObject = new Hand();

		public virtual List<Card> Cards { get; set; }

		/// <summary>
		/// Create the specified cards.
		/// </summary>
		/// <param name="cards">Cards.</param>
		public static Hand Create (List<Card> cards = null) { return MethodObject.CreateSlave(cards); }
		protected internal virtual Hand CreateSlave(List<Card> cards)
		{
			cards = cards ?? new List<Card>();
			if (cards.Count > 7) throw new ArgumentException("A Hand cannot contain more than seven cards", nameof(cards));
			if (cards.Distinct(new CardComparer()).Count() != cards.Count) throw new ArgumentException("A Hand cannot contain duplicate cards", nameof(cards));
			return new Hand
			{
				Cards = cards.ToList()
			};
		}

		public virtual void AddCard(Card cardToAdd)
		{
			throw new NotImplementedException();
		}
	}
}
