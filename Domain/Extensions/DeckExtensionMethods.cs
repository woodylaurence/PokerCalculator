using PokerCalculator.Domain.PokerObjects;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Domain.Extensions
{
	public static class DeckExtensionMethods
	{
		public static List<Hand> GenerateAllPossible5CardHands(this Deck deck)
		{
			if (deck.Cards.Count != 52) throw new ArgumentException("Deck does not contain all cards");
			var hands = new List<Hand>(2598960);

			for (var i = 0; i < 48; i++)
			{
				for (var j = i + 1; j < 49; j++)
				{
					for (var k = j + 1; k < 50; k++)
					{
						for (var l = k + 1; l < 51; l++)
						{
							for (var m = l + 1; m < 52; m++)
							{
								hands.Add(new Hand(new List<Card>
								{
									deck.Cards[i],
									deck.Cards[j],
									deck.Cards[k],
									deck.Cards[l],
									deck.Cards[m]
								}));
							}
						}
					}
				}
			}

			return hands;
		}
	}
}
