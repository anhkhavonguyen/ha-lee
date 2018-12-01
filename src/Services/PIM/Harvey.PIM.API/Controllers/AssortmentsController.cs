using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure.Commands.Assortments;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Assortments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/assortments")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class AssortmentsController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        public AssortmentsController(
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AssortmentModel>>> Get(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetAssortmentsQuery(pagingFilterCriteria));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssortmentModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetAssortmentByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<AssortmentModel>> Add([FromBody] AssortmentModel assortment)
        {
            if (string.IsNullOrEmpty(assortment.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new AddAssortmentCommand(User.GetUserId(), assortment.Name, assortment.Description));
            if (result != null && result.Id != Guid.Empty)
            {
                return CreatedAtAction("Add", result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add assortment. Please try again.");
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Update(Guid id, [FromBody] AssortmentModel assortment)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            if (string.IsNullOrEmpty(assortment.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new UpdateAssortmentCommand(User.GetUserId(), assortment.Id, assortment.Name, assortment.Description));
            if (result != null)
            {
                return Ok();
            }
            else
            {
                throw new InvalidOperationException("Cannot not update assortment. Please try again.");
            }
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            await _commandExecutor.ExecuteAsync(new DeleteAssortmentCommand(id));
            return Ok();
        }
    }
}