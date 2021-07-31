using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class DeckFunctionalTests : AbstractUnitTestBase
	{
		private Deck _instance;
		private CardComparer _cardComparer;
		private int RandomNumberGeneratorSeed { get; set; }

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_cardComparer = new CardComparer();
		}

		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddTransient<IRandomNumberGenerator>(x => new FakeRandomNumberGenerator(RandomNumberGeneratorSeed));
		}

		[TearDown]
		protected void TearDown()
		{
			RandomNumberGeneratorSeed = FakeRandomNumberGenerator.DefaultRandomSeedingValue;
		}

		[Test]
		public void Shuffle()
		{
			//arrange
			_instance = new Deck();

			//act
			_instance.Shuffle();

			//assert (Results are valid for RNG with seed 1337)
			Assert.That(_instance.Cards[0], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Hearts)).Using(_cardComparer));
			Assert.That(_instance.Cards[3], Is.EqualTo(new Card(CardValue.Five, CardSuit.Clubs)).Using(_cardComparer));
			Assert.That(_instance.Cards[10], Is.EqualTo(new Card(CardValue.Two, CardSuit.Spades)).Using(_cardComparer));
			Assert.That(_instance.Cards[20], Is.EqualTo(new Card(CardValue.Ten, CardSuit.Clubs)).Using(_cardComparer));
			Assert.That(_instance.Cards[30], Is.EqualTo(new Card(CardValue.Jack, CardSuit.Diamonds)).Using(_cardComparer));
			Assert.That(_instance.Cards[40], Is.EqualTo(new Card(CardValue.Five, CardSuit.Diamonds)).Using(_cardComparer));
			Assert.That(_instance.Cards[50], Is.EqualTo(new Card(CardValue.Eight, CardSuit.Spades)).Using(_cardComparer));
		}

		[Test]
		public void Clone_SHOULD_create_deck_with_different_instance_of_random_number_generator()
		{
			//arrange
			_instance = new Deck();

			//to show that we use a random number generator with this value, not the default one used by the above deck
			RandomNumberGeneratorSeed = 123456789;

			//act
			var actual = _instance.Clone();

			//assert
			var randomCardFromOriginalDeck = _instance.TakeRandomCard();
			var randomCardFromClonedDeck = actual.TakeRandomCard();
			Assert.That(randomCardFromClonedDeck, Is.Not.EqualTo(randomCardFromOriginalDeck).Using(_cardComparer));
		}
	}
}