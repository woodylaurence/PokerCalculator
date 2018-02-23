using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;
using PokerCalculator.Tests.Shared.TestObjects;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class DeckIntegrationTests : LocalTestBase
	{
		private IRandomNumberGenerator _fakeRandomNumberGenerator;

		[SetUp]
		public override void Setup()
		{
			_fakeRandomNumberGenerator = new FakeRandomNumberGenerator();

			base.Setup();
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_fakeRandomNumberGenerator));
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
			//act
			var actual = new Deck(_fakeRandomNumberGenerator, UtilitiesService);

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(52));

			var allCards = CardTestCaseData.AllCards;
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

			//act
			instance.Shuffle();

			//assert - card results valid for deck seeding value 1337
			Assert.That(instance.Cards[0], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Hearts)).Using(CardComparer));
			Assert.That(instance.Cards[3], Is.EqualTo(new Card(CardValue.Five, CardSuit.Clubs)).Using(CardComparer));
			Assert.That(instance.Cards[10], Is.EqualTo(new Card(CardValue.Two, CardSuit.Spades)).Using(CardComparer));
			Assert.That(instance.Cards[20], Is.EqualTo(new Card(CardValue.Ten, CardSuit.Clubs)).Using(CardComparer));
			Assert.That(instance.Cards[30], Is.EqualTo(new Card(CardValue.Jack, CardSuit.Diamonds)).Using(CardComparer));
			Assert.That(instance.Cards[40], Is.EqualTo(new Card(CardValue.Five, CardSuit.Diamonds)).Using(CardComparer));
			Assert.That(instance.Cards[50], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Spades)).Using(CardComparer));
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

			//assert - card results valid for deck seeding value 1337
			Assert.That(actual, Is.EqualTo(new Card(CardValue.Queen, CardSuit.Spades)).Using(CardComparer));
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

			//assert - card results valid for deck seeding value 1337
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo(new Card(CardValue.Queen, CardSuit.Spades)).Using(CardComparer));
			Assert.That(actual[1], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Spades)).Using(CardComparer));
			Assert.That(actual[2], Is.EqualTo(new Card(CardValue.Six, CardSuit.Hearts)).Using(CardComparer));

			Assert.That(instance.Cards, Has.Count.EqualTo(52));
			Assert.That(instance.Cards, Has.Some.EqualTo(actual[0]).Using(CardComparer));
			Assert.That(instance.Cards, Has.Some.EqualTo(actual[1]).Using(CardComparer));
			Assert.That(instance.Cards, Has.Some.EqualTo(actual[2]).Using(CardComparer));
		}

		#endregion
	}
}
