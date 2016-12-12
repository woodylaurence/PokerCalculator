using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Domain.PokerEnums;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class CardUnitTests : AbstractUnitTestBase
	{
		Card _instance;

		[SetUp]
		public new void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<Card>();

			Card.MethodObject = MockRepository.GenerateStrictMock<Card>();
		}

		[TearDown]
		public void TearDown()
		{
			Card.MethodObject = new Card();
		}

		#region Create

		[Test]
		public void Create_calls_slave()
		{
			//arrange
			const CardValue cardValue = CardValue.Ace;
			const CardSuit cardSuit = CardSuit.Spades;

			Card.MethodObject.Expect(x => x.CreateSlave(cardValue, cardSuit)).Return(_instance);

			//act
			var actual = Card.Create(cardValue, cardSuit);

			//assert
			Card.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(_instance));
		}

		[Test, TestCaseSource(typeof(CardTestCases), "AllCardsTestCaseData")]
		public void CreateSlave(CardValue value, CardSuit suit)
		{
			//act
			var actual = _instance.CreateSlave(value, suit);

			//assert
			Assert.That(actual.Value, Is.EqualTo(value));
			Assert.That(actual.Suit, Is.EqualTo(suit));
		}

		#endregion
	}

	public static class CardTestCases
	{
		public static IEnumerable<TestCaseData> AllCardsTestCaseData
		{
			get
			{
				var cardSuits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToList();
				var cardValues = Enum.GetValues(typeof(CardValue)).Cast<CardValue>().ToList();

				return cardSuits.SelectMany(cardSuit => cardValues.Select(cardValue => new TestCaseData(cardValue, cardSuit)));
			}
		}
	}
}
