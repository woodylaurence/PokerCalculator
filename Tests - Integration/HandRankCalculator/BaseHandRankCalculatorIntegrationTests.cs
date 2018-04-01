using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PokerCalculator.Domain.Extensions;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using System.Linq;

namespace PokerCalculator.Tests.Integration.HandRankCalculator
{
	public abstract class BaseHandRankCalculatorIntegrationTests<THandRank, TRank> : LocalTestBase where THandRank : IHandRank<TRank>
	{
		protected IHandRankCalculator<THandRank, TRank> _instance;

		[SetUp]
		protected override void Setup()
		{
			base.Setup();
			_instance = ServiceLocator.Current.GetInstance<IHandRankCalculator<THandRank, TRank>>();
		}

		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IRandomNumberGenerator>().ImplementedBy<RandomNumberGenerator>());
		}

		[Test]
		public void HandRankCalculator_identifies_every_hand_correctly()
		{
			var deck = new Deck();
			var allHands = deck.GenerateAllPossible5CardHands();
			var pokerHandCounts = allHands.Select(x => _instance.CalculateHandRank(x))
										  .GroupBy(x => x.PokerHand)
										  .ToDictionary(x => x.Key, x => x.Count());

			Assert.That(pokerHandCounts[PokerHand.RoyalFlush], Is.EqualTo(4));
			Assert.That(pokerHandCounts[PokerHand.StraightFlush], Is.EqualTo(36));
			Assert.That(pokerHandCounts[PokerHand.FourOfAKind], Is.EqualTo(624));
			Assert.That(pokerHandCounts[PokerHand.FullHouse], Is.EqualTo(3744));
			Assert.That(pokerHandCounts[PokerHand.Flush], Is.EqualTo(5108));
			Assert.That(pokerHandCounts[PokerHand.Straight], Is.EqualTo(10200));
			Assert.That(pokerHandCounts[PokerHand.ThreeOfAKind], Is.EqualTo(54912));
			Assert.That(pokerHandCounts[PokerHand.TwoPair], Is.EqualTo(123552));
			Assert.That(pokerHandCounts[PokerHand.Pair], Is.EqualTo(1098240));
			Assert.That(pokerHandCounts[PokerHand.HighCard], Is.EqualTo(1302540));
		}
	}
}
