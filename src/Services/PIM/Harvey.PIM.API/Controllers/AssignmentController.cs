using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure.Commands.Assignments;
using Harvey.PIM.Application.Infrastructure.Commands.AssortmentAssignments;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Assignments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/assignments")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class AssignmentController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;
        public AssignmentController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        [HttpGet()]
        [Route("assortment")]
        public async Task<ActionResult<List<AssortmentAssignmentModel>>> GetAssignment(string text)
        {
            var allAssignemt = await _queryExecutor.ExecuteAsync(new GetAssortmentAssignmentByName(text));

            return allAssignemt;
        }

        [HttpGet()]
        [Route("assortments")]
        public async Task<ActionResult<List<AssortmentAssignmentModel>>> GetAllAssignment()
        {
            var allAssignemt = await _queryExecutor.ExecuteAsync(new GetAllAssortmentAssignment());

            return allAssignemt;
        }

        [HttpGet()]
        [Route("assortment/Id={id}")]
        public async Task<ActionResult<List<AssortmentAssignmentModel>>> GetAssignmentByAssortmentId(Guid id)
        {
            var allAssignemt = await _queryExecutor.ExecuteAsync(new GetAssortmentAssignmentSelected(id));

            return allAssignemt;
        }

        [HttpGet()]
        [Route("channel/Id={id}")]
        public async Task<ActionResult<List<ChannelAssignmentModel>>> GetAssignmentByChannelId(Guid id)
        {
            var allAssignemt = await _queryExecutor.ExecuteAsync(new GetChannelAssignmentSelected(id));

            return allAssignemt;
        }

        [HttpGet()]
        [Route("channel")]
        public async Task<ActionResult<List<ChannelAssignmentModel>>> GetChannelAssignment(string text)
        {
            var allAssignemt = await _queryExecutor.ExecuteAsync(new GetChannelAssignmentByName(text));

            return allAssignemt;
        }

        [HttpGet()]
        [Route("channels")]
        public async Task<ActionResult<List<ChannelAssignmentModel>>> GetAllChannel()
        {
            var allAssignemt = await _queryExecutor.ExecuteAsync(new GetAllChannelAssignment());

            return allAssignemt;
        }

        [HttpPost]
        [Route("assortment/{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> AddAssortmentAssignment([FromBody] List<AddAssortmentAssignmentModel> assignments, Guid id)
        {
            var result = await _commandExecutor.ExecuteAsync(new AddAssortmentAssignmentCommand(assignments, id));
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add assignment. Please try again.");
            }
        }

        [HttpPost]
        [Route("channel/{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> AddChannelAssignment([FromBody] List<AddChannelAssignmentModel> assignments, Guid id)
        {
            var result = await _commandExecutor.ExecuteAsync(new AddChannelAssignmentCommand(assignments, id));
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add assignment. Please try again.");
            }
        }
    }
}
