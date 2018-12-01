using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.PIM.Application.Channels.Categories
{
    public class ChannelCategoryFilter : IFeedFilter<Category>
    {
        private readonly IAssignmentService _assignmentService;
        public ChannelCategoryFilter(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }
        public IEnumerable<Category> Filter(Guid correlationId, IEnumerable<Category> source)
        {
            List<Guid> categoryIds = _assignmentService.GetAssignmentBy(AssortmentAssignmentType.Category, correlationId);
            var result = source.Where(x => categoryIds.Contains(x.Id));
            return result;
        }
    }
}
