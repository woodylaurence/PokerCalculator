using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class DeckUnitTests : AbstractUnitTestBase
	{
		private Deck _instance;

		[SetUp]
		public new void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<Deck>();

			Deck.MethodObject = MockRepository.GenerateStrictMock<Deck>();
			Card.MethodObject = MockRepository.GenerateStrictMock<Card>();
			MyRandom.MethodObject = MockRepository.GenerateStrictMock<MyRandom>();
		}

		[TearDown]
		public void TearDown()
		{
			Deck.MethodObject = new Deck();
			Card.MethodObject = new Card();
			MyRandom.MethodObject = new MyRandom();
		}

		#region Create

		[Test]
		public void Create_calls_slave()
		{
			//arrange
			Deck.MethodObject.Expect(x => x.CreateSlave()).Return(_instance);

			//act
			var actual = Deck.Create();

			//assert
			Deck.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void CreateSlave()
		{
			//arrange
			_instance.Stub(x => x.GetAllCardSuits()).Return(new List<CardSuit> { CardSuit.Clubs, CardSuit.Hearts });
			_instance.Stub(x => x.GetAllCardValues()).Return(new List<CardValue> { CardValue.Eight, CardValue.King });

			var card1 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.Eight, CardSuit.Clubs)).Return(card1);

			var card2 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.Eight, CardSuit.Hearts)).Return(card2);

			var card3 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.King, CardSuit.Hearts)).Return(card3);

			var card4 = MockRepository.GenerateStrictMock<Card>();
			Card.MethodObject.Stub(x => x.CreateSlave(CardValue.King, CardSuit.Clubs)).Return(card4);

			//act
			var actual = _instance.CreateSlave();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card4));
		}

		#region GetAllCardSuits

		[Test]
		public void GetAllCardSuits()
		{
			//act
			var actual = _instance.GetAllCardSuits();

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Spades));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Hearts));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Diamonds));
			Assert.That(actual, Has.Some.EqualTo(CardSuit.Clubs));
		}

		#endregion

		#region GetAllCardValues

		[Test]
		public void GetAllCardValues()
		{
			//act
			var actual = _instance.GetAllCardValues();

			//assert
			Assert.That(actual, Has.Count.EqualTo(13));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Ace));
			Assert.That(actual, Has.Some.EqualTo(CardValue.King));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Queen));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Jack));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Ten));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Nine));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Eight));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Seven));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Six));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Five));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Four));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Three));
			Assert.That(actual, Has.Some.EqualTo(CardValue.Two));
		}

		#endregion

		#endregion

		#region CreateShuffledDeck

		[Test]
		public void CreateShuffledDeck_calls_slave()
		{
			//arrange
			Deck.MethodObject.Expect(x => x.CreateShuffledDeckSlave()).Return(_instance);

			//act
			var actual = Deck.CreateShuffledDeck();

			//assert
			Deck.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test]
		public void CreateShuffledDeckSlave()
		{
			//arrange
			var expected = MockRepository.GenerateStrictMock<Deck>();
			Deck.MethodObject.Stub(x => x.CreateSlave()).Return(expected);
			expected.Expect(x => x.Shuffle());

			//act
			var actual = _instance.CreateShuffledDeckSlave();

			//assert
			expected.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region Shuffle

		[Test]
		public void Shuffle()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();
			var card3 = MockRepository.GenerateStrictMock<Card>();
			var card4 = MockRepository.GenerateStrictMock<Card>();

			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				card1, card2, card3, card4
			});

			card1.Stub(x => x.Value).Return(CardValue.Ace);
			card2.Stub(x => x.Value).Return(CardValue.Two);
			card3.Stub(x => x.Value).Return(CardValue.Three);
			card4.Stub(x => x.Value).Return(CardValue.Four);

			const int firstRandomNumber = 1781;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(firstRandomNumber).Repeat.Once();

			const int secondRandomNumber = 514;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(secondRandomNumber).Repeat.Once();

			const int thirdRandomNumber = 4981;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(thirdRandomNumber).Repeat.Once();

			const int fourthRandomNumber = 45;
			MyRandom.MethodObject.Stub(x => x.GenerateRandomNumberSlave(5000)).Return(fourthRandomNumber).Repeat.Once();

			_instance.Expect(x => x.Cards = Arg<List<Card>>.Matches(y => y.Count == 4 &&
			                                                             y[0] == card4 &&
			                                                             y[1] == card2 &&
			                                                             y[2] == card1 &&
			                                                             y[3] == card3));

			//act
			_instance.Shuffle();

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion
	}
}
