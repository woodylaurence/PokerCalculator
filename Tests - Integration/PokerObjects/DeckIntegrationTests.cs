using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared.TestData;
using PokerCalculator.Tests.Shared.TestObjects;
using System.Configuration;
using System.Linq;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class DeckIntegrationTests : LocalTestBase
	{
		private IRandomNumberGenerator _fakeRandomNumberGenerator;
		private Deck _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_fakeRandomNumberGenerator = new FakeRandomNumberGenerator();
			_instance = new Deck(_fakeRandomNumberGenerator);
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().ImplementedBy<FakeRandomNumberGenerator>().LifestyleTransient());
		}

		[TearDown]
		protected void TearDown()
		{
			ConfigurationManager.AppSettings["PokerCalculator.Helpers.FakeRandomNumberGenerator.RandomSeedingValue"] = "1337";
		}

		#region Constructor

		[Test]
		public void Constructor_SHOULD_return_deck_full_of_every_card()
		{
			//act
			var actual = new Deck();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(52));

			var allCards = CardTestCaseData.AllCards;
			for (var i = 0; i < 52; i++)
			{
				Assert.That(actual.Cards[i], Is.EqualTo(allCards[i]).Using(CardComparer));
			}
		}

		[Test]
		public void Constructor_with_random_SHOULD_return_deck_full_of_every_card()
		{
			//act - in setup

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(52));

			var allCards = CardTestCaseData.AllCards;
			for (var i = 0; i < 52; i++)
			{
				Assert.That(_instance.Cards[i], Is.EqualTo(allCards[i]).Using(CardComparer));
			}
		}

		[Test]
		public void Constructor_with_cards_SHOULD_return_deck_with_supplied_cards()
		{
			//arrange
			var originalDeck = new Deck();

			originalDeck.RemoveCard(CardValue.Ace, CardSuit.Diamonds);
			originalDeck.RemoveCard(CardValue.Jack, CardSuit.Hearts);
			originalDeck.RemoveCard(CardValue.Ace, CardSuit.Spades);

			//act
			var actual = new Deck(originalDeck.Cards);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(49));
			Assert.That(actual.Cards, Has.None.EqualTo(new Card(CardValue.Ace, CardSuit.Diamonds)).Using(CardComparer));
			Assert.That(actual.Cards, Has.None.EqualTo(new Card(CardValue.Jack, CardSuit.Hearts)).Using(CardComparer));
			Assert.That(actual.Cards, Has.None.EqualTo(new Card(CardValue.Ace, CardSuit.Spades)).Using(CardComparer));

			originalDeck.RemoveCard(CardValue.Five, CardSuit.Diamonds);
			Assert.That(actual.Cards, Has.Some.EqualTo(new Card(CardValue.Five, CardSuit.Diamonds)).Using(CardComparer));
		}

		#endregion

		#region Shuffle

		[Test]
		public void Shuffle()
		{
			//act
			_instance.Shuffle();

			//assert - card results valid for deck seeding value 1337
			Assert.That(_instance.Cards[0], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Hearts)).Using(CardComparer));
			Assert.That(_instance.Cards[3], Is.EqualTo(new Card(CardValue.Five, CardSuit.Clubs)).Using(CardComparer));
			Assert.That(_instance.Cards[10], Is.EqualTo(new Card(CardValue.Two, CardSuit.Spades)).Using(CardComparer));
			Assert.That(_instance.Cards[20], Is.EqualTo(new Card(CardValue.Ten, CardSuit.Clubs)).Using(CardComparer));
			Assert.That(_instance.Cards[30], Is.EqualTo(new Card(CardValue.Jack, CardSuit.Diamonds)).Using(CardComparer));
			Assert.That(_instance.Cards[40], Is.EqualTo(new Card(CardValue.Five, CardSuit.Diamonds)).Using(CardComparer));
			Assert.That(_instance.Cards[50], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Spades)).Using(CardComparer));
		}

		#endregion

		#region Clone

		[Test]
		public void Clone()
		{
			//arrange
			_instance.TakeRandomCards(4);
			ConfigurationManager.AppSettings["PokerCalculator.Helpers.FakeRandomNumberGenerator.RandomSeedingValue"] = "123456789";

			//act
			var actual = _instance.Clone();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(_instance.Cards.Count));
			Assert.That(_instance.Cards.TrueForAll(cardInOriginalDeck => actual.Cards.Contains(cardInOriginalDeck, CardComparer)));

			var randomCardsFromClonedDeck = actual.TakeRandomCards(2);
			Assert.That(randomCardsFromClonedDeck[0], Is.EqualTo(new Card(CardValue.Three, CardSuit.Diamonds)).Using(CardComparer));
			Assert.That(randomCardsFromClonedDeck[1], Is.EqualTo(new Card(CardValue.Jack, CardSuit.Spades)).Using(CardComparer));
		}

		#endregion

		#region RemoveCard

		[Test]
		public void RemoveCard()
		{
			//act
			_instance.RemoveCard(CardValue.Jack, CardSuit.Spades);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(51));
			Assert.That(_instance.Cards, Has.None.EqualTo(new Card(CardValue.Jack, CardSuit.Spades)).Using(CardComparer));
		}

		#endregion

		#region TakeCard

		[Test]
		public void TakeCard()
		{
			//arrange
			const CardValue value = CardValue.Eight;
			const CardSuit suit = CardSuit.Diamonds;

			//act
			var actual = _instance.TakeCard(value, suit);

			//assert
			Assert.That(actual.Value, Is.EqualTo(value));
			Assert.That(actual.Suit, Is.EqualTo(suit));

			Assert.That(_instance.Cards, Has.Count.EqualTo(51));
			Assert.That(_instance.Cards, Has.None.EqualTo(new Card(value, suit)).Using(CardComparer));
		}

		#endregion

		#region TakeRandomCard

		[Test]
		public void TakeRandomCard()
		{
			//act
			var actual = _instance.TakeRandomCard();

			//assert - card results valid for deck seeding value 1337
			Assert.That(actual, Is.EqualTo(new Card(CardValue.Queen, CardSuit.Spades)).Using(CardComparer));
			Assert.That(_instance.Cards, Has.Count.EqualTo(51));
			Assert.That(_instance.Cards, Has.None.EqualTo(actual).Using(CardComparer));
		}

		#endregion

		#region TakeRandomCards

		[Test]
		public void TakeRandomCards()
		{
			//act
			var actual = _instance.TakeRandomCards(4);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual[0], Is.EqualTo(new Card(CardValue.Queen, CardSuit.Spades)).Using(CardComparer));
			Assert.That(actual[1], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Spades)).Using(CardComparer));
			Assert.That(actual[2], Is.EqualTo(new Card(CardValue.Six, CardSuit.Hearts)).Using(CardComparer));
			Assert.That(actual[3], Is.EqualTo(new Card(CardValue.Jack, CardSuit.Clubs)).Using(CardComparer));

			Assert.That(_instance.Cards, Has.Count.EqualTo(48));
			Assert.That(_instance.Cards, Has.None.EqualTo(actual[0]).Using(CardComparer));
			Assert.That(_instance.Cards, Has.None.EqualTo(actual[1]).Using(CardComparer));
			Assert.That(_instance.Cards, Has.None.EqualTo(actual[2]).Using(CardComparer));
			Assert.That(_instance.Cards, Has.None.EqualTo(actual[3]).Using(CardComparer));
		}

		#endregion

		#region GetRandomCards

		[Test]
		public void GetRandomCards()
		{
			//act
			var actual = _instance.GetRandomCards(3);

			//assert - card results valid for deck seeding value 1337
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(new Card(CardValue.Queen, CardSuit.Spades)).Using(CardComparer));
			Assert.That(actual[1], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Spades)).Using(CardComparer));
			Assert.That(actual[2], Is.EqualTo(new Card(CardValue.Six, CardSuit.Hearts)).Using(CardComparer));

			Assert.That(_instance.Cards, Has.Count.EqualTo(52));
			Assert.That(_instance.Cards, Has.Some.EqualTo(actual[0]).Using(CardComparer));
			Assert.That(_instance.Cards, Has.Some.EqualTo(actual[1]).Using(CardComparer));
			Assert.That(_instance.Cards, Has.Some.EqualTo(actual[2]).Using(CardComparer));
		}

		#endregion
	}
}
