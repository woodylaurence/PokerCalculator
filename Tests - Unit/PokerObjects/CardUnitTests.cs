using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.TestData;
using System;

namespace PokerCalculator.Tests.Unit.PokerObjects
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

		[TestCase("")]
		[TestCase("   ")]
		[TestCase(null)]
		public void Constructor_string_WHERE_string_is_null_or_whitespace_SHOULD_throw_error(string cardAsString)
		{
			//todo this can probably be moved out of here
			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => new Card(cardAsString));
			Assert.That(actualException.Message, Is.EqualTo("Must provide string representation of Card"));
		}

		[TestCase("TD")]
		[TestCase("11S")]
		[TestCase("9G")]
		[TestCase("9Clubs")]
		[TestCase("0D")]
		[TestCase("asjhgfas")]
		public void Constructor_string_WHERE_string_does_not_conform_to_card_regex_SHOULD_throw_error(string input)
		{
			//act
			var actualException = Assert.Throws<ArgumentException>(() => new Card(input));
			Assert.That(actualException.Message, Is.EqualTo("Supplied string does not conform to allowed Card values"));
		}


		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsAsString))]
		public void Constructor_string(string cardAsString, CardValue expectedValue, CardSuit expectedSuit)
		{
			//act
			var actual = new Card(cardAsString);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsAsString))]
		public void Constructor_string_SHOULD_ignore_casing(string cardAsString, CardValue expectedValue, CardSuit expectedSuit)
		{
			//act
			var actual = new Card(cardAsString.ToLower());

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}

		#endregion
	}
}
