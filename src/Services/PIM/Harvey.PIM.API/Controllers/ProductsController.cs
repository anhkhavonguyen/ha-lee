using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Commands.Products;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Indexing;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Products;
using Harvey.Search.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Harvey.PIM.Application.Infrastructure.Models.AddProductModel;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/products")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;
        private readonly PimDbContext _pimDbContext;
        private readonly ISearchService _searchService;
        private readonly IEfRepository<PimDbContext, Product, ProductListModel> _efRepository;
        public ProductsController(
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor,
            IMapper mapper,
            PimDbContext pimDbContext,
            ISearchService searchService,
            IEfRepository<PimDbContext, Product, ProductListModel> efRepository)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
            _mapper = mapper;
            _pimDbContext = pimDbContext;
            _searchService = searchService;
            _efRepository = efRepository;
        }

        [Route("all")]
        public async Task<ActionResult<IEnumerable<ProductListModel>>> GetAll()
        {
            return Ok(await _efRepository.GetAsync());
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductListModel>>> GetAll(PagingFilterCriteria pagingFilterCriteria, string queryText)
        {
            return await _queryExecutor.ExecuteAsync(new GetProductListQuery(pagingFilterCriteria, queryText));
        }

        [HttpGet]
        [Route("template/{id}")]
        public async Task<ActionResult<ProductModel>> GetFromTemplate(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetProductFromTemplateIdQuery(id));
            return result;
        }

        [HttpGet()]
        [Route("{id}")]
        public async Task<ActionResult<ProductModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetProductByIdQuery(id));
            return result;
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<ProductListModel>> Add([FromBody]AddProductModel productModel)
        {
            if (productModel == null)
            {
                return BadRequest("Model is required.");
            }
            if(productModel.CategoryId==Guid.Empty)
            {
                return BadRequest("Category is required.");
            }
            if (string.IsNullOrEmpty(productModel.Name))
            {
                throw new ArgumentException("Name is required.");
            }
            if (string.IsNullOrEmpty(productModel.Description))
            {
                throw new ArgumentException("Description is required.");
            }
            if (productModel.FieldTemplateId == Guid.Empty)
            {
                throw new ArgumentException("Product requires a field template.");
            }
            var queryProduct = _pimDbContext
                .Products
                .Where(x => x.Name.ToLower() == productModel.Name.Trim().ToLower()).FirstOrDefault();
            if (queryProduct != null)
            {
                throw new ArgumentException("Name is duplicate!");
            }

            var fields = await _pimDbContext
               .Field_FieldTemplates
               .Include(x => x.Field)
               .Where(x => x.FieldTemplateId == productModel.FieldTemplateId).ToListAsync();

            var fieldIds = fields.Select(x => x.FieldId);
            var productFieldIds = productModel
                .ProductFields
                .Select(x => x.FieldId)
                .Union(productModel.Variants.SelectMany(x => x.VariantFields).Select(x => x.FieldId));

            foreach (var item in productFieldIds)
            {
                if (!fieldIds.Any(x => item == x))
                {
                    return BadRequest("Product includes field(s) not in template");
                }
            }

            var productId = Guid.NewGuid();
            var addProductFieldValueModels = new List<DetailFieldValueModel>();
            var addVariantFieldValueModels = new Dictionary<Guid, List<DetailFieldValueModel>>();
            var addVariantPriceModel = new Dictionary<Guid, PriceModel>();
            foreach (var item in productModel.ProductFields)
            {
                var field = fields.Single(x => x.FieldId == item.FieldId);
                addProductFieldValueModels.Add(new DetailFieldValueModel(
                    productId,
                    field.Section,
                    field.OrderSection,
                    field.IsVariantField,
                    field.Field.Type,
                    item.FieldId,
                    field.Field.Name,
                    item.FieldValue)
                {
                    Id = Guid.NewGuid()
                });
            }

            var fieldValues = productModel.ProductFields.Select(p => !String.IsNullOrEmpty(p.FieldValue) ? p.FieldValue.ToString() : String.Empty).ToList();
            var variantFieldValues = new List<string>();

            if (productModel.Variants != null)
            {
                var variantFields = productModel.Variants.SelectMany(p => p.VariantFields).ToList();
                variantFieldValues = variantFields.Select(v => !String.IsNullOrEmpty(v.FieldValue) ? v.FieldValue.ToString() : String.Empty).ToList();

                foreach (var item in productModel.Variants)
                {
                    var variantId = Guid.NewGuid();
                    foreach (var variantField in item.VariantFields)
                    {
                        var field = fields.Single(x => x.FieldId == variantField.FieldId);
                        if (addVariantFieldValueModels.ContainsKey(variantId))
                        {
                            addVariantFieldValueModels[variantId].Add(
                                new DetailFieldValueModel(
                                   productId,
                                   field.Section,
                                   field.OrderSection,
                                   field.IsVariantField,
                                   field.Field.Type,
                                   variantField.FieldId,
                                   field.Field.Name,
                                   variantField.FieldValue)
                                {
                                    Id = Guid.NewGuid()
                                });

                            addVariantPriceModel[variantId] = item.Prices;
                        }
                        else
                        {
                            addVariantFieldValueModels.Add(
                              variantId,
                              new List<DetailFieldValueModel>() {
                                   new DetailFieldValueModel(
                                   productId,
                                   field.Section,
                                   field.OrderSection,
                                   field.IsVariantField,
                                   field.Field.Type,
                                   variantField.FieldId,
                                   field.Field.Name,
                                   variantField.FieldValue)
                                   {
                                       Id = Guid.NewGuid()
                                   }
                              });
                            addVariantPriceModel.Add(variantId, item.Prices);
                        }
                    }
                }
            }

            fieldValues = fieldValues.Concat(variantFieldValues).ToList();
            var indexingValue = string.Join(",", fieldValues);

            var product = await _commandExecutor.ExecuteAsync(
                new AddProductCommand(
                    productId,
                    productModel.CategoryId,
                    productModel.Name,
                    productModel.Description,
                    productModel.FieldTemplateId,
                    addProductFieldValueModels,
                    addVariantFieldValueModels,
                    addVariantPriceModel,
                    indexingValue
                    ));

            return _mapper.Map<ProductListModel>(product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProductModel productModel)
        {
            if (productModel.CategoryId == Guid.Empty)
            {
                return BadRequest("Category is required.");
            }

            if (string.IsNullOrEmpty(productModel.Name))
            {
                throw new ArgumentException("Name is required.");
            }
            if (string.IsNullOrEmpty(productModel.Description))
            {
                throw new ArgumentException("Description is required.");
            }
            if (productModel.FieldTemplateId == Guid.Empty)
            {
                throw new ArgumentException("Product requires a field template.");
            }

            var fields = await _pimDbContext
               .Field_FieldTemplates
               .Include(x => x.Field)
               .Where(x => x.FieldTemplateId == productModel.FieldTemplateId).ToListAsync();

            var fieldIds = fields.Select(x => x.FieldId);
            var productFieldIds = productModel
                .ProductFields
                .Select(x => x.FieldId)
                .Union(productModel.Variants.SelectMany(x => x.VariantFields).Select(x => x.FieldId));

            foreach (var item in productFieldIds)
            {
                if (!fieldIds.Any(x => item == x))
                {
                    return BadRequest("Product includes field(s) not in template");
                }
            }

            var productId = productModel.Id;
            var productFieldValueModels = new List<DetailFieldValueModel>();
            var variantFieldValueModels = new Dictionary<AddVariantModel, List<DetailFieldValueModel>>();
            var variantPriceModel = new Dictionary<Guid, PriceModel>();
            foreach (var item in productModel.ProductFields)
            {
                var field = fields.Single(x => x.FieldId == item.FieldId);
                productFieldValueModels.Add(new DetailFieldValueModel(
                    productId,
                    field.Section,
                    field.OrderSection,
                    field.IsVariantField,
                    field.Field.Type,
                    item.FieldId,
                    field.Field.Name,
                    item.FieldValue)
                {
                    Id = item.Id
                });
            }

            var fieldValues = productModel.ProductFields.Select(p => !String.IsNullOrEmpty(p.FieldValue) ? p.FieldValue.ToString() : String.Empty).ToList();
            var variantFieldValues = new List<string>();

            if (productModel.Variants != null)
            {
                var variantFields = productModel.Variants.SelectMany(p => p.VariantFields).ToList();
                variantFieldValues = variantFields.Select(v => !String.IsNullOrEmpty(v.FieldValue) ? v.FieldValue.ToString() : String.Empty).ToList();

                foreach (var item in productModel.Variants)
                {
                    var variantId = Guid.NewGuid();
                    if (item.Id != Guid.Empty)
                    {
                        variantId = item.Id;
                    }
                    foreach (var variantField in item.VariantFields)
                    {
                        var field = fields.Single(x => x.FieldId == variantField.FieldId);
                        var variantModelKeyValuePair = variantFieldValueModels.FirstOrDefault(x => x.Key.Id == variantId);
                        AddVariantModel variantModel = null;
                        foreach (var vm in variantFieldValueModels)
                        {
                            if (vm.Key.Id == variantId)
                            {
                                variantModel = vm.Key;
                                break;
                            }
                        }
                        if (variantModel == null)
                        {
                            variantModel = new AddVariantModel()
                            {
                                Id = variantId,
                                Action = variantId != item.Id ? ItemActionType.Add : ItemActionType.Update
                            };
                        }

                        if (variantFieldValueModels.ContainsKey(variantModel))
                        {
                            variantFieldValueModels[variantModel].Add(
                                new DetailFieldValueModel(
                                   productId,
                                   field.Section,
                                   field.OrderSection,
                                   field.IsVariantField,
                                   field.Field.Type,
                                   variantField.FieldId,
                                   field.Field.Name,
                                   variantField.FieldValue)
                                {
                                    Id = variantField.Id
                                });

                            variantPriceModel[variantId] = item.Prices;
                        }
                        else
                        {
                            variantFieldValueModels.Add(
                              variantModel,
                              new List<DetailFieldValueModel>() {
                                   new DetailFieldValueModel(
                                   productId,
                                   field.Section,
                                   field.OrderSection,
                                   field.IsVariantField,
                                   field.Field.Type,
                                   variantField.FieldId,
                                   field.Field.Name,
                                   variantField.FieldValue)
                                   {
                                       Id = variantField.Id
                                   }
                              });
                            variantPriceModel.Add(variantId, item.Prices);
                        }
                    }
                }
            }
            fieldValues = fieldValues.Concat(variantFieldValues).ToList();
            var indexingValue = string.Join(",", fieldValues);

            var product = await _commandExecutor.ExecuteAsync(
                new UpdateProductCommand(
                    productId,
                    productModel.CategoryId,
                    productModel.Name,
                    productModel.Description,
                    productFieldValueModels,
                    variantFieldValueModels,
                    variantPriceModel,
                    indexingValue
                    ));

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("searchItems")]
        public async Task<ActionResult<ProductSearchResults>> Search(string searchText)
        {
            var productSearchQuery = new ProductSearchQuery()
            {
                QueryText = searchText,
                NumberItemsPerPage = 10,
                Page = 1
            };
            var result = await _searchService.SearchAsync<ProductSearchItem, ProductSearchResult>(productSearchQuery);

            return Ok(result);
        }

        [HttpPost]
        [Route("rebuildIndex")]
        public async Task<ActionResult> RebuildIndex()
        {
            var productIndex = "product_index";
            await _searchService.DeleteByQueryAsync<ProductSearchItem>(productIndex);

            var products =  await _queryExecutor.ExecuteAsync(new GetProductListWithoutPagingQuery());
            var productSearchIndexedItems = new List<Search.IndexedItem<ProductSearchItem>>();
            var productSearchIndexedItem = new ProductSearchIndexedItem(new ProductSearchItem(Guid.NewGuid()));

            var items = products.Select(x => new ProductSearchIndexedItem(new ProductSearchItem(Guid.NewGuid())
            {
                Name = x.Name,
                Description = x.Description
            }));

            productSearchIndexedItems.AddRange(items);

            await _searchService.InsertDocumentsAsync<ProductSearchItem>(productSearchIndexedItems);
            return Ok();
        }
    }
}
