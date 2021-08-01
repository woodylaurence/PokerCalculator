using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Hand
	{
		#region Properties and Fields

		//todo is a Hand actually a collection of cards, rather than having a collection of cards?
		public virtual List<Card> Cards { get; private init; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cards"></param>
		public Hand(List<Card> cards)
		{
			if (cards.Count > 7) throw new ArgumentException("A Hand cannot contain more than seven cards", nameof(cards));
			if (cards.Distinct().Count() != cards.Count) throw new ArgumentException("A Hand cannot contain duplicate cards", nameof(cards));

			Cards = cards.ToList();
		}
		private Hand() { }

		#endregion

		#region Instance Methods

		#region AddCard

		/// <summary>
		/// Adds the card.
		/// </summary>
		/// <param name="cardToAdd">Card to add.</param>
		public virtual void AddCard(Card cardToAdd) => AddCards(new List<Card> { cardToAdd });

		#endregion

		#region AddCards

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cardsToAdd"></param>
		public virtual void AddCards(List<Card> cardsToAdd)
		{
			VerifyCardsCanBeAdded(cardsToAdd);
			Cards.AddRange(cardsToAdd);
		}

		#endregion

		#endregion

		#region Operator Overloads

		#region +

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hand1"></param>
		/// <param name="hand2"></param>
		/// <returns></returns>
		public static Hand operator +(Hand hand1, Hand hand2)
		{
			var returnHand = hand1.Clone();
			returnHand.AddCards(hand2.Cards.ToList());
			return returnHand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Hand Clone() => new() { Cards = Cards.ToList() };

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cardsToAdd"></param>
		private void VerifyCardsCanBeAdded(List<Card> cardsToAdd)
		{
			if (Cards.Count + cardsToAdd.Count > 7) throw new ArgumentException("A Hand cannot have more than seven cards");
			if (cardsToAdd.Distinct().Count() != cardsToAdd.Count) throw new ArgumentException("A Hand cannot contain duplicate cards");
			if (cardsToAdd.Any(x => Cards.Contains(x))) throw new ArgumentException("A Hand cannot contain duplicate cards");
		}

		#endregion

		#endregion
	}
}
