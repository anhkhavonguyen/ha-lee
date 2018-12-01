﻿using MassTransit;
using System.Threading.Tasks;
using System;
using Harvey.Message.Activities;

namespace Harvey.CRMLoyalty.Application.Services.Activity
{
    public class LoggingActivityService : ILoggingActivityService
    {
        private readonly IBusControl _bus;

        public LoggingActivityService(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task ExecuteAsync(LoggingActivityRequest request, string RabbitMqUrl)
        {
            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(RabbitMqUrl, "/", "logging_activity_queue")));

            await sendEndpointTask.Send<LoggingActivityCommand>(new
            {
                ActionAreaPath = request.ActionAreaPath,
                ActionType = request.ActionType,
                Description = request.Description,
                Comment = request.Comment,
                UserId = request.UserId,
                CreatedByName = request.CreatedByName,
                Value = request.Value
            });
        }
    }
}
