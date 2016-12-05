using System.Collections.Generic;
using System;
using System.Linq;
using PokerCalculator.Domain.PokerEnums;
using System.Runtime.InteropServices;
using System.Net;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Hand
	{
		internal static Hand MethodObject = new Hand();

		#region Properties and Fields

		public virtual List<Card> Cards { get; set; }

		protected internal virtual HandRank _rank { get; set; }
		public virtual HandRank Rank
		{
			get
			{
				if (_rank == null) _rank = CalculateRank();
				return _rank;
			}
			set { _rank = value; }
		}

		#endregion

		#region Static Methods

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

		#region CalculateRank

		public virtual HandRank CalculateRank()
		{
			var flushValues = GetFlushValues();
			if (flushValues.Count >= 5) return GetFlushBasedHandRank(flushValues);

			var straightValues = GetStraightValues();
			if (straightValues.Count >= 5) return HandRank.Create(PokerHand.Straight, new List<CardValue> { straightValues.First() });

			throw new NotImplementedException();
		}

		protected internal virtual HandRank GetFlushBasedHandRank(List<CardValue> flushValues)
		{
			var straightFlushValues = GetStraightValues(flushValues);
			if (straightFlushValues.Count < 5) return HandRank.Create(PokerHand.Flush, flushValues);

			var highestStraightFlushValue = straightFlushValues.First();
			return highestStraightFlushValue == CardValue.Ace
						? HandRank.Create(PokerHand.RoyalFlush)
						: HandRank.Create(PokerHand.StraightFlush, new List<CardValue> { straightFlushValues.First() });
		}

		#region GetFlushValues

		/// <summary>
		/// Gets the flush values.
		/// </summary>
		/// <returns>The flush values.</returns>
		protected internal virtual List<CardValue> GetFlushValues()
		{
			return Cards.GroupBy(x => x.Suit)
						.OrderByDescending(x => x.Count())
						.First()
						.Select(x => x.Value)
						.OrderByDescending(x => x)
				        .ToList();
		}

		#endregion

		#region GetStraightValues

		/// <summary>
		/// Gets the straight values.
		/// </summary>
		/// <returns>The straight values.</returns>
		protected internal virtual List<CardValue> GetStraightValues()
		{
			return GetStraightValues(Cards.Select(x => x.Value).ToList());
		}

		/// <summary>
		/// Gets the straight values.
		/// </summary>
		/// <returns>The straight values.</returns>
		/// <param name="cardValues">Cards.</param>
		protected internal virtual List<CardValue> GetStraightValues(List<CardValue> cardValues)
		{
			var values = Enumerable.Repeat(false, 14).ToList();
			cardValues.ForEach(x => {
				var cardValueAsInt = (int)x;
				values[14 - cardValueAsInt] = true;
			});
			values[13] = values[0];

			var listOfStraights = new List<List<CardValue>>();
			var straight = new List<CardValue>();
			if (values[0]) straight.Add((CardValue)Enum.Parse(typeof(CardValue), "14"));

			for (var i = 1; i < 14; i++)
			{
				var cardValue = i == 13 ? CardValue.Ace : (CardValue)Enum.Parse(typeof(CardValue), (14 - i).ToString());
				if (values[i]) straight.Add(cardValue);
				else
				{
					listOfStraights.Add(straight);
					straight = new List<CardValue>();
				}
			}

			if (straight.Any()) listOfStraights.Add(straight);
			return listOfStraights.OrderByDescending(x => x.Count)
								  .ThenByDescending(x => x.FirstOrDefault())
								  .First();
		}

		#endregion

		#region GetOrderedCardGroups

		protected internal virtual List<KeyValuePair<int, CardValue>> GetOrderedCardGroups()
		{
			return Cards.GroupBy(x => x.Value)
				        .Select(x => new KeyValuePair<int, CardValue>(x.Count(), x.Key))
						.OrderByDescending(x => x.Key)
						.ThenByDescending(x => x.Value)
						.ToList();
		}

		#endregion

		#endregion

		#endregion
	}
}
