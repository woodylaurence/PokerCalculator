using System;
using System.Collections.Generic;
using System.Linq;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Domain.PokerObjects
{
	public class Deck
	{
		#region Properties and Fields

		internal static Deck MethodObject = new Deck();
		public virtual List<Card> Cards { get; set; }

		#endregion

		#region Static and Factory Methods

		#region Create

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Deck Create() { return MethodObject.CreateSlave();}
		protected internal virtual Deck CreateSlave()
		{
			var cardSuits = GetAllCardSuits();
			var cardValues = GetAllCardValues();

			return new Deck
			{
				Cards = cardSuits.SelectMany(suit => cardValues.Select(value => Card.Create(value, suit))).ToList()
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected internal virtual List<CardSuit> GetAllCardSuits()
		{
			return Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected internal virtual List<CardValue> GetAllCardValues()
		{
			return Enum.GetValues(typeof(CardValue)).Cast<CardValue>().ToList();
		}

		#endregion

		#region Create Shuffled Deck

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Deck CreateShuffledDeck() { return MethodObject.CreateShuffledDeckSlave(); }
		protected internal virtual Deck CreateShuffledDeckSlave()
		{
			var deck = Create();
			deck.Shuffle();
			return deck;
		}

		#endregion

		#endregion

		#region Instance Methods

		#region Shuffle

		public virtual void Shuffle()
		{
			Cards = Cards.OrderBy(x => MyRandom.GenerateRandomNumber(5000)).ToList();
		}

		#endregion

		#endregion
	}
}
