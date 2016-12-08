using System;
using System.Collections.Generic;
using System.Linq;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Domain.HandRankCalculator
{
	public class HandRankCalculator : IHandRankCalculator
	{
		public virtual HandRank CalculateHandRank(Hand hand)
		{
			var flushValues = GetFlushValues(hand.Cards);
			if (flushValues.Count >= 5) return GetFlushBasedHandRank(flushValues);

			var straightValues = GetStraightValues(hand.Cards);
			if (straightValues.Count >= 5) return HandRank.Create(PokerHand.Straight, new List<CardValue> { straightValues.First() });

			return GetMultiCardOrHighCardHandRank(hand.Cards);
		}

		/// <summary>
		/// Gets the flush based hand rank.
		/// </summary>
		/// <returns>The flush based hand rank.</returns>
		/// <param name="flushValues">Flush values.</param>
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
		protected internal virtual List<CardValue> GetFlushValues(List<Card> cards)
		{
			return cards.GroupBy(x => x.Suit)
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
		protected internal virtual List<CardValue> GetStraightValues(List<Card> cards)
		{
			return GetStraightValues(cards.Select(x => x.Value).ToList());
		}

		/// <summary>
		/// Gets the straight values.
		/// </summary>
		/// <returns>The straight values.</returns>
		/// <param name="cardValues">Cards.</param>
		protected internal virtual List<CardValue> GetStraightValues(List<CardValue> cardValues)
		{
			var values = Enumerable.Repeat(false, 14).ToList();
			cardValues.ForEach(x =>
			{
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

		#region GetMultiCardOrHighCardRank

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected internal virtual HandRank GetMultiCardOrHighCardHandRank(List<Card> cards)
		{
			var cardGroups = GetOrderedCardGroups(cards);
			switch (cardGroups.First().Key)
			{
			case 4:
				return GetFourOfAKindHandRank(cardGroups);
			case 3:
				return GetFullHouseOrThreeOfAKindHandRank(cardGroups);
			case 2:
				return GetPairBasedHandRank(cardGroups);
			case 1:
				return GetHighCardHandRank(cardGroups);
			default:
				throw new Exception("Unexpected Card group");
			}
		}

		/// <summary>
		/// Gets the four of AK ind hand rank.
		/// </summary>
		/// <returns>The four of AK ind hand rank.</returns>
		/// <param name="cardGroups">Card groups.</param>
		protected internal virtual HandRank GetFourOfAKindHandRank(List<KeyValuePair<int, CardValue>> cardGroups)
		{
			var fourOfAKindKickerValues = new List<CardValue> { cardGroups[0].Value };
			fourOfAKindKickerValues.AddRange(cardGroups.Skip(1).Select(x => x.Value).OrderByDescending(x => x).Take(1));
			return HandRank.Create(PokerHand.FourOfAKind, fourOfAKindKickerValues);
		}

		/// <summary>
		/// Gets the full house or three of AK ind hand rank.
		/// </summary>
		/// <returns>The full house or three of AK ind hand rank.</returns>
		/// <param name="cardGroups">Card groups.</param>
		protected internal virtual HandRank GetFullHouseOrThreeOfAKindHandRank(List<KeyValuePair<int, CardValue>> cardGroups)
		{
			if (cardGroups.Count > 1 && cardGroups[1].Key > 1) return HandRank.Create(PokerHand.FullHouse, new List<CardValue> { cardGroups[0].Value, cardGroups[1].Value });

			var threeOfAKindKickerValues = new List<CardValue> { cardGroups[0].Value };
			threeOfAKindKickerValues.AddRange(cardGroups.Skip(1).Select(x => x.Value).OrderByDescending(x => x).Take(2));
			return HandRank.Create(PokerHand.ThreeOfAKind, threeOfAKindKickerValues);
		}

		/// <summary>
		/// Gets the pair based hand rank.
		/// </summary>
		/// <returns>The pair based hand rank.</returns>
		/// <param name="cardGroups">Card groups.</param>
		protected internal virtual HandRank GetPairBasedHandRank(List<KeyValuePair<int, CardValue>> cardGroups)
		{
			if (cardGroups.Count > 1 && cardGroups[1].Key > 1)
			{
				var twoPairKickerValues = new List<CardValue> { cardGroups[0].Value, cardGroups[1].Value };
				twoPairKickerValues.AddRange(cardGroups.Skip(2).Select(x => x.Value).OrderByDescending(x => x).Take(1));
				return HandRank.Create(PokerHand.TwoPair, twoPairKickerValues);
			}

			var pairKickerValues = new List<CardValue> { cardGroups[0].Value };
			pairKickerValues.AddRange(cardGroups.Skip(1).Select(x => x.Value).OrderByDescending(x => x).Take(3));
			return HandRank.Create(PokerHand.Pair, pairKickerValues);
		}

		/// <summary>
		/// Gets the high card hand rank.
		/// </summary>
		/// <returns>The high card hand rank.</returns>
		/// <param name="cardGroups">Card groups.</param>
		protected internal virtual HandRank GetHighCardHandRank(List<KeyValuePair<int, CardValue>> cardGroups)
		{
			return HandRank.Create(PokerHand.HighCard, cardGroups.Select(x => x.Value).OrderByDescending(x => x).Take(5).ToList());
		}

		#region GetOrderedCardGroups

		/// <summary>
		/// Gets the ordered card groups.
		/// </summary>
		/// <returns>The ordered card groups.</returns>
		protected internal virtual List<KeyValuePair<int, CardValue>> GetOrderedCardGroups(List<Card> cards)
		{
			return cards.GroupBy(x => x.Value)
						.Select(x => new KeyValuePair<int, CardValue>(x.Count(), x.Key))
						.OrderByDescending(x => x.Key)
						.ThenByDescending(x => x.Value)
						.ToList();
		}

		#endregion

		#endregion
	}
}
