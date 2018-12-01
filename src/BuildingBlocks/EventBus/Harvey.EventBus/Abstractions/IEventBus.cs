using Harvey.Domain;
using System;
using System.Threading.Tasks;

namespace Harvey.EventBus.Abstractions
{
    public interface IEventBus
    {
        Func<Guid> AuthorIdResolver { get; set; }

        IEventBus AddSubcription<TEvent, TEventHandler>(Guid? correlationId = null)
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>;

        IEventBus AddSubcription<TEvent, TEventHandler>(string publisher, Guid? correlationId = null)
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>;

        IEventBus AddSubcription<TPublisher, TEvent, TEventHandler>(Guid? correlationId = null)
            where TPublisher : IPublisher, new()
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>;
        void Commit();

        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : EventBase;
    }
}
