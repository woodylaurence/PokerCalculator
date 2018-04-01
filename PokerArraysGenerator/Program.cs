using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.Extensions;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokerArraysGenerator
{
	public class Program
	{
		static void Main(string[] args)
		{
			WindsorContainerUtilities.SetupAndConfigureWindsorContainer();

			var orderedPossibleHands = GetOrderedListOfPossibleHandsWithAssociatedRank();
			var orderedDistinctPossibleHands = GetOrderedListOfDistinctPossibleHandsWithAssociatedRank(orderedPossibleHands);
			var flushesValuesList = GetFlushesValuesList(orderedDistinctPossibleHands);

			const string outputFilePath = "C:\\test\\flushesArray";
			File.WriteAllText(outputFilePath, string.Join(",", flushesValuesList));
			Console.WriteLine($"Written FlushesArray out to {outputFilePath}.");


			Console.ReadLine();
		}

		private static List<KeyValuePair<Hand, PokerHandBasedHandRank>> GetOrderedListOfPossibleHandsWithAssociatedRank()
		{
			var pokerHandBasedHandRankCalculator = ServiceLocator.Current.GetInstance<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>();

			return new Deck().GenerateAllPossible5CardHands()
							 .Select(x => new KeyValuePair<Hand, PokerHandBasedHandRank>(x, pokerHandBasedHandRankCalculator.CalculateHandRank(x)))
							 .OrderByDescending(x => x.Value)
							 .ToList();
		}

		private static List<KeyValuePair<Hand, PokerHandBasedHandRank>> GetOrderedListOfDistinctPossibleHandsWithAssociatedRank(List<KeyValuePair<Hand, PokerHandBasedHandRank>> orderedPossibleHands)
		{
			var handRankToCompareAgainst = orderedPossibleHands.First().Value;
			var distinctPossibleHands = new List<KeyValuePair<Hand, PokerHandBasedHandRank>> { orderedPossibleHands.First() };

			foreach (var handHandRankPair in orderedPossibleHands)
			{
				var handRank = handHandRankPair.Value;
				if (handRank.CompareTo(handRankToCompareAgainst) == 0) continue;

				handRankToCompareAgainst = handRank;
				distinctPossibleHands.Add(handHandRankPair);
			}

			return distinctPossibleHands;
		}

		private static List<int> GetFlushesValuesList(List<KeyValuePair<Hand, PokerHandBasedHandRank>> orderedDistinctPossibleHands)
		{
			var flushesList = Enumerable.Repeat(0, 7937).ToList();
			for (var index = 0; index < orderedDistinctPossibleHands.Count; index++)
			{
				var pokerHand = orderedDistinctPossibleHands[index].Value.PokerHand;
				if (pokerHand != PokerHand.Flush && pokerHand != PokerHand.StraightFlush && pokerHand != PokerHand.RoyalFlush) continue;

				var hand = orderedDistinctPossibleHands[index].Key;
				var handCardsValue = (int)hand.Cards.Sum(x => Math.Pow(2, (int)x.Value - 2));

				flushesList[handCardsValue] = index + 1;
			}

			return flushesList;
		}
	}
}
