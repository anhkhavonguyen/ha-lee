using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Commands.Fields;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Harvey.PIM.API.Controllers
{
    [Route("api/fields")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class FieldsController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IEntityRefService _entityRefService;
        private readonly IFieldTemplateService _fieldTemplateService;
        private readonly IEfRepository<PimDbContext, FieldTemplate> _efRepository;
        public FieldsController(
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor,
            IEntityRefService entityRefService,
            IFieldTemplateService fieldTemplateService,
            IEfRepository<PimDbContext, FieldTemplate> efRepository
            )
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
            _entityRefService = entityRefService;
            _fieldTemplateService = fieldTemplateService;
            _efRepository = efRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<FieldModel>>> GetAll()
        {
            return Ok(await _queryExecutor.ExecuteAsync(new GetAllFieldsQuery()));
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<FieldModel>>> Get(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetFieldsQuery(pagingFilterCriteria));
            return Ok(result);
        }

        [HttpGet]
        [Route("ref")]
        public async Task<ActionResult<EntityRefModel>> GetEntityRef()

        {
            var result = await _entityRefService.GetAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FieldModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetFieldByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<FieldModel>> Add([FromBody] FieldModel field)
        {
            var command = new AddFieldCommand(User.GetUserId(), field.Name, field.Description, field.Type, field.DefaultValue);
            var result = await _commandExecutor.ExecuteAsync(command);
            if (result != null && result.Id != Guid.Empty)
            {
                return CreatedAtAction("Add", result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add field. Please try again.");
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Update(Guid id, [FromBody] FieldModel field)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var command = new UpdateFieldCommand(User.GetUserId(), field.Id, field.Name, field.Description, field.DefaultValue);
            var result = await _commandExecutor.ExecuteAsync(command);
            if (result != null && result.Id != Guid.Empty)
            {
                return Ok(result);
            }
            else
            {
                throw new InvalidOperationException("Cannot update field. Please try again.");
            }
        }

        [ServiceFilter(typeof(EfUnitOfWork))]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            await _commandExecutor.ExecuteAsync(new DeleteFieldCommand(id));
            return Ok();
        }

        /// <summary>
        /// Add field template
        /// </summary>
        /// <param name="fieldTemplateModel">field template model</param>
        /// <returns>Field template</returns>
        /// <response code="201">OK</response>
        /// <response code="400">Model is required</response>
        /// <response code="400">Name is required</response>
        /// <response code="400">Field template should contain at least one section</response>
        /// <response code="500">Template with variant requires variant field(s)</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("templates")]
        public async Task<ActionResult<FieldTemplateModel>> AddFieldTemplate([FromBody] FieldTemplateModel fieldTemplateModel)
        {
            if (fieldTemplateModel == null)
            {
                return BadRequest("Model is required");
            }
            if (string.IsNullOrEmpty(fieldTemplateModel.Name))
            {
                return BadRequest("Name is required");
            }
            if (fieldTemplateModel.Sections == null || !fieldTemplateModel.Sections.Any())
            {
                return BadRequest("Field template should contain at least one section");
            }

            if (fieldTemplateModel.Type == FieldTemplateType.WithVariant && !fieldTemplateModel.Sections.Any(x => x.IsVariantField))
            {
                throw new ArgumentException($"Template {FieldTemplateType.WithVariant.ToString()} requires variant section");
            }
            var result = await _fieldTemplateService.SaveAsync(fieldTemplateModel);
            return CreatedAtAction("Add", result);
        }

        /// <summary>
        /// Get field template by id
        /// </summary>
        /// <param name="id">Field template id</param>
        /// <returns>Field template</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Id is required</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet("templates/{id}")]
        public async Task<ActionResult<FieldTemplateModel>> GetFieldTemplate(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required");
            }
            var result = await _fieldTemplateService.GetAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Update field template
        /// </summary>
        /// <param name="id">field template id</param>
        /// <param name="fieldTemplateModel">field template model</param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Id is required</response>
        /// <response code="400">Model is required</response>
        /// <response code="400">Name is required</response>
        /// <response code="500">Template with variant requires variant field(s)</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPut("templates/{id}")]
        public async Task<ActionResult> UpdateFieldTemplate(Guid id, [FromBody] FieldTemplateModel fieldTemplateModel)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            if (fieldTemplateModel == null)
            {
                return BadRequest("Model is required.");
            }

            if (string.IsNullOrEmpty(fieldTemplateModel.Name))
            {
                return BadRequest("File template name is required.");
            }

            if (fieldTemplateModel.Type == FieldTemplateType.WithVariant && !fieldTemplateModel.Sections.Any(x => x.IsVariantField))
            {
                throw new ArgumentException($"Template {FieldTemplateType.WithVariant.ToString()} requires variant section");
            }

            await _fieldTemplateService.SaveAsync(fieldTemplateModel);

            return Ok();
        }

        [HttpGet("alltemplates")]
        public async Task<ActionResult<IEnumerable<FieldTemplate>>> List()
        {
            var result = await _efRepository.GetAsync();
            return Ok(result);
        }

        /// <summary>
        /// get all field templates
        /// </summary>
        /// <returns>List of field template</returns>
        /// <response code="200">Returns field templates</response>
        [ProducesResponseType(200)]
        [HttpGet("templates")]
        public async Task<ActionResult<PagedResult<FieldTemplate>>> GetAllAsync(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _fieldTemplateService.GetAllAsync(pagingFilterCriteria);
            return Ok(result);
        }

        /// <summary>
        /// get all field template types
        /// </summary>
        /// <returns>list of field template types</returns>
        /// <response code="200">Returns field template types</response>
        [ProducesResponseType(200)]
        [HttpGet("templates/types")]
        public async Task<ActionResult<IEnumerable<string>>> GetFileTemplateTypes()
        {
            await Task.Yield();
            return Enum.GetNames(typeof(FieldTemplateType)).ToList();
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpDelete("templates/{id}")]
        public async Task<ActionResult> DeleteFieldTemplate(Guid id)
        {
            await _fieldTemplateService.Delete(id);
            return Ok();
        }

    }
}