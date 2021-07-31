﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestObjects;
using PokerCalculator.Tests.Unit.TestData;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PokerCalculator.Tests.Unit.PokerCalculator
{
	[TestFixture]
	public class PokerHandBasedHandRankPokerCalculatorUnitTests : AbstractUnitTestBase
	{
		private PokerHandBasedHandRankPokerCalculator _instance;
		private Deck _deck;
		private Mock<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>> _handRankCalculator;
		private CardComparer _cardComparer;
		private MethodInfo _executeCalculatePokerOddsForIterationMethod;

		[SetUp]
		protected override void Setup()
		{
			_handRankCalculator = new Mock<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>();
			_cardComparer = new CardComparer();

			base.Setup();

			_instance = new PokerHandBasedHandRankPokerCalculator(_handRankCalculator.Object);

			_executeCalculatePokerOddsForIterationMethod = _instance.GetType().GetMethod("ExecuteCalculatePokerOddsForIteration", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		protected override void RegisterServices(IServiceCollection services)
		{
			base.RegisterServices(services);

			services.AddSingleton<IRandomNumberGenerator, FakeRandomNumberGenerator>();
			services.AddSingleton<IEqualityComparer<Card>>(_cardComparer);
		}


		#region Instance Methods

		#region ExecuteCalculatePokerOddsForIteration

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_SHOULD_leave_supplied_deck_untouched()
		{
			//arrange
			_deck = new Deck();

			var myHandCard1 = _deck.TakeRandomCard();
			var myHandCard2 = _deck.TakeRandomCard();
			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = _deck.TakeRandomCard();
			var boardHandCard2 = _deck.TakeRandomCard();
			var boardHandCard3 = _deck.TakeRandomCard();
			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3 });

			_handRankCalculator.Setup(x => x.CalculateHandRank(It.IsAny<Hand>())).Returns(new PokerHandBasedHandRank(PokerHand.FourOfAKind));

			//act
			_executeCalculatePokerOddsForIterationMethod.Invoke(_instance, new object[] { _deck, myHand, boardHand, 3, new PokerOdds() });

			//assert
			Assert.That(_deck.Cards, Has.Count.EqualTo(47));

			var expectedDeckCards = CardTestCaseData.AllCards.Where(x => _cardComparer.Equals(x, myHandCard1) == false &&
																		 _cardComparer.Equals(x, myHandCard2) &&
																		 _cardComparer.Equals(x, boardHandCard1) &&
																		 _cardComparer.Equals(x, boardHandCard2) &&
																		 _cardComparer.Equals(x, boardHandCard3)).ToList();
			expectedDeckCards.ForEach(x => Assert.That(_deck.Cards.Contains(x, _cardComparer)));
		}

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_SHOULD_leave_supplied_hand_untouched()
		{
			//arrange
			_deck = new Deck();

			var myHandCard1 = _deck.TakeRandomCard();
			var myHandCard2 = _deck.TakeRandomCard();
			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = _deck.TakeRandomCard();
			var boardHandCard2 = _deck.TakeRandomCard();
			var boardHandCard3 = _deck.TakeRandomCard();
			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3 });

			_handRankCalculator.Setup(x => x.CalculateHandRank(It.IsAny<Hand>())).Returns(new PokerHandBasedHandRank(PokerHand.FourOfAKind));

			//act
			_executeCalculatePokerOddsForIterationMethod.Invoke(_instance, new object[] { _deck, myHand, boardHand, 3, new PokerOdds() });

			//assert
			Assert.That(myHand.Cards, Has.Count.EqualTo(2));
			Assert.That(myHand.Cards, Has.One.EqualTo(myHandCard1).Using(_cardComparer));
			Assert.That(myHand.Cards, Has.One.EqualTo(myHandCard2).Using(_cardComparer));
		}

		[Test]
		public void ExecuteCalculatePokerOddsForIteration_SHOULD_leave_supplied_board_hand_untouched()
		{
			//arrange
			_deck = new Deck();

			var myHandCard1 = _deck.TakeRandomCard();
			var myHandCard2 = _deck.TakeRandomCard();
			var myHand = new Hand(new List<Card> { myHandCard1, myHandCard2 });

			var boardHandCard1 = _deck.TakeRandomCard();
			var boardHandCard2 = _deck.TakeRandomCard();
			var boardHandCard3 = _deck.TakeRandomCard();
			var boardHand = new Hand(new List<Card> { boardHandCard1, boardHandCard2, boardHandCard3 });

			_handRankCalculator.Setup(x => x.CalculateHandRank(It.IsAny<Hand>())).Returns(new PokerHandBasedHandRank(PokerHand.FourOfAKind));

			//act
			_executeCalculatePokerOddsForIterationMethod.Invoke(_instance, new object[] { _deck, myHand, boardHand, 3, new PokerOdds() });

			//assert
			Assert.That(boardHand.Cards, Has.Count.EqualTo(3));
			Assert.That(boardHand.Cards, Has.One.EqualTo(boardHandCard1).Using(_cardComparer));
			Assert.That(boardHand.Cards, Has.One.EqualTo(boardHandCard2).Using(_cardComparer));
			Assert.That(boardHand.Cards, Has.One.EqualTo(boardHandCard3).Using(_cardComparer));
		}

		#endregion

		#endregion
	}
}
