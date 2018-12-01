using Harvey.EventBus;

namespace Harvey.PIM.MarketingAutomation
{
    public interface IEventProcessor
    {
        bool CanProcess(EventBase @event);
        void Process();
    }
}
