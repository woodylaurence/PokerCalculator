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

		public virtual HandRank CalculateRank()
		{
			throw new NotImplementedException();
		}

		protected internal virtual bool IsFlush()
		{
			throw new NotImplementedException();
		}

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

		/// <summary>
		/// Gets the straight values.
		/// </summary>
		/// <returns>The straight values.</returns>
		protected internal virtual List<CardValue> GetStraightValues()
		{
			var values = Enumerable.Repeat(false, 14).ToList();
			Cards.ForEach(x =>
			{
				var cardValueAsInt = (int)x.Value;
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
	}
}
