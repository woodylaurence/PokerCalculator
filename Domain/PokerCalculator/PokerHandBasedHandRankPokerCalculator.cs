using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerCalculator
{
	public class PokerHandBasedHandRankPokerCalculator : IPokerCalculator
	{
		private readonly IHandRankCalculator<PokerHandBasedHandRank, PokerHand> _handRankCalculator;

		#region Constructor

		public PokerHandBasedHandRankPokerCalculator(IHandRankCalculator<PokerHandBasedHandRank, PokerHand> handRankCalculator)
		{
			_handRankCalculator = handRankCalculator;
		}

		#endregion

		#region Instance Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deck"></param>
		/// <param name="myHand"></param>
		/// <param name="boardHand"></param>
		/// <param name="numOpponents"></param>
		/// <param name="numIterations"></param>
		/// <returns></returns>
		public PokerOdds CalculatePokerOdds(Deck deck, Hand myHand, Hand boardHand, int numOpponents, int numIterations)
		{
			const int numBatches = 5;
			var listOfPokerOdds = new List<PokerOdds>();
			for (var i = 0; i < numBatches; i++)
			{
				var pokerOddsForBatch = new PokerOdds();
				for (var j = 0; j < numIterations / numBatches; j++)
				{
					ExecuteCalculatePokerOddsForIteration(deck, myHand, boardHand, numOpponents, pokerOddsForBatch);
				}
				listOfPokerOdds.Add(pokerOddsForBatch);
			}
			return PokerOdds.AggregatePokerOdds(listOfPokerOdds);
		}

		#region ExecuteCalculatePokerOddsForIteration

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deck"></param>
		/// <param name="myHand"></param>
		/// <param name="boardHand"></param>
		/// <param name="numOpponents"></param>
		/// <param name="pokerOdds"></param>
		private void ExecuteCalculatePokerOddsForIteration(Deck deck, Hand myHand, Hand boardHand, int numOpponents, PokerOdds pokerOdds)
		{
			var clonedDeck = deck.Clone();
			var clonedMyHand = myHand.Clone();
			DealRequiredNumberOfCardsToHand(clonedMyHand, clonedDeck, 2);

			var clonedBoardHand = boardHand.Clone();
			DealRequiredNumberOfCardsToHand(clonedBoardHand, clonedDeck, 5);
			clonedMyHand += clonedBoardHand;

			var myHandRank = _handRankCalculator.CalculateHandRank(clonedMyHand);
			var bestOpponentHand = SimulateOpponentHandsAndReturnBestHand(clonedDeck, clonedBoardHand, numOpponents);
			var bestOpponentHandRank = bestOpponentHand == null ? null : _handRankCalculator.CalculateHandRank(bestOpponentHand);

			if (myHandRank.IsLessThan(bestOpponentHandRank)) pokerOdds.NumLosses++;
			else if (myHandRank.IsGreaterThan(bestOpponentHandRank)) pokerOdds.NumWins++;
			else pokerOdds.NumDraws++;

			pokerOdds.PokerHandFrequencies[myHandRank.PokerHand]++;
		}

		#endregion

		#region DealRequiredNumberOfCardsToHand

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hand"></param>
		/// <param name="deck"></param>
		/// <param name="numCardsRequiredInHand"></param>
		private void DealRequiredNumberOfCardsToHand(Hand hand, Deck deck, int numCardsRequiredInHand)
		{
			var numCardsToDealToHand = numCardsRequiredInHand - hand.Cards.Count;
			if (numCardsToDealToHand <= 0) return;

			var randomCards = deck.TakeRandomCards(numCardsToDealToHand);
			hand.AddCards(randomCards);
		}

		#endregion

		#region SimulateOpponentHandsAndReturnBestHand

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deck"></param>
		/// <param name="boardHand"></param>
		/// <param name="numOpponents"></param>
		/// <returns></returns>
		private Hand SimulateOpponentHandsAndReturnBestHand(Deck deck, Hand boardHand, int numOpponents)
		{
			return Enumerable.Range(0, numOpponents)
							 .Select(_ => new Hand(deck.TakeRandomCards(2)) + boardHand)
							 .OrderByDescending(_handRankCalculator.CalculateHandRank)
							 .FirstOrDefault();
		}

		#endregion

		#endregion
	}
}
