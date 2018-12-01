using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.PostgreSQL;

namespace Harvey.Logging.SeriLog
{
    public class SeriLogger
    {
        public void Initilize(List<IDatabaseLoggingConfiguration> databaseLoggingConfigurations, List<ICentralizeLoggingConfiguration> centralizeLoggingConfigurations)
        {
            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
            };

            var configuration = new LoggerConfiguration()
                          .Enrich.WithMachineName();

            foreach (var item in databaseLoggingConfigurations)
            {
                configuration.WriteTo.PostgreSQL(
                    item.ConnectionString,
                    item.TableName,
                    columnOptions: columnWriters,
                    needAutoCreateTable: true,
                    restrictedToMinimumLevel: GetLogLevel(item.LogLevel))
                    .Filter.ByIncludingOnly(Matching.WithProperty<string>("SourceContext", p => p.StartsWith("Harvey")));
            }

            foreach (var item in centralizeLoggingConfigurations)
            {
                configuration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(item.Url))
                {
                    MinimumLogEventLevel = GetLogLevel(item.LogLevel),
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                });
            }
            Log.Logger = configuration.CreateLogger();
        }

        private LogEventLevel GetLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Trace:
                    return LogEventLevel.Verbose;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
