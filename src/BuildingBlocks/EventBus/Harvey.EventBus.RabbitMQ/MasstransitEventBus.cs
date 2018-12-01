using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Publishers;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.EventBus.RabbitMQ
{
    public class MasstransitEventBus : IEventBus
    {
        private class Subcription
        {
            public string Publisher { get; }
            public Guid? CorrelationId { get; }
            public List<Action<HostReceiveEndpointHandle>> ConnectHandlerConfigurator = new List<Action<HostReceiveEndpointHandle>>();
            public List<Action<IRabbitMqReceiveEndpointConfigurator>> HandlerConfigurator = new List<Action<IRabbitMqReceiveEndpointConfigurator>>();
            public HashSet<string> Handlers = new HashSet<string>();
            public Subcription(string publisher, Guid? correlationId = null)
            {
                Publisher = publisher;
                CorrelationId = correlationId;
            }
        };
        private Dictionary<string, HostReceiveEndpointHandle> _registeredEndPoint = new Dictionary<string, HostReceiveEndpointHandle>();
        private List<Subcription> _subcriptions = new List<Subcription>();
        private List<Subcription> _newSubcriptions = new List<Subcription>();
        private readonly IServiceProvider _serviceProvider;
        private readonly MasstransitPersistanceConnection _persistanceConnection;
        private readonly ILogger<MasstransitEventBus> _logger;

        public Func<Guid> AuthorIdResolver { get; set; }

        public MasstransitEventBus(
            MasstransitPersistanceConnection persitanceConnection,
            IServiceProvider serviceProvider
            )
        {
            _persistanceConnection = persitanceConnection;
            _serviceProvider = serviceProvider;
            _logger = (ILogger<MasstransitEventBus>)_serviceProvider.GetService(typeof(ILogger<MasstransitEventBus>));
        }

        public IEventBus AddSubcription<TEvent, TEventHandler>(Guid? correlationId = null)
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>
        {
            AddSubcriptionInternal<TEvent, TEventHandler>(new DefaultPublisher()
            {
                CorrelationId = correlationId
            });
            return this;
        }

        public IEventBus AddSubcription<TEvent, TEventHandler>(string publisher, Guid? correlationId = null)
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>
        {
            AddSubcriptionInternal<TEvent, TEventHandler>(new DefaultPublisher(publisher)
            {
                CorrelationId = correlationId
            });
            return this;
        }

        public IEventBus AddSubcription<TPublisher, TEvent, TEventHandler>(Guid? correlationId = null)
            where TPublisher : IPublisher, new()
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>
        {
            var publisher = new TPublisher
            {
                CorrelationId = correlationId
            };
            AddSubcriptionInternal<TEvent, TEventHandler>(publisher);
            return this;
        }

        private void AddSubcriptionInternal<TEvent, TEventHandler>(IPublisher publisher)
             where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>
        {
            if (!_subcriptions.Any(x => x.Publisher == publisher.Name && x.CorrelationId == publisher.CorrelationId))
            {
                var subcription = new Subcription(publisher.Name, publisher.CorrelationId);
                _subcriptions.Add(subcription);
                _newSubcriptions.Add(subcription);
                ConfigSubcription<TEvent, TEventHandler>(subcription);
            }
            else
            {
                var newSubcription = _newSubcriptions.FirstOrDefault(x => x.Publisher == publisher.Name && x.CorrelationId == publisher.CorrelationId);
                if (newSubcription != null)
                {
                    ConfigSubcription<TEvent, TEventHandler>(newSubcription);
                }

            }
        }

        public void Commit()
        {
            foreach (var subcription in _newSubcriptions)
            {
                var handle = _persistanceConnection.Configurator.ConnectReceiveEndpoint(subcription.Publisher, configure =>
                {
                    foreach (var configurator in subcription.HandlerConfigurator)
                    {
                        configurator(configure);
                    }
                });

                foreach (var configurator in subcription.ConnectHandlerConfigurator)
                {
                    configurator(handle);
                }
            }
            _newSubcriptions.Clear();
        }

        public Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : EventBase
        {
            @event.CreatedBy = AuthorIdResolver == null ? default(Guid) : AuthorIdResolver();
            @event.CreatedDate = DateTime.Now;
            _logger.LogTrace($"[EvenBus] [Publish {@event.GetType().Name}] {JsonConvert.SerializeObject(@event)}");
            return _persistanceConnection.BusControl.Publish(@event);
        }

        private void ConfigSubcription<TEvent, TEventHandler>(Subcription subcription)
             where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>
        {
            if (!subcription.Handlers.Any(x => x == typeof(TEvent).Name))
            {
                subcription.HandlerConfigurator.Add((configure) =>
                {
                    configure.Handler<TEvent>(async context =>
                    {
                        if (context.Message.CorrelationId.HasValue)
                        {
                            if (!subcription.CorrelationId.HasValue || subcription.CorrelationId.Value != context.Message.CorrelationId.Value)
                            {
                                return;
                            }
                        }
                        var handler = (EventHandlerBase<TEvent>)_serviceProvider.GetService(typeof(TEventHandler));
                        if (handler == null)
                        {
                            throw new ArgumentException(nameof(EventHandlerBase<TEvent>));
                        }
                        await handler.Handle(context.Message);
                        return;
                    });
                });
            }
            else
            {
                subcription.ConnectHandlerConfigurator.Add((handle) =>
                {
                    handle.ReceiveEndpoint.ConnectHandler<TEvent>(async context =>
                    {
                        if (context.Message.CorrelationId.HasValue)
                        {
                            if (!subcription.CorrelationId.HasValue || subcription.CorrelationId.Value != context.Message.CorrelationId.Value)
                            {
                                return;
                            }
                        }
                        var handler = (EventHandlerBase<TEvent>)_serviceProvider.GetService(typeof(TEventHandler));
                        if (handler == null)
                        {
                            throw new ArgumentException(nameof(EventHandlerBase<TEvent>));
                        }
                        await handler.Handle(context.Message);

                    });
                });
            }
        }
    }
}
