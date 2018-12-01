using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Categories;
using Harvey.EventBus.Events.FileldValues;
using Harvey.EventBus.Events.Prices;
using Harvey.EventBus.Events.Products;
using Harvey.EventBus.Events.Variants;
using Harvey.Persitance.EF;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Channels.Categories;
using Harvey.PIM.Application.Channels.FieldValues;
using Harvey.PIM.Application.Channels.Prices;
using Harvey.PIM.Application.Channels.Products;
using Harvey.PIM.Application.Channels.Variants;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Commands.Channels;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog.Models;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Provisions;
using Harvey.PIM.Application.Infrastructure.Queries.Channels;
using Harvey.PIM.MarketingAutomation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/channels")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class ChannelsController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IProvisionTask<DbProvisionTaskOption> _provisionTask;
        private readonly ApplicationBuilder _applicationBuilder;
        private readonly IEventBus _eventBus;
        private readonly IEfRepository<PimDbContext, Channel> _efRepository;
        public ChannelsController(IQueryExecutor queryExecutor,
                                  ICommandExecutor commandExecutor,
                                  IProvisionTask<DbProvisionTaskOption> provisionTask,
                                  ApplicationBuilder applicationBuilder,
                                  IEventBus eventBus,
                                  IEfRepository<PimDbContext, Channel> efRepository)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
            _provisionTask = provisionTask;
            _applicationBuilder = applicationBuilder;
            _eventBus = eventBus;
            _efRepository = efRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ChannelModel>>> Get(PagingFilterCriteria pagingFilterCriteria)
        {
            var result = await _queryExecutor.ExecuteAsync(new GetChannelsQuery(pagingFilterCriteria));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var result = await _queryExecutor.ExecuteAsync(new GetChannelByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult<AssortmentModel>> Add([FromBody] ChannelModel channel)
        {
            if (string.IsNullOrEmpty(channel.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new AddChannelCommand(User.GetUserId(), channel.Name, channel.Description, channel.ServerInformation));
            if (result != null && result.Id != Guid.Empty)
            {
                return CreatedAtAction("Add", result);
            }
            else
            {
                throw new InvalidOperationException("Cannot add channel. Please try again.");
            }
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(EfUnitOfWork))]
        public async Task<ActionResult> Update(Guid id, [FromBody] ChannelModel channel)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            if (string.IsNullOrEmpty(channel.Name))
            {
                return BadRequest("Name is required.");
            }
            var result = await _commandExecutor.ExecuteAsync(new UpdateChannelCommand(User.GetUserId(), channel.Id, channel.Name, channel.Description, channel.ServerInformation, channel.IsProvision));
            if (result != null)
            {
                return Ok();
            }
            else
            {
                throw new InvalidOperationException("Cannot update assortment. Please try again.");
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
            await _commandExecutor.ExecuteAsync(new DeleteChannelCommand(id));
            return Ok();
        }

        [HttpPost("{id}/provision")]
        public async Task<ActionResult> Provision(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id is required.");
            }

            var channel = await _queryExecutor.ExecuteAsync(new GetChannelByIdQuery(id));
            bool checkServerInformationExisted = await _commandExecutor.ExecuteAsync(new CheckServerInfomation(channel.Id, channel.ServerInformation));
            if (channel == null)
            {
                return NotFound("Channel is not presented.");
            }
            if (channel.IsProvision)
            {
                throw new InvalidOperationException("Channel has been provisioned.");
            }

            if (checkServerInformationExisted)
            {
                throw new InvalidOperationException("Channel Server Information has been created.");
            }

            try
            {
                var option = new DbProvisionTaskOption(channel.ServerInformation);
                var dbMigrationResult = await _provisionTask.ExecuteAsync(option);
                if (!dbMigrationResult)
                {
                    throw new InvalidOperationException("Cannot create database. Please check Server Infomation and try again.");
                    //TODO run rollback
                }
                else
                {

                }
                await _commandExecutor.ExecuteAsync(new UpdateChannelCommand(User.GetUserId(), channel.Id, channel.Name, channel.Description, channel.ServerInformation, true));
                _applicationBuilder.AddConnector(channel.Id, channel.Name, _eventBus, (connectorRegistration) =>
                {
                    connectorRegistration
                    .AddProductSyncService(productSyncServiceRegistration =>
                    {
                        productSyncServiceRegistration
                        .UseSyncHandler<MarketingAutomationEvent<ProductCreatedEvent>, ChannelProductCreatedEventHandler>()
                        .UseSyncHandler<MarketingAutomationEvent<ProductUpdatedEvent>, ChannelProductUpdatedEventHandler>();
                    })
                    .AddProductFeedService<ProductFeed, CatalogProductFeed>(productFeedServiceRegistration =>
                    {
                        productFeedServiceRegistration
                        .UseFetcher<ChannelProductFetcher>()
                        .UseFilter<ChannelProductFilter>()
                        .UseConverter<ChannelProductConveter>()
                        .UseSerializer<ChannelProductSerializer>()
                        .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                    })
                    .AddVariantSyncService(productSyncServiceRegistration =>
                    {
                        productSyncServiceRegistration
                        .UseSyncHandler<MarketingAutomationEvent<VariantCreatedEvent>, ChannelVariantCreatedEventHandler>();
                    })
                    .AddVariantFeedService<Variant, CatalogVariant>(productFeedServiceRegistration =>
                    {
                        productFeedServiceRegistration
                        .UseFetcher<ChannelVariantFetcher>()
                        .UseFilter<ChannelVariantFilter>()
                        .UseConverter<ChannelVariantConveter>()
                        .UseSerializer<ChannelVariantSerializer>()
                        .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                    })
                    .AddCategorySyncService(categorySyncServiceRegistration =>
                    {
                        categorySyncServiceRegistration
                        .UseSyncHandler<MarketingAutomationEvent<CategoryCreatedEvent>, ChannelCategoryCreatedEventHandler>()
                        .UseSyncHandler<MarketingAutomationEvent<CategoryUpdatedEvent>, ChannelCategoryUpdatedEventHandler>();
                    })
                    .AddCategoryFeedService<Category, CatalogCategory>(productFeedServiceRegistration =>
                     {
                         productFeedServiceRegistration
                         .UseFetcher<ChannelCategoryFetcher>()
                         .UseFilter<ChannelCategoryFilter>()
                         .UseConverter<ChannelCategoryConveter>()
                         .UseSerializer<ChannelCategorySerializer>()
                         .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                     })
                     .AddFieldValueSyncService(fieldValueSyncServiceRegistration =>
                     {
                         fieldValueSyncServiceRegistration
                         .UseSyncHandler<MarketingAutomationEvent<FieldValueCreatedEvent>, ChannelFieldValueCreatedEventHandler>()
                         .UseSyncHandler<MarketingAutomationEvent<FieldValueUpdatedEvent>, ChannelFieldValueUpdatedEventHandler>();
                     })
                     .AddPriceSyncService(priceSyncServiceRegistration =>
                     {
                         priceSyncServiceRegistration
                         .UseSyncHandler<MarketingAutomationEvent<PriceCreatedEvent>, ChannelPriceCreatedEventHandler>();
                     })
                        .AddPriceFeedService<Price, CatalogPrice>(priceFeedServiceRegistration =>
                        {
                            priceFeedServiceRegistration
                            .UseFetcher<ChannelPriceFetcher>()
                            .UseFilter<ChannelPriceFilter>()
                            .UseConverter<ChannelPriceConveter>()
                            .UseSerializer<ChannelPriceSerializer>()
                            .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                        });
                });
                _eventBus.Commit();
                return Ok();
            }
            catch
            {
                throw new InvalidOperationException("Cannot provision channel. Please try again.");
            }
        }

        [HttpGet("{channelId}/products")]
        public async Task<ActionResult<IEnumerable<CatalogProductModel>>> Products(Guid channelId)
        {
            if (channelId == Guid.Empty)
            {
                return BadRequest("id is required.");
            }
            var channel = await _efRepository.GetByIdAsync(channelId);
            if (channel == null)
            {
                return NotFound("channel is not presented.");
            }

            if (!channel.IsProvision)
            {
                return Ok();
            }
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql(channel.ServerInformation);
            var result = new List<CatalogProductModel>();
            using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
            {
                //TODO maybe cause performance issue
                var products = await dbContext.Products.ToListAsync();
                var variants = await dbContext.Variants.ToListAsync();
                var fieldValues = await dbContext.FieldValues.ToListAsync();
                var prices = await dbContext.Prices.ToListAsync();
                foreach (var item in products)
                {
                    var product = new CatalogProductModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description
                    };

                    foreach (var variant in variants.Where(x => x.ProductId == item.Id))
                    {
                        var price = prices.FirstOrDefault(x => x.Id == variant.PriceId);
                        var variantModel = new CatalogVariantModel()
                        {
                            Id = variant.Id,
                            Fields = fieldValues.Where(x => x.EntityId == variant.Id).Select(x => new CatalogFieldValueModel()
                            {
                                Name = x.FieldName,
                                Value = x.FieldValue
                            }).ToList()
                        };

                        if (price != null)
                        {
                            variantModel.Price = new CatalogPriceModel()
                            {
                                Id = price.Id,
                                ListPrice = price.ListPrice,
                                MemberPrice = price.MemberPrice,
                                StaffPrice = price.StaffPrice
                            };
                        }
                        product.Variants.Add(variantModel);
                    }
                    result.Add(product);
                }
            };

            return Ok(result);
        }
    }
}
