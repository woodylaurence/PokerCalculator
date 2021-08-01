using NUnit.Framework;
using PokerCalculator.App;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Unit.Domain.TestData;
using System;

namespace PokerCalculator.Tests.Unit.App
{
	[TestFixture]
	public class StringToCardParserUnitTests : AbstractUnitTestBase
	{
		private StringToCardParser _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = new StringToCardParser();
		}

		#region ParseCard

		[TestCase("")]
		[TestCase("   ")]
		[TestCase(null)]
		public void ParseCard_WHERE_string_is_null_or_whitespace_SHOULD_throw_error(string cardAsString)
		{
			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.ParseCard(cardAsString));
			Assert.That(actualException.Message, Is.EqualTo("Must provide string representation of Card"));
		}

		[TestCase("TD")]
		[TestCase("11S")]
		[TestCase("9G")]
		[TestCase("9Clubs")]
		[TestCase("0D")]
		[TestCase("asjhgfas")]
		public void ParseCard_WHERE_string_does_not_conform_to_card_regex_SHOULD_throw_error(string input)
		{
			//act
			var actualException = Assert.Throws<ArgumentException>(() => _instance.ParseCard(input));
			Assert.That(actualException.Message, Is.EqualTo("Supplied string does not conform to allowed Card values"));
		}


		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsAsString))]
		public void ParseCard(string cardAsString, CardValue expectedValue, CardSuit expectedSuit)
		{
			//act
			var actual = _instance.ParseCard(cardAsString);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsAsString))]
		public void ParseCard_SHOULD_ignore_casing(string cardAsString, CardValue expectedValue, CardSuit expectedSuit)
		{
			//act
			var actual = _instance.ParseCard(cardAsString.ToLower());

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedSuit));
		}

		#endregion
	}
}
