using System;
using System.Collections.Generic;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.MarketingAutomation;

namespace Harvey.PIM.Application.Channels.Prices
{
    public class ChannelPriceFilter : IFeedFilter<Price>
    {
        private readonly IAssignmentService _assignmentService;
        public ChannelPriceFilter(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }
        public IEnumerable<Price> Filter(Guid correlationId, IEnumerable<Price> source)
        {
            return source;
        }
    }
}
