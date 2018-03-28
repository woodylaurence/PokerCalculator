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
		public virtual List<Card> Cards { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public Deck() : this(ServiceLocator.Current.GetInstance<IRandomNumberGenerator>()) { }
		public Deck(IRandomNumberGenerator randomNumberGenerator)
		{
			_randomNumberGenerator = randomNumberGenerator;

			var cardSuits = Utilities.GetEnumValues<CardSuit>();
			var cardValues = Utilities.GetEnumValues<CardValue>();
			Cards = cardSuits.SelectMany(suit => cardValues.Select(value => new Card(value, suit))).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cards"></param>
		public Deck(List<Card> cards)
		{
			_randomNumberGenerator = ServiceLocator.Current.GetInstance<IRandomNumberGenerator>();
			Cards = cards.ToList();
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

		#region Clone

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Deck Clone()
		{
			return new Deck(Cards);
		}

		#endregion

		#region RemoveCard

		/// <summary>
		/// Removes the card.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="suit"></param>
		public virtual void RemoveCard(CardValue value, CardSuit suit)
		{
			TakeCard(value, suit);
		}

		#endregion

		#region TakeCard

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="suit"></param>
		/// <returns></returns>
		public virtual Card TakeCard(CardValue value, CardSuit suit)
		{
			var cardToRemoveInDeck = Cards.FirstOrDefault(x => x.Value == value && x.Suit == suit);
			if (cardToRemoveInDeck == null) throw new Exception("No matching card in Deck.");
			Cards.Remove(cardToRemoveInDeck);
			return cardToRemoveInDeck;
		}

		#endregion

		#region TakeRandomCard

		/// <summary>
		/// Takes the random card.
		/// </summary>
		/// <returns>The random card.</returns>
		public virtual Card TakeRandomCard()
		{
			return TakeRandomCards(1).First();
		}

		#endregion

		#region TakeRandomCards

		/// <summary>
		/// 
		/// </summary>
		/// <param name="numCardsToTake"></param>
		/// <returns></returns>
		public virtual List<Card> TakeRandomCards(int numCardsToTake)
		{
			if (Cards.Count < numCardsToTake) throw new ArgumentException("Cannot take more cards than there are left in the Deck.");

			var cardsToTake = new List<Card>();
			for (var i = 0; i < numCardsToTake; i++)
			{
				var indexOfCardToTake = _randomNumberGenerator.Next(Cards.Count);
				var cardToTake = Cards[indexOfCardToTake];
				Cards.Remove(cardToTake);
				cardsToTake.Add(cardToTake);
			}

			return cardsToTake;
		}

		#endregion

		#region GetRandomCards

		/// <summary>
		/// Gets the random cards.
		/// </summary>
		/// <returns>The random cards.</returns>
		/// <param name="numCardsToGet">Number cards to get.</param>
		public virtual List<Card> GetRandomCards(int numCardsToGet)
		{
			if (numCardsToGet > Cards.Count) throw new Exception("Cannot get more cards than there are left in the Deck.");

			var cardsToTake = new List<Card>();
			var cardsLeftInDeckToSelectFrom = Cards.ToList();
			for (var i = 0; i < numCardsToGet; i++)
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
