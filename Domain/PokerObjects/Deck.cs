using Microsoft.Practices.ServiceLocation;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Deck
	{
		#region Properties and Fields

		private IRandomNumberGenerator _randomNumberGenerator { get; }
		private IUtilitiesService _utilities { get; }
		public virtual List<Card> Cards { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public Deck() : this(ServiceLocator.Current.GetInstance<IRandomNumberGenerator>(), ServiceLocator.Current.GetInstance<IUtilitiesService>()) { }
		public Deck(IRandomNumberGenerator randomNumberGenerator, IUtilitiesService utilitiesService)
		{
			_randomNumberGenerator = randomNumberGenerator;
			_utilities = utilitiesService;

			var cardSuits = utilitiesService.GetEnumValues<CardSuit>();
			var cardValues = utilitiesService.GetEnumValues<CardValue>();
			Cards = cardSuits.SelectMany(suit => cardValues.Select(value => new Card(value, suit))).ToList();
		}

		#endregion

		#region Instance Methods 

		#region Shuffle

		/// <summary>
		/// Shuffle this instance.
		/// </summary>
		public virtual void Shuffle()
		{
			Cards = Cards.OrderBy(x => _randomNumberGenerator.Next(5000)).ToList();
		}

		#endregion

		#region RemoveCard

		/// <summary>
		/// Removes the card.
		/// </summary>
		/// <param name="cardToRemove">Card to remove.</param>
		public virtual void RemoveCard(Card cardToRemove)
		{
			var cardToRemoveInDeck = Cards.FirstOrDefault(x => new CardComparer().Equals(x, cardToRemove));
			if (cardToRemoveInDeck == null) throw new Exception("Cannot remove Card, it is not in Deck.");
			Cards.Remove(cardToRemoveInDeck);
		}

		#endregion

		#region TakeRandomCard

		/// <summary>
		/// Takes the random card.
		/// </summary>
		/// <returns>The random card.</returns>
		public virtual Card TakeRandomCard()
		{
			if (Cards.Any() == false) throw new Exception("No cards left in Deck to take.");

			var indexOfCardToTake = _randomNumberGenerator.Next(Cards.Count);
			var cardToTake = Cards[indexOfCardToTake];
			Cards.Remove(cardToTake);
			return cardToTake;
		}

		#endregion

		#region GetRandomCards

		/// <summary>
		/// Gets the random cards.
		/// </summary>
		/// <returns>The random cards.</returns>
		/// <param name="numCardsToTake">Number cards to take.</param>
		public virtual List<Card> GetRandomCards(int numCardsToTake)
		{
			if (numCardsToTake > Cards.Count) throw new Exception("Cannot get more cards than there are left in the Deck.");

			var cardsToTake = new List<Card>();
			var cardsLeftInDeckToSelectFrom = Cards.ToList();
			for (var i = 0; i < numCardsToTake; i++)
			{
				var indexOfCardToTake = _randomNumberGenerator.Next(cardsLeftInDeckToSelectFrom.Count);
				var cardToTake = cardsLeftInDeckToSelectFrom[indexOfCardToTake];
				cardsToTake.Add(cardToTake);
				cardsLeftInDeckToSelectFrom.Remove(cardToTake);
			}
			return cardsToTake;
		}

		#endregion

		#endregion
	}
}
