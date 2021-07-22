using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandIntegrationTests : LocalTestBase
	{
		private Hand _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = new Hand(new List<Card>());
		}

		#region Instance Methods

		#endregion

		#region Operator Overloads

		#region + Overload

		[Test]
		public void AdditionOverload_PlusEqualsCheck()
		{
			//arrange
			//todo should we keep this?
			var hand1Card1 = new Card(CardValue.Four, CardSuit.Hearts);
			var hand1Card2 = new Card(CardValue.Five, CardSuit.Diamonds);
			var hand1 = new Hand(new List<Card> { hand1Card1, hand1Card2 });

			var hand2Card1 = new Card(CardValue.King, CardSuit.Diamonds);
			var hand2Card2 = new Card(CardValue.Jack, CardSuit.Hearts);
			var hand2Card3 = new Card(CardValue.Seven, CardSuit.Hearts);
			var hand2 = new Hand(new List<Card> { hand2Card1, hand2Card2, hand2Card3 });

			//act
			var actual = hand1;
			actual += hand2;

			//assert
			Assert.That(actual, Is.Not.SameAs(hand2));
			Assert.That(actual, Is.Not.SameAs(hand1));

			Assert.That(actual.Cards, Has.Count.EqualTo(5));
			Assert.That(actual.Cards, Has.Some.EqualTo(hand1Card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(hand1Card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(hand2Card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(hand2Card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(hand2Card3));
		}

		#endregion

		#endregion
	}
}
