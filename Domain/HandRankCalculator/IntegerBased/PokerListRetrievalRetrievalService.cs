using PokerCalculator.Domain.Helpers;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Domain.HandRankCalculator.IntegerBased
{
	public class PokerListRetrievalService : IPokerListRetrievalService
	{
		private IEmbeddedResourceRetrievalService _embeddedResourceRetrievalService;
		protected internal virtual List<int> _flushesList { get; set; }
		public List<int> FlushesList { get { throw new NotImplementedException(); } }

		public PokerListRetrievalService(IEmbeddedResourceRetrievalService embeddedResourceRetrievalService)
		{
			_embeddedResourceRetrievalService = embeddedResourceRetrievalService;
			throw new NotImplementedException("Load backing field from embedded resource");
		}
	}
}
