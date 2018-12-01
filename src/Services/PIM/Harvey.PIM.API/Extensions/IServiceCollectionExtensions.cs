using System;
using System.Collections.Generic;
using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.EventStore.Marten;
using Harvey.EventBus.RabbitMQ;
using Harvey.EventBus.RabbitMQ.Policies;
using Harvey.Logging;
using Harvey.Logging.SeriLog;
using Harvey.Persitance.EF;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.EventHandlers;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Implementation;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Commands.Brands;
using Harvey.PIM.Application.Infrastructure.Commands.Assortments;
using Harvey.PIM.Application.Infrastructure.Commands.Categories;
using Harvey.PIM.Application.Infrastructure.Commands.Locations;
using Harvey.PIM.Application.Infrastructure.Commands.Fields;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.Application.Infrastructure.Queries.Brands;
using Harvey.PIM.Application.Infrastructure.Queries.Assortments;
using Harvey.PIM.Application.Infrastructure.Queries.Categories;
using Harvey.PIM.Application.Infrastructure.Queries.Locations;
using Harvey.PIM.Application.Infrastructure.Queries.Fields;
using Harvey.PIM.Application.Services;
using Harvey.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Harvey.Exception.Handlers;
using Harvey.PIM.Application.Infrastructure.Queries.Products;
using Harvey.PIM.Application.Infrastructure.Commands.Products;
using Harvey.PIM.Application.Events.Products;
using Harvey.EventBus;
using Harvey.PIM.Application.Infrastructure.Queries.Channels;
using Harvey.PIM.Application.Infrastructure.Commands.Channels;
using Harvey.PIM.Application.Infrastructure.Provisions;
using Harvey.PIM.MarketingAutomation;
using Harvey.PIM.MarketingAutomation.Connectors;
using Harvey.PIM.Application.Infrastructure.Commands.AssortmentAssignments;
using Harvey.PIM.Application.Infrastructure.Queries.Assignments;
using Harvey.PIM.Application.Infrastructure.Commands.Assignments;
using Harvey.Job;
using Harvey.Job.Hangfire;
using Hangfire;
using Harvey.PIM.Application.Channels;
using Harvey.PIM.Application.Channels.Products;
using Hangfire.PostgreSql;
using Harvey.PIM.Application.EventHandlers.Products;
using Harvey.PIM.Application.Channels.Variants;
using Harvey.Search.Abstractions;
using Harvey.Search.NEST;
using Harvey.PIM.Application.Channels.Categories;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.EventHandlers.FieldValues;
using Harvey.PIM.Application.Channels.FieldValues;
using Harvey.PIM.MarketingAutomation.EventHandlers;
using Harvey.PIM.Application.EventHandlers.Variants;
using Harvey.PIM.Application.Channels.Prices;

