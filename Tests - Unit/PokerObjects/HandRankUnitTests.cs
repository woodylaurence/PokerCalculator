using NUnit.Framework;
using PokerCalculator.Domain.PokerObjects;
using Rhino.Mocks;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class HandRankUnitTests
	{
		HandRank _instance;

		[SetUp]
		public void Setup()
		{
			_instance = MockRepository.GeneratePartialMock<HandRank>();

			HandRank.MethodObject = MockRepository.GenerateStrictMock<HandRank>();
		}

		[TearDown]
		public void TearDown()
		{
			HandRank.MethodObject = new HandRank();
		}

		#region Create

		[Test]
		public void Create_WHERE_not_providing_kicker_cards_SHOULD_call_slave_with_null()
		{
			//arrange
			const PokerHand handRank = PokerHand.StraightFlush;

			var expected = MockRepository.GenerateStrictMock<HandRank>();
			HandRank.MethodObject.Expect(x => x.CreateSlave(handRank, null)).Return(expected);

			//act
			var actual = HandRank.Create(handRank);

			//assert
			HandRank.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Create_calls_slave()
		{
			//arrange
			const PokerHand handRank = PokerHand.StraightFlush;
			var kickers = new List<Card> { MockRepository.GenerateStrictMock<Card>() };

			var expected = MockRepository.GenerateStrictMock<HandRank>();
			HandRank.MethodObject.Expect(x => x.CreateSlave(handRank, kickers)).Return(expected);

			//act
			var actual = HandRank.Create(handRank, kickers);

			//assert
			HandRank.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CreateSlave_WHERE_null_kickers_provided_SHOULD_set_kickers_to_empty_list()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.Pair;

			//act
			var actual = _instance.CreateSlave(pokerHand, null);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCards, Is.Empty);
		}

		[Test]
		public void CreateSlave()
		{
			//arrange
			const PokerHand pokerHand = PokerHand.Pair;
			var kickers = new List<Card> { MockRepository.GenerateStrictMock<Card>() };

			//act
			var actual = _instance.CreateSlave(pokerHand, kickers);

			//assert
			Assert.That(actual.PokerHand, Is.EqualTo(pokerHand));
			Assert.That(actual.KickerCards, Is.EqualTo(kickers));
		}

		#endregion
	}
}
