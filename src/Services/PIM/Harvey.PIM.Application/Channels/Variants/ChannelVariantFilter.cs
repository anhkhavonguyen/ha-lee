using System;
using System.Collections.Generic;
using System.Linq;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation;

namespace Harvey.PIM.Application.Channels.Variants
{
    public class ChannelVariantFilter : IFeedFilter<Variant>
    {
        private readonly IAssignmentService _assignmentService;
        public ChannelVariantFilter(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }
        public IEnumerable<Variant> Filter(Guid correlationId, IEnumerable<Variant> source)
        {
            List<Guid> productIds = _assignmentService.GetAssignmentBy(AssortmentAssignmentType.Product, correlationId);
            var result = source.Where(x => productIds.Contains(x.ProductId)).ToList();
            return result;
        }
    }
}
