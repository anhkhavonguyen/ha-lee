using System;
using System.Collections.Generic;
using System.Linq;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation;

namespace Harvey.PIM.Application.Channels.Products
{
    public class ChannelProductFilter : IFeedFilter<ProductFeed>
    {
        private readonly IAssignmentService _assignmentService;
        public ChannelProductFilter(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        public IEnumerable<ProductFeed> Filter(Guid correlationId, IEnumerable<ProductFeed> source)
        {
            List<Guid> productIds = _assignmentService.GetAssignmentBy(AssortmentAssignmentType.Product, correlationId);
            var result = source.Where(x => productIds.Contains(x.Id)).ToList();
            return result;
        }
    }
}
