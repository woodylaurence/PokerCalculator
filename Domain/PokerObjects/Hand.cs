using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.HandRankCalculator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Hand
	{
		#region Properties and Fields

		private readonly IHandRankCalculator _handRankCalculator;
		private readonly IEqualityComparer<Card> _cardComparer;

		public virtual List<Card> Cards { get; }

		protected internal virtual HandRank _rank { get; set; }
		public virtual HandRank Rank
		{
			get => _rank ?? (_rank = _handRankCalculator.CalculateHandRank(this));
			set => _rank = value;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cards"></param>
		public Hand(List<Card> cards) : this(cards, ServiceLocator.Current.GetInstance<IEqualityComparer<Card>>(), ServiceLocator.Current.GetInstance<IHandRankCalculator>()) { }
		public Hand(List<Card> cards, IEqualityComparer<Card> cardComparer, IHandRankCalculator handRankCalculator)
		{
			_cardComparer = cardComparer;
			_handRankCalculator = handRankCalculator;

			if (cards.Count > 7) throw new ArgumentException("A Hand cannot contain more than seven cards", nameof(cards));
			if (cards.Distinct(_cardComparer).Count() != cards.Count) throw new ArgumentException("A Hand cannot contain duplicate cards", nameof(cards));
			Cards = cards.ToList();
		}

		#endregion

		#region Instance Methods

		#region AddCard

		/// <summary>
		/// Adds the card.
		/// </summary>
		/// <param name="cardToAdd">Card to add.</param>
		public virtual void AddCard(Card cardToAdd)
		{
			AddCards(new List<Card> { cardToAdd });
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
		public static Hand operator +(Hand hand1, Hand hand2) => hand1.Operator_plus(hand2);
		protected internal virtual Hand Operator_plus(Hand hand2)
		{
			var returnHand = Clone();
			returnHand.AddCards(hand2.Cards.ToList());
			return returnHand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected internal virtual Hand Clone()
		{
			return new Hand(Cards, _cardComparer, _handRankCalculator);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cardsToAdd"></param>
		protected internal virtual void AddCards(List<Card> cardsToAdd)
		{
			VerifyCardsCanBeAdded(cardsToAdd);
			Cards.AddRange(cardsToAdd);
			Rank = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cardsToAdd"></param>
		protected internal virtual void VerifyCardsCanBeAdded(List<Card> cardsToAdd)
		{
			if (Cards.Count + cardsToAdd.Count > 7) throw new ArgumentException("A Hand cannot have more than seven cards");
			if (cardsToAdd.Distinct(_cardComparer).Count() != cardsToAdd.Count) throw new ArgumentException("A Hand cannot contain duplicate cards");
			if (cardsToAdd.Any(x => Cards.Contains(x, _cardComparer))) throw new ArgumentException("A Hand cannot contain duplicate cards");
		}

		#endregion

		#endregion
	}
}
