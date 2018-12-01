using Harvey.Domain;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure.Commands.Categories;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/categories")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class CategoriesController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        public CategoriesController(
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAll()
        {
            var result = await _queryExecutor.ExecuteAsync(new GetAllCategoriesQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CategoryModel>>> Get(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetCategoriesQuery(pagingFilterCriteria));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetCategoryByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<CategoryModel>> Add([FromBody] CategoryModel category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new AddCategoryCommand(User.GetUserId(), category.Name, category.Description));
            if (result != null && result.Id != Guid.Empty)
            {
                return CreatedAtAction("Add", result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add category. Please try again.");
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Update(Guid id, [FromBody] CategoryModel category)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new UpdateCategoryCommand(User.GetUserId(), category.Id, category.Name, category.Description));
            if (result != null)
            {
                return Ok();
            }
            else
            {
                throw new InvalidOperationException("Cannot not update category. Please try again.");
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
            await _commandExecutor.ExecuteAsync(new DeleteCategoryCommand(id));
            return Ok();
        }
    }
}