using Harvey.Extensions;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.PIM.Application.Channels.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly TransientPimDbContext _pimDbContext;
        public AssignmentService(TransientPimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }

        public List<Guid> GetAssignmentBy(AssortmentAssignmentType assortmentAssignmentType, Guid channelId)
        {
            var assormentIds = _pimDbContext
                                .ChannelAssignments
                                .AsNoTracking()
                                .Where(x => x.ChannelId == channelId && x.EntityType == ChannelAssignmentType.Assortment)
                                .Select(x => x.ReferenceId).ToArray();
            if (!assormentIds.Any())
            {
                return new List<Guid>();
            }

            var feedTypePredicate = PredicateBuilder.False<AssortmentAssignment>();
            var assortmentPredicate = PredicateBuilder.False<AssortmentAssignment>();
            foreach (var item in assormentIds)
            {
                assortmentPredicate = assortmentPredicate.Or(x => x.AssortmentId == item);
            }
            switch (assortmentAssignmentType)
            {
                case AssortmentAssignmentType.Category:
                    feedTypePredicate = feedTypePredicate.Or(x => x.EntityType == AssortmentAssignmentType.Category);
                    break;
                case AssortmentAssignmentType.Product:
                    feedTypePredicate = feedTypePredicate.Or(x => x.EntityType == AssortmentAssignmentType.Category);
                    feedTypePredicate = feedTypePredicate.Or(x => x.EntityType == AssortmentAssignmentType.Product);
                    break;
                default:
                    return new List<Guid>();
            }
            var predicate = feedTypePredicate.And(assortmentPredicate);
            return _pimDbContext
                .AssortmentAssignments
                .AsNoTracking()
                .Where(predicate).Select(x => x.ReferenceId).ToList();

        }

        public bool IsAssignment(AssortmentAssignmentType assortmentAssignmentType, Guid channelId, Guid referenceId)
        {
            var ids = GetAssignmentBy(assortmentAssignmentType, channelId);
            return ids.Contains(referenceId);
        }
    }
}
