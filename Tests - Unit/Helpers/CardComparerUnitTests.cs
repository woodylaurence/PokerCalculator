using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;

namespace PokerCalculator.Tests.Unit.Helpers
{
	[TestFixture]
	public class CardComparerUnitTests : AbstractUnitTestBase
	{
		CardComparer _instance;

		[SetUp]
		protected override void Setup()
		{
			_instance = new CardComparer();
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
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = _instance.Equals(null, card);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_second_card_is_null_and_first_is_not_SHOULD_return_false()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = _instance.Equals(card, null);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_cards_are_same_value_in_memory_SHOULD_return_true()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = _instance.Equals(card, card);

			//asssert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_WHERE_values_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = new Card(CardValue.Five, CardSuit.Spades);
			var card2 = new Card(CardValue.Nine, CardSuit.Spades);

			//act
			var actual = _instance.Equals(card1, card2);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_suits_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = new Card(CardValue.Four, CardSuit.Clubs);
			var card2 = new Card(CardValue.Four, CardSuit.Diamonds);

			//act
			var actual = _instance.Equals(card1, card2);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_values_and_suits_are_the_same_SHOULD_return_true()
		{
			//arrange
			var card1 = new Card(CardValue.Ten, CardSuit.Spades);
			var card2 = new Card(CardValue.Ten, CardSuit.Spades);

			//act
			var actual = _instance.Equals(card1, card2);

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region GetHashCode

		[Test]
		public new void GetHashCode()
		{
			//arrange
			var card = new Card(CardValue.Queen, CardSuit.Hearts);

			//act
			var actual = _instance.GetHashCode(card);

			//assert
			Assert.That(actual, Is.Not.Null);
		}

		#endregion
	}
}
