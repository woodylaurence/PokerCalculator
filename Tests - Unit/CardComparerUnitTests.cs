using NUnit.Framework;
using Rhino.Mocks;
using PokerCalculator.Domain;
using PokerCalculator.Domain.PokerObjects;
using System.Security.Cryptography.X509Certificates;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Unit
{
	[TestFixture]
	public class CardComparerUnitTests
	{
		CardComparer _instance;

		[SetUp]
		public void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<CardComparer>();
		}

		#region Equals

		[Test]
		public void Equals_WHERE_both_cards_are_null_SHOULD_return_true()
		{
			//act
			var actual = _instance.Equals(null, null);

			//asssert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_WHERE_first_card_is_null_and_second_is_not_SHOULD_return_false()
		{
			//arrange
			var card = MockRepository.GenerateStrictMock<Card>();

			//act
			var actual = _instance.Equals(null, card);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_second_card_is_null_and_first_is_not_SHOULD_return_false()
		{
			//arrange
			var card = MockRepository.GenerateStrictMock<Card>();

			//act
			var actual = _instance.Equals(card, null);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_cards_are_same_value_in_memory_SHOULD_return_true()
		{
			//arrange
			var card = MockRepository.GenerateStrictMock<Card>();

			//act
			var actual = _instance.Equals(card, card);

			//asssert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_WHERE_values_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();

			card1.Stub(x => x.Value).Return(CardValue.Five);
			card2.Stub(x => x.Value).Return(CardValue.Nine);

			//act
			var actual = _instance.Equals(card1, card2);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_suits_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();

			card1.Stub(x => x.Value).Return(CardValue.Four);
			card2.Stub(x => x.Value).Return(CardValue.Four);

			card1.Stub(x => x.Suit).Return(CardSuit.Clubs);
			card2.Stub(x => x.Suit).Return(CardSuit.Diamonds);

			//act
			var actual = _instance.Equals(card1, card2);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_values_and_suits_are_the_same_SHOULD_return_true()
		{
			//arrange
			var card1 = MockRepository.GenerateStrictMock<Card>();
			var card2 = MockRepository.GenerateStrictMock<Card>();

			card1.Stub(x => x.Value).Return(CardValue.Ten);
			card2.Stub(x => x.Value).Return(CardValue.Ten);

			card1.Stub(x => x.Suit).Return(CardSuit.Spades);
			card2.Stub(x => x.Suit).Return(CardSuit.Spades);

			//act
			var actual = _instance.Equals(card1, card2);

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region

		[Test]
		public new void GetHashCode()
		{
			//arrange
			var card = MockRepository.GenerateStrictMock<Card>();

			card.Stub(x => x.Value).Return(CardValue.Queen);
			card.Stub(x => x.Suit).Return(CardSuit.Hearts);

			//act
			var actual = _instance.GetHashCode();

			//assert
			Assert.That(actual, Is.Not.Null);
		}

		#endregion
	}
}
