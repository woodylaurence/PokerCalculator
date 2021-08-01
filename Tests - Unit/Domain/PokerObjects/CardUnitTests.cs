using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.Domain.TestData;

namespace PokerCalculator.Tests.Unit.Domain.PokerObjects
{
	[TestFixture]
	public class CardUnitTests : AbstractUnitTestBase
	{
		#region Constructor

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsTestCaseData))]
		public void Constructor(CardValue value, CardSuit suit)
		{
			//act
			var actual = new Card(value, suit);

			//assert
			Assert.That(actual.Value, Is.EqualTo(value));
			Assert.That(actual.Suit, Is.EqualTo(suit));
		}

		#endregion

		#region Equals

		[Test]
		public void Equals_WHERE_second_card_is_null_and_first_is_not_SHOULD_return_false()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = card.Equals(null);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_cards_are_same_value_in_memory_SHOULD_return_true()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = card.Equals(card);

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_WHERE_values_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = new Card(CardValue.Five, CardSuit.Spades);
			var card2 = new Card(CardValue.Nine, CardSuit.Spades);

			//act
			var actual = card1.Equals(card2);

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
			var actual = card1.Equals(card2);

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
			var actual = card1.Equals(card2);

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region ==

		[Test]
		public void Equals_operator_WHERE_both_cards_are_null_SHOULD_return_true()
		{
			//act
			var actual = (Card)null == null;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_operator_WHERE_first_card_is_null_and_second_is_not_SHOULD_return_false()
		{
			//arrange
			var card2 = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = (Card)null == card2;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_operator_WHERE_second_card_is_null_and_first_is_not_SHOULD_return_false()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = card == null;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_operator_WHERE_cards_are_same_value_in_memory_SHOULD_return_true()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = card == card;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_operator_WHERE_values_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = new Card(CardValue.Five, CardSuit.Spades);
			var card2 = new Card(CardValue.Nine, CardSuit.Spades);

			//act
			var actual = card1 == card2;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_operator_WHERE_suits_are_different_SHOULD_return_false()
		{
			//arrange
			var card1 = new Card(CardValue.Four, CardSuit.Clubs);
			var card2 = new Card(CardValue.Four, CardSuit.Diamonds);

			//act
			var actual = card1 == card2;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_operator_WHERE_values_and_suits_are_the_same_SHOULD_return_true()
		{
			//arrange
			var card1 = new Card(CardValue.Ten, CardSuit.Spades);
			var card2 = new Card(CardValue.Ten, CardSuit.Spades);

			//act
			var actual = card1 == card2;

			//assert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region !=

		[Test]
		public void NotEquals_operator_WHERE_both_cards_are_null_SHOULD_return_false()
		{
			//act
			var actual = (Card)null != null;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void NotEquals_operator_WHERE_first_card_is_null_and_second_is_not_SHOULD_return_true()
		{
			//arrange
			var card2 = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = (Card)null != card2;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void NotEquals_operator_WHERE_second_card_is_null_and_first_is_not_SHOULD_return_true()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = card != null;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void NotEquals_operator_WHERE_cards_are_same_value_in_memory_SHOULD_return_false()
		{
			//arrange
			var card = new Card(CardValue.Jack, CardSuit.Spades);

			//act
			var actual = card != card;

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void NotEquals_operator_WHERE_values_are_different_SHOULD_return_true()
		{
			//arrange
			var card1 = new Card(CardValue.Five, CardSuit.Spades);
			var card2 = new Card(CardValue.Nine, CardSuit.Spades);

			//act
			var actual = card1 != card2;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void NotEquals_operator_WHERE_suits_are_different_SHOULD_return_true()
		{
			//arrange
			var card1 = new Card(CardValue.Four, CardSuit.Clubs);
			var card2 = new Card(CardValue.Four, CardSuit.Diamonds);

			//act
			var actual = card1 != card2;

			//assert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void NotEquals_operator_WHERE_values_and_suits_are_the_same_SHOULD_return_false()
		{
			//arrange
			var card1 = new Card(CardValue.Ten, CardSuit.Spades);
			var card2 = new Card(CardValue.Ten, CardSuit.Spades);

			//act
			var actual = card1 != card2;

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion
	}
}
