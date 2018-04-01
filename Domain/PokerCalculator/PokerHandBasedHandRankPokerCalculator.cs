using PokerCalculator.Domain.Extensions;
using PokerCalculator.Domain.HandRankCalculator;
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
				var pokerOddsForBatch = InitializePokerOdds();
				for (var j = 0; j < numIterations / numBatches; j++)
				{
					ExecuteCalculatePokerOddsForIteration(deck, myHand, boardHand, numOpponents, pokerOddsForBatch);
				}
				listOfPokerOdds.Add(pokerOddsForBatch);
			}
			return PokerOdds.AggregatePokerOdds(listOfPokerOdds);
		}

		#region InitializePokerOdds

		protected internal virtual PokerOdds InitializePokerOdds()
		{
			return new PokerOdds();
		}

		#endregion

		#region ExecuteCalculatePokerOddsForIteration

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deck"></param>
		/// <param name="myHand"></param>
		/// <param name="boardHand"></param>
		/// <param name="numOpponents"></param>
		/// <param name="pokerOdds"></param>
		protected internal virtual void ExecuteCalculatePokerOddsForIteration(Deck deck, Hand myHand, Hand boardHand, int numOpponents, PokerOdds pokerOdds)
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
		protected internal virtual void DealRequiredNumberOfCardsToHand(Hand hand, Deck deck, int numCardsRequiredInHand)
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
		/// <param name="boardhand"></param>
		/// <param name="numOpponents"></param>
		/// <returns></returns>
		protected internal virtual Hand SimulateOpponentHandsAndReturnBestHand(Deck deck, Hand boardhand, int numOpponents)
		{
			var opponentsHands = new List<Hand>();
			for (var i = 0; i < numOpponents; i++)
			{
				var opponentHand = ConstructTwoCardOpponentHand(deck);
				opponentHand += boardhand;
				opponentsHands.Add(opponentHand);
			}

			return GetBestOpponentHand(opponentsHands);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deck"></param>
		/// <returns></returns>
		protected internal virtual Hand ConstructTwoCardOpponentHand(Deck deck)
		{
			return new Hand(deck.TakeRandomCards(2));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="opponentHands"></param>
		/// <returns></returns>
		protected internal virtual Hand GetBestOpponentHand(List<Hand> opponentHands)
		{
			return opponentHands.Count < 2
						? opponentHands.FirstOrDefault()
						: opponentHands.OrderBy(x => _handRankCalculator.CalculateHandRank(x)).Last();
		}

		#endregion

		#endregion
	}
}
