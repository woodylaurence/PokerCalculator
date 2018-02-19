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
			if (Cards.Count == 7) throw new Exception("A Hand cannot have more than seven cards");
			if (Cards.Contains(cardToAdd, new CardComparer())) throw new Exception("A Hand cannot contain duplicate cards");
			Cards.Add(cardToAdd);
			Rank = null;
		}

		#endregion

		#endregion
	}
}
