using System;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure.Commands.Brands;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/brands")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class BrandsController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;

        public BrandsController(IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BrandModel>>> Get(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetBrandsQuery(pagingFilterCriteria));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetBrandByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<BrandModel>> Add([FromBody] BrandModel brand)
        {
            if (string.IsNullOrEmpty(brand.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new AddBrandCommand(User.GetUserId(), brand.Name, brand.Description));
            if (result != null && result.Id != Guid.Empty)
            {
                return CreatedAtAction("Add", result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add brand. Please try again.");
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Update(Guid id, [FromBody] BrandModel brand)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new UpdateBrandCommand(User.GetUserId(), brand.Id, brand.Name, brand.Description));
            if (result != null)
            {
                return Ok();
            }
            else
            {
                throw new InvalidOperationException("Cannot update brand. Please try again.");
            }
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required");
            }
            await _commandExecutor.ExecuteAsync(new DeleteBrandCommand(id));
            return Ok();
        }
    }
}