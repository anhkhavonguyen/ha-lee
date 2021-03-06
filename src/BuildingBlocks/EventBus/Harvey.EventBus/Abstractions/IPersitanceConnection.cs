﻿using Harvey.Polly;

namespace Harvey.EventBus.Abstractions
{
    public interface IPersitanceConnection<TBusConfigurator, TBusControl>
    {
        TBusConfigurator Configurator { get; }
        TBusControl BusControl { get; }
        bool IsConnect { get; }
        void Connect(IRetrivalPolicy retrivalPolicy);
    }
}