namespace Harvey.PIM.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEventStore>(sp =>
            {
                return new MartenEventStore(configuration["ConnectionString"]);
            });
            services.AddTransient<LoggingEventHandler>();
            services.AddTransient<ProductCreatedEventHandler>();
            services.AddTransient<ProductUpdatedEventHandler>();
            services.AddTransient<ProductFieldValueCreatedEventHandler>();
            services.AddTransient<ProductFieldValueUpdatedEventHandler>();
            services.AddTransient<VariantCreatedEventHandler>();
            services.AddTransient<PriceCreatedEventHandler>();

            services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<MasstransitPersistanceConnection>>();
                var endPoint = configuration["EventBusConnection"];
                return new MasstransitPersistanceConnection(new BusCreationRetrivalPolicy(), logger, endPoint);
            });
            services.AddSingleton<IEventBus, MasstransitEventBus>();
            return services;
        }

        public static IServiceCollection AddServiceDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PimDbContext>(options =>
            {
                var connectionString = configuration["ConnectionString"];
                options.UseNpgsql(connectionString);
            });

            services.AddDbContext<TransientPimDbContext>(options =>
            {
                var connectionString = configuration["ConnectionString"];
                options.UseNpgsql(connectionString);
            }, ServiceLifetime.Transient);

            services.AddDbContext<ActivityLogDbContext>(options =>
            {
                var connectionString = configuration["ConnectionString"];
                options.UseNpgsql(connectionString);
            }, ServiceLifetime.Transient);

            services.AddDbContext<TransactionDbContext>(options =>
            {
                var connectionString = configuration["ConnectionString"];
                options.UseNpgsql(connectionString);
            });
            return services;
        }

        public static IServiceCollection AddCQRS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICommandExecutor, CommandExecutor>();
            services.AddTransient<IQueryExecutor, QueryExecutor>();

            services.AddTransient<ICommandHandler<AddCategoryCommand, CategoryModel>, AddCategoryCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateCategoryCommand, CategoryModel>, UpdateCategoryCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteCategoryCommand, bool>, DeleteCategoryCommandHandler>();
            services.AddTransient<IQueryHandler<GetCategoriesQuery, PagedResult<CategoryModel>>, GetCategoriesQueryHandler>();
            services.AddTransient<IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryModel>>, GetAllCategoriesQueryHandler>();
            services.AddTransient<IQueryHandler<GetCategoryByIdQuery, CategoryModel>, GetCategoryByIdQueryHandler>();

            services.AddTransient<IQueryHandler<GetFieldByIdQuery, FieldModel>, GetFieldByIdQueryHandler>();
            services.AddTransient<IQueryHandler<GetFieldsQuery, PagedResult<FieldModel>>, GetFieldsQueryHandler>();
            services.AddTransient<IQueryHandler<GetAllFieldsQuery, IEnumerable<FieldModel>>, GetAllFieldsQueryHandler>();
            services.AddTransient<ICommandHandler<AddFieldCommand, FieldModel>, AddFieldCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateFieldCommand, FieldModel>, UpdateFieldCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteFieldCommand, bool>, DeleteFieldCommandHandler>();
            services.AddTransient<IQueryHandler<GetFieldByIdQuery, FieldModel>, GetFieldByIdQueryHandler>();
            services.AddTransient<ICommandHandler<AddAssortmentCommand, AssortmentModel>, AddAssortmentCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateAssortmentCommand, AssortmentModel>, UpdateAssortmentCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteAssortmentCommand, bool>, DeleteAssortmentCommandHandler>();
            services.AddTransient<IQueryHandler<GetAssortmentsQuery, PagedResult<AssortmentModel>>, GetAssortmentsQueryHandler>();
            services.AddTransient<IQueryHandler<GetAssortmentByIdQuery, AssortmentModel>, GetAssortmentByIdQueryHandler>();

            services.AddTransient<ICommandHandler<AddBrandCommand, BrandModel>, AddBrandCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateBrandCommand, BrandModel>, UpdateBrandCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteBrandCommand, bool>, DeleteBrandCommandHandler>();
            services.AddTransient<IQueryHandler<GetBrandsQuery, PagedResult<BrandModel>>, GetBrandsQueryHandler>();
            services.AddTransient<IQueryHandler<GetBrandByIdQuery, BrandModel>, GetBrandByIdQueryHandler>();


            services.AddTransient<IQueryHandler<GetLocationsQuery, PagedResult<LocationModel>>, GetLocationsQueryHandler>();
            services.AddTransient<IQueryHandler<GetLocationByIdQuery, LocationModel>, GetLocationByIdQueryHandler>();
            services.AddTransient<ICommandHandler<AddLocationCommand, LocationModel>, AddLocationCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateLocationCommand, LocationModel>, UpdateLocationCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteLocationCommand, LocationModel>, DeleteLocationCommandHandler>();

            services.AddTransient<IQueryHandler<GetProductFromTemplateIdQuery, ProductModel>, GetProductFromTemplateIdQueryHandler>();
            services.AddTransient<IQueryHandler<GetProductListQuery, PagedResult<ProductListModel>>, GetProductListQueryHandler>();
            services.AddTransient<IQueryHandler<GetProductListWithoutPagingQuery, IEnumerable<ProductListModel>>, GetProductListWithoutPagingQueryHandler>();
            services.AddTransient<IQueryHandler<GetProductByIdQuery, ProductModel>, GetProductByIdQueryHandler>();
            services.AddTransient<ICommandHandler<AddProductCommand, Product>, AddProductCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>();

            services.AddTransient<IQueryHandler<GetChannelsQuery, PagedResult<ChannelModel>>, GetChannelsQueryHandler>();
            services.AddTransient<IQueryHandler<GetChannelByIdQuery, ChannelModel>, GetChannelByIdQueryHandler>();
            services.AddTransient<ICommandHandler<AddChannelCommand, ChannelModel>, AddChannelCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateChannelCommand, ChannelModel>, UpdateChannelCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteChannelCommand, bool>, DeleteChannelCommandHandler>();
            services.AddTransient<ICommandHandler<CheckServerInfomation, bool>, CheckServerInfomationHandler>();

            services.AddTransient<IQueryHandler<GetAssortmentAssignmentByName, List<AssortmentAssignmentModel>>, GetAssortmentAssignmentByNameHandler>();
            services.AddTransient<IQueryHandler<GetAssortmentAssignmentSelected, List<AssortmentAssignmentModel>>, GetAssortmentAssignmentSelectedHandler>();
            services.AddTransient<IQueryHandler<GetAllAssortmentAssignment, List<AssortmentAssignmentModel>>, GetAllAssortmentAssignmentHandler>();
            services.AddTransient<ICommandHandler<AddAssortmentAssignmentCommand, bool>, AddAssignmentCommandHandler>();

            services.AddTransient<IQueryHandler<GetChannelAssignmentByName, List<ChannelAssignmentModel>>, GetChannelAssignmentByNameHandler>();
            services.AddTransient<IQueryHandler<GetChannelAssignmentSelected, List<ChannelAssignmentModel>>, GetChannelAssignmentSelectedHandler>();
            services.AddTransient<IQueryHandler<GetAllChannelAssignment, List<ChannelAssignmentModel>>, GetAllChannelAssignmentHandler>();
            services.AddTransient<ICommandHandler<AddChannelAssignmentCommand, bool>, AddChannelAssignmentCommandHandler>();

            services.AddTransient<IQueryHandler<GetLocationsByTypeQuery, IEnumerable<LocationModel>>, GetLocationByTypeQueryHandler>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<EfUnitOfWork>();
            services.AddScoped<IEfRepository<PimDbContext, Category, CategoryModel>, EfRepository<PimDbContext, Category, CategoryModel>>();
            services.AddScoped<IEfRepository<PimDbContext, Category>, EfRepository<PimDbContext, Category>>();

            services.AddScoped<IEfRepository<PimDbContext, Field>, EfRepository<PimDbContext, Field>>();
            services.AddScoped<IEfRepository<PimDbContext, Field, FieldModel>, EfRepository<PimDbContext, Field, FieldModel>>();
            services.AddScoped<IEfRepository<PimDbContext, Assortment>, EfRepository<PimDbContext, Assortment>>();
            services.AddScoped<IEfRepository<PimDbContext, Assortment, AssortmentModel>, EfRepository<PimDbContext, Assortment, AssortmentModel>>();

            services.AddScoped<IEfRepository<PimDbContext, Brand>, EfRepository<PimDbContext, Brand>>();
            services.AddScoped<IEfRepository<PimDbContext, Brand, BrandModel>, EfRepository<PimDbContext, Brand, BrandModel>>();

            services.AddScoped<IEfRepository<PimDbContext, Location, LocationModel>, EfRepository<PimDbContext, Location, LocationModel>>();
            services.AddScoped<IEfRepository<PimDbContext, Location>, EfRepository<PimDbContext, Location>>();

            services.AddScoped<IEventStoreRepository<Product>, EvenStoreRepository<Product>>();
            services.AddScoped<IEfRepository<PimDbContext, Product>, EfRepository<PimDbContext, Product>>();
            services.AddTransient<IEfRepository<TransientPimDbContext, Product>, EfRepository<TransientPimDbContext, Product>>();
            services.AddScoped<IEfRepository<PimDbContext, Product, ProductListModel>, EfRepository<PimDbContext, Product, ProductListModel>>();
            services.AddScoped<IEfRepository<PimDbContext, FieldTemplate>, EfRepository<PimDbContext, FieldTemplate>>();
            services.AddScoped<IEfRepository<PimDbContext, FieldValue>, EfRepository<PimDbContext, FieldValue>>();
            services.AddTransient<IEfRepository<TransientPimDbContext, FieldValue>, EfRepository<TransientPimDbContext, FieldValue>>();
            services.AddScoped<IEfRepository<PimDbContext, Variant>, EfRepository<PimDbContext, Variant>>();
            services.AddTransient<IEfRepository<TransientPimDbContext, Variant>, EfRepository<TransientPimDbContext, Variant>>();
            services.AddScoped<IEfRepository<PimDbContext, Price>, EfRepository<PimDbContext, Price>>();
            services.AddScoped<IEfRepository<PimDbContext, Price, PriceModel>, EfRepository<PimDbContext, Price, PriceModel>>();

            services.AddScoped<IEfRepository<PimDbContext, Channel, ChannelModel>, EfRepository<PimDbContext, Channel, ChannelModel>>();
            services.AddScoped<IEfRepository<PimDbContext, Channel>, EfRepository<PimDbContext, Channel>>();
            services.AddTransient<IEfRepository<TransientPimDbContext, Channel>, EfRepository<TransientPimDbContext, Channel>>();
            services.AddTransient<IEfRepository<TransientPimDbContext, Category>, EfRepository<TransientPimDbContext, Category>>();

            services.AddScoped<IEfRepository<PimDbContext, AssortmentAssignment>, EfRepository<PimDbContext, AssortmentAssignment>>();

            services.AddScoped<IEfRepository<PimDbContext, ChannelAssignment>, EfRepository<PimDbContext, ChannelAssignment>>();

            services.AddScoped<IEfRepository<TransactionDbContext, StockType>, EfRepository<TransactionDbContext, StockType>>();
            return services;
        }

        public static IServiceCollection AddTrackingActivity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ActivityTracking>();
            services.AddTransient<IEfRepository<ActivityLogDbContext, ActivityLog>, EfRepository<ActivityLogDbContext, ActivityLog>>();
            return services;
        }

        public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
        {
            new SeriLogger().Initilize(new List<IDatabaseLoggingConfiguration>()
            {
                new DatabaseLoggingConfiguration()
                {
                    ConnectionString = configuration["Logging:DataLogger:ConnectionString"],
                    TableName = configuration["Logging:DataLogger:TableName"],
                    LogLevel = (LogLevel)Enum.Parse( typeof(LogLevel), configuration["Logging:DataLogger:LogLevel"],true)
                }
            }, new List<ICentralizeLoggingConfiguration>() {
                new CentralizeLoggingConfiguration()
                {
                    Url = configuration["Logging:CentralizeLogger:Url"],
                    LogLevel = (LogLevel)Enum.Parse( typeof(LogLevel), configuration["Logging:CentralizeLogger:LogLevel"],true)
                }
            });
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper();
            return services;
        }

        public static IServiceCollection AddFieldFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFieldService, FieldService>();
            services.AddTransient<IFieldTemplateService, FieldTemplateService>();
            services.AddTransient<IFieldValueService, FieldValueService>();
            services.AddTransient<IEntityRefService, EntityRefService>();
            services.AddTransient<IEntityRefValueService<Brand>, BrandEntityRefValueService>();
            services.AddTransient<IFieldTemplateService, FieldTemplateService>();

            return services;
        }

        public static IServiceCollection AddAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAppSettingService, AppSettingService>();
            return services;
        }

        public static IServiceCollection AddAssignmentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAssignmentService, AssignmentService>();
            return services;
        }

        public static IServiceCollection AddProvisionTask(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProvisionTask<DbProvisionTaskOption>, DbProvisionTask>();
            return services;
        }

        public static IServiceCollection AddExceptionHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ArgumentExceptionHandler>();
            services.AddScoped<EfUniqueConstraintExceptionHandler>();
            services.AddScoped<SqlExceptionHandler>();
            services.AddScoped<ForBiddenExceptionHandler>();
            services.AddScoped<NotFoundExceptionHandler>();
            services.AddScoped<BadModelExceptionHandler>();
            return services;
        }

        public static IServiceCollection AddMarketingAutomation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<MarketingAutomationService>();
            services.AddSingleton<ApplicationBuilder>();
            services.AddSingleton<ConnectorInfoCollection>();
            services.AddTransient<ChannelConnectorInstaller>();
            services.AddSingleton<FeedWorker>();
            services.AddTransient<MarketingProductCreatedEventHandler>();
            services.AddTransient<MarketingProductUpdatedEventHandler>();
            services.AddTransient<MarketingCategoryCreatedEventHandler>();
            services.AddTransient<MarketingCategoryUpdatedEventHandler>();
            services.AddTransient<MarketingVariantCreatedEventHandler>();
            services.AddTransient<MarketingFieldValueCreatedEventHandler>();
            services.AddTransient<MarketingFieldValueUpdatedEventHandler>();
            services.AddTransient<MarketingPriceCreatedEventHandler>();

            services.AddTransient<ChannelProductCreatedEventHandler>();
            services.AddTransient<ChannelProductUpdatedEventHandler>();
            services.AddTransient<ChannelProductFetcher>();
            services.AddTransient<ChannelProductFilter>();
            services.AddTransient<ChannelProductConveter>();
            services.AddTransient<ChannelProductSerializer>();

            services.AddTransient<ChannelVariantCreatedEventHandler>();
            services.AddTransient<ChannelVariantFetcher>();
            services.AddTransient<ChannelVariantFilter>();
            services.AddTransient<ChannelVariantConveter>();
            services.AddTransient<ChannelVariantSerializer>();

            services.AddTransient<ChannelCategoryCreatedEventHandler>();
            services.AddTransient<ChannelCategoryUpdatedEventHandler>();
            services.AddTransient<ChannelCategoryFetcher>();
            services.AddTransient<ChannelCategoryFilter>();
            services.AddTransient<ChannelCategoryConveter>();
            services.AddTransient<ChannelCategorySerializer>();

            services.AddTransient<ChannelFieldValueCreatedEventHandler>();
            services.AddTransient<ChannelFieldValueUpdatedEventHandler>();

            services.AddTransient<ChannelPriceCreatedEventHandler>();
            services.AddTransient<ChannelPriceFetcher>();
            services.AddTransient<ChannelPriceFilter>();
            services.AddTransient<ChannelPriceConveter>();
            services.AddTransient<ChannelPriceSerializer>();
            return services;
        }

        public static IServiceCollection AddJobManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJobManager, HangfireJobManager>();
            services.AddHangfire(options => options.UseStorage(new PostgreSqlStorage(configuration["ConnectionString"]))
            .UseColouredConsoleLogProvider());
            return services;
        }

        public static IServiceCollection AddSearchService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<SearchSettings>(sp =>
            {
                return new SearchSettings(configuration["ELASTICSEARCHURL"]);
            });
            return services;
        }
    }
}
