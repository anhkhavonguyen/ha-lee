using Harvey.EventBus.Abstractions;
using Harvey.Polly;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using System;

namespace Harvey.EventBus.RabbitMQ
{
    public class MasstransitPersistanceConnection : IPersitanceConnection<IRabbitMqHost, IBusControl>
    {
        public IRabbitMqHost Configurator { get; private set; }
        public IBusControl BusControl { get; private set; }
        public bool IsConnect => BusControl != null;
        private  string _endPoint;
        private readonly string _userName;
        private readonly string _password;
        private readonly IRetrivalPolicy _retrivalPolicy;
        private readonly ILogger<MasstransitPersistanceConnection> _logger;

        public MasstransitPersistanceConnection(IRetrivalPolicy retrivalPolicy, ILogger<MasstransitPersistanceConnection> logger, string endPoint, string userName = "guest", string password = "guest")
        {
            _retrivalPolicy = retrivalPolicy;
            _logger = logger;
            _endPoint = endPoint;
            _userName = userName;
            _password = password;

            Connect(_retrivalPolicy);
        }

        public void Connect(IRetrivalPolicy retrivalPolicy)
        {
            retrivalPolicy.ExecuteStrategyAsync(_logger, () =>
           {
               BusControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
               {
                   Configurator = cfg.Host(new Uri(_endPoint), h =>
                   {
                       h.Username(_userName);
                       h.Password(_password);
                       h.Heartbeat(10);

                   });
               });

               BusControl.Start();
           }).Wait();
        }
    }
}
