using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Commands.Locations;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Locations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/locations")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class LocationsController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IEfRepository<PimDbContext, Location, LocationModel> _efRepository;
        public LocationsController(
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor,
            IEfRepository<PimDbContext, Location, LocationModel> efRepository)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
            _efRepository = efRepository;
        }


        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<LocationModel>>> GetAll()
        {
            return Ok(await _efRepository.GetAsync());
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<LocationModel>>> Get(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetLocationsQuery(pagingFilterCriteria));
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<LocationModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetLocationByIdQuery(id));
            return result;
        }

        [HttpGet]
        [Route("type/{type}")]
        public async Task<IEnumerable<LocationModel>> Get(LocationType type)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetLocationsByTypeQuery(type));
            return result;
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<LocationModel>> Create([FromBody]LocationModel locationModel)
        {
            if (string.IsNullOrEmpty(locationModel.Name))
            {
                return BadRequest("Name is required.");
            }
            if (string.IsNullOrEmpty(locationModel.Address))
            {
                return BadRequest("Address is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new AddLocationCommand(locationModel.Name, locationModel.Address, locationModel.Type, User.GetUserId()));
            if (result != null && result.Id != Guid.Empty)
            {
                return CreatedAtAction("Create", result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add location. Please try again.");
            }
        }

        [HttpPut]
        [ServiceFilter(typeof(EfUnitOfWork))]
        [Route("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody]LocationModel locationModel)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            if (string.IsNullOrEmpty(locationModel.Name))
            {
                return BadRequest("Name is required.");
            }
            if (string.IsNullOrEmpty(locationModel.Address))
            {
                return BadRequest("Address is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new UpdateLocationCommand(locationModel.Id, locationModel.Name, locationModel.Address, locationModel.Type, User.GetUserId()));
            if (result != null)
            {
                return Ok();
            }
            else
            {
                throw new InvalidOperationException("Cannot not update location. Please try again.");
            }
        }

        [HttpDelete]
        [ServiceFilter(typeof(EfUnitOfWork))]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new DeleteLocationCommand(id));
            return Ok();
        }
    }
}