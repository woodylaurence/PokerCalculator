﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using PokerCalculator.Domain.HandRankCalculator;
using PokerCalculator.Domain.HandRankCalculator.PokerHandBased;
using PokerCalculator.Domain.PokerCalculator;
using PokerCalculator.Domain.PokerCalculator.PokerHandBased;
using PokerCalculator.Domain.PokerEnums;

namespace PokerCalculator.Tests.Speed.PokerCalculator
{
	[TestFixture]
	public class PokerCalculatorWithPokerHandBasedHandRankCalculatorSpeedTests : BasePokerCalculatorSpeedTests
	{
		protected override void RegisterComponentsToWindsor(IWindsorContainer windsorContainer)
		{
			base.RegisterComponentsToWindsor(windsorContainer);
			windsorContainer.Register(Component.For<IHandRankCalculator<PokerHandBasedHandRank, PokerHand>>().ImplementedBy<PokerHandBasedHandRankCalculator>());
			windsorContainer.Register(Component.For<IPokerCalculator>().ImplementedBy<PokerHandBasedHandRankPokerCalculator>());
		}
	}
}
