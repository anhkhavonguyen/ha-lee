using Harvey.Domain;
using Harvey.Logging;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/activities")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IEfRepository<ActivityLogDbContext, ActivityLog> _efRepository;
        public ActivitiesController(IEfRepository<ActivityLogDbContext, ActivityLog> efRepository)
        {
            _efRepository = efRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<PagedResult<ActivityLog>>> GetAll(Guid userId, PagingFilterCriteria pagingFilterCriteria)
        {
            var data = await _efRepository.ListAsync(x => x.UserId == userId, pagingFilterCriteria.Page, pagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _efRepository.Count(x => x.UserId == userId);
            return Ok(new PagedResult<ActivityLog>()
            {
                CurrentPage = pagingFilterCriteria.Page,
                NumberItemsPerPage = pagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = data
            });
        }
    }
}
