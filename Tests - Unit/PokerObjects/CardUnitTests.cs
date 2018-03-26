using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;
using Rhino.Mocks;
using System;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class CardUnitTests : AbstractUnitTestBase
	{
		[SetUp]
		protected override void Setup()
		{
			Utilities.MethodObject = MockRepository.GenerateStrictMock<Utilities>();
		}

		[TearDown]
		protected void TearDown()
		{
			Utilities.MethodObject = new Utilities();
		}

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

		[TestCase("4", "D")]
		[TestCase("10", "S")]
		[TestCase("a", "H")]
		[TestCase("K", "H")]
		[TestCase("j", "S")]
		[TestCase("Q", "C")]
		[TestCase("4", "C")]
		public void Constructor_string(string cardValueAsString, string cardSuitAsString)
		{
			//arrange
			var cardAsString = $"{cardValueAsString}{cardSuitAsString}";

			var cardValueAsStringUppercase = cardValueAsString.ToUpper();
			var cardSuitAsStringUppercase = cardSuitAsString.ToUpper();

			const CardValue expectedValue = CardValue.Jack;
			Utilities.MethodObject.Stub(x => x.GetEnumValueFromDescriptionSlave<CardValue>(cardValueAsStringUppercase)).Return(expectedValue);

			const CardSuit expectedSuit = CardSuit.Hearts;
			Utilities.MethodObject.Stub(x => x.GetEnumValueFromDescriptionSlave<CardSuit>(cardSuitAsStringUppercase)).Return(expectedSuit);

			//act
			var actual = new Card(cardAsString);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}


		#endregion
	}
}
