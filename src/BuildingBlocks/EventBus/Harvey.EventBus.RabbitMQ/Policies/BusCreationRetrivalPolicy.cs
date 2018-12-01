using System.Collections.Generic;
using System.Net.Sockets;
using Harvey.Polly;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client.Exceptions;

namespace Harvey.EventBus.RabbitMQ.Policies
{
    public class BusCreationRetrivalPolicy : IRetrivalPolicy
    {
        public int NumbersOfRetrival => 10;
        public RetrivalStategy RetrivalStategy => RetrivalStategy.Exponential;
        public int Delay => 2;
        public List<System.Exception> HandledExceptions => new List<System.Exception>() {
            new SocketException(),
            new BrokerUnreachableException(new System.Exception()),
            new RabbitMqConnectionException()};
    }
}
