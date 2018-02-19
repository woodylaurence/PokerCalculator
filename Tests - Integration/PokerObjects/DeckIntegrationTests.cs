using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using System;
using System.Linq;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class DeckIntegrationTests : LocalTestBase
	{
		#region Constructor

		[Test]
		public void Constructor_SHOULD_return_deck_full_of_every_card()
		{
			//act
			var actual = new Deck();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(52));

			var allCards = CardTestCases.AllCards;
			for (var i = 0; i < 52; i++)
			{
				Assert.That(actual.Cards[i], Is.EqualTo(allCards[i]).Using(CardComparer));
			}
		}

		[Test]
		public void Constructor_with_random_SHOULD_return_deck_full_of_every_card()
		{
			//act
			var actual = new Deck(new Random(), UtilitiesService);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(52));

			var allCards = CardTestCases.AllCards;
			for (var i = 0; i < 52; i++)
			{
				Assert.That(actual.Cards[i], Is.EqualTo(allCards[i]).Using(CardComparer));
			}
		}

		#endregion

		#region Shuffle

		[Test]
		public void Shuffle()
		{
			//arrange
			var instance = new Deck();
			var originalCards = instance.Cards.ToList();

			//act
			instance.Shuffle();

			//assert
			Assert.That(instance.Cards.TrueForAll(x => originalCards.Contains(x, CardComparer)));
			Assert.That(originalCards.TrueForAll(x => instance.Cards.Contains(x, CardComparer)));

			var numCardsInSameOrderAsBefore = 0;
			var orderedCards = CardTestCases.AllCards;
			for (var i = 0; i < 52; i++)
			{
				if (CardComparer.Equals(instance.Cards[i], orderedCards[i])) numCardsInSameOrderAsBefore++;
			}

			//Represents roughly 1/300,000,000 chance
			Assert.That(numCardsInSameOrderAsBefore, Is.LessThanOrEqualTo(5));
		}

		#endregion

		#region RemoveCard

		[Test]
		public void RemoveCard()
		{
			//arrange
			var instance = new Deck();
			var cardToRemove = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			instance.RemoveCard(cardToRemove);

			//assert
			Assert.That(instance.Cards, Has.Count.EqualTo(51));
			Assert.That(instance.Cards, Has.None.EqualTo(cardToRemove));
		}

		#endregion

		#region TakeRandomCard

		[Test]
		public void TakeRandomCard()
		{
			//arrange
			var instance = new Deck();

			//act
			var actual = instance.TakeRandomCard();

			//assert
			Assert.That(instance.Cards, Has.Count.EqualTo(51));
			Assert.That(instance.Cards, Has.None.EqualTo(actual).Using(CardComparer));
		}

		#endregion

		#region GetRandomCards

		[Test]
		public void GetRandomCards()
		{
			//arrange
			var instance = new Deck();

			//act
			var actual = instance.GetRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));

			Assert.That(instance.Cards, Has.Count.EqualTo(52));
			Assert.That(instance.Cards, Has.Some.EqualTo(actual[0]).Using(CardComparer));
			Assert.That(instance.Cards, Has.Some.EqualTo(actual[1]).Using(CardComparer));
			Assert.That(instance.Cards, Has.Some.EqualTo(actual[2]).Using(CardComparer));
		}

		#endregion
	}
}
