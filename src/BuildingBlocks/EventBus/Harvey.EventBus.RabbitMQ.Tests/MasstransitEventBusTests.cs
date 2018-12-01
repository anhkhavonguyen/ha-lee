using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Policies;
using Harvey.EventBus.RabbitMQ.Policies;
using Harvey.EventBus.RabbitMQ.Tests.MockModels;
using Harvey.TestBase;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Harvey.EventBus.RabbitMQ.Tests
{
    [TestClass]
    public class MasstransitEventBusTests : UnitTestsBase
    {
        private IEventBus _eventBus;
        private Mock<IServiceProvider> _mockServiceProvider;
        private Mock<ILogger<MasstransitPersistanceConnection>> _mockMasstransitPersistanceConnectionLogger;
        public override void OnTestInitialize()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockMasstransitPersistanceConnectionLogger = new Mock<ILogger<MasstransitPersistanceConnection>>();
            _eventBus = new MasstransitEventBus(
                new MasstransitPersistanceConnection(
                    new BusCreationRetrivalPolicy(),
                    _mockMasstransitPersistanceConnectionLogger.Object,
                    "rabbitmq://localhost", "guest", "guest"),
                _mockServiceProvider.Object);
        }

        [TestMethod]
        public async Task When_A_Client_Subcribe_To_Default_Publisher_Then_It_Receives_Message_After_Publishing()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);

            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _eventBus.AddSubcription<MockEvent, MockEventHandler>();

            _eventBus.Commit();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsTrue(eventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_Multiple_Clients_Subcribe_To_Default_Publisher_Then_They_Receive_Message_After_Publishing()
        {
            var domainEvent = new MockEvent();


            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);



            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var anotherMockLogger = new Mock<ILogger<AnotherMockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            var anotherEventHandler = new AnotherMockEventHandler(mockEventStore.Object, anotherMockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(AnotherMockEventHandler))))
                .Returns(anotherEventHandler);

            _eventBus.AddSubcription<MockEvent, MockEventHandler>();
            _eventBus.AddSubcription<MockEvent, AnotherMockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsTrue(eventHandler.HasExecuted);
            Assert.IsTrue(anotherEventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_A_Client_Subcribe_To_Specific_Publisher_Then_It_Receives_Message_After_Publishing()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);


            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockPublisher))))
                .Returns(new MockPublisher());

            _eventBus.AddSubcription<MockPublisher, MockEvent, MockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsTrue(eventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_Multiple_Clients_Subcribe_To_Specific_Publisher_Then_They_Receive_Message_After_Publishing()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);


            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var anotherMockLogger = new Mock<ILogger<AnotherMockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            var anotherEventHandler = new AnotherMockEventHandler(mockEventStore.Object, anotherMockLogger.Object);


            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(AnotherMockEventHandler))))
                .Returns(anotherEventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockPublisher))))
                .Returns(new MockPublisher());


            _eventBus.AddSubcription<MockPublisher, MockEvent, MockEventHandler>();
            _eventBus.AddSubcription<MockPublisher, MockEvent, AnotherMockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsTrue(eventHandler.HasExecuted);
            Assert.IsTrue(anotherEventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_Two_Clients_Subcribe_To_Different_Publisher_With_Same_Event_Then_They_Receive_Message_After_Publishing()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);


            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var anotherMockLogger = new Mock<ILogger<AnotherMockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            var anotherEventHandler = new AnotherMockEventHandler(mockEventStore.Object, anotherMockLogger.Object);


            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(AnotherMockEventHandler))))
                .Returns(anotherEventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockPublisher))))
                .Returns(new MockPublisher());


            _eventBus.AddSubcription<MockEvent, MockEventHandler>();
            _eventBus.AddSubcription<MockPublisher, MockEvent, AnotherMockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsTrue(eventHandler.HasExecuted);
            Assert.IsTrue(anotherEventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_A_Client_Subcribe_To_Default_Publisher_Then_It_Ignore_Duplicated_Messsage_After_Publishing()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(true);


            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _eventBus.AddSubcription<MockEvent, MockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsFalse(eventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_Idempotent_Is_Detected_Then_Handler_Is_Not_Executed()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(true);

            var mockLogger = new Mock<ILogger<MockEventHandler>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _eventBus.AddSubcription<MockEvent, MockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsFalse(eventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_Additional_Idempotent_Is_Detected_Then_Handler_Is_Not_Executed()
        {
            var domainEvent = new MockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);

            var mockLogger = new Mock<ILogger<MockEventHandlerWithAdditionalIDempodentPolicy>>();

            var eventHandler = new MockEventHandlerWithAdditionalIDempodentPolicy(mockEventStore.Object, mockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _eventBus.AddSubcription<MockEvent, MockEventHandler>();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsFalse(eventHandler.HasExecuted);
        }

        [TestMethod]
        public async Task When_Endpoint_Is_Ready_Then_It_Can_Handle_Multiple_Events()
        {
            var domainEvent = new MockEvent();
            var anotherDomainEvent = new AnotherMockEvent();

            var mockEventStore = new Mock<IEventStore>();
            mockEventStore
                .Setup(x => x.Existed<EventBase>(It.IsAny<Guid>()))
                .Returns(false);

            var mockLogger = new Mock<ILogger<MockEventHandler>>();
            var anotherMockLogger = new Mock<ILogger<AnotherMockEventHandler2>>();

            var eventHandler = new MockEventHandler(mockEventStore.Object, mockLogger.Object);
            var anotherEventHandler = new AnotherMockEventHandler2(mockEventStore.Object, anotherMockLogger.Object);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEvent))))
                .Returns(domainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(AnotherMockEvent))))
                .Returns(anotherDomainEvent);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(MockEventHandler))))
                .Returns(eventHandler);

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(type => type == typeof(AnotherMockEventHandler2))))
                .Returns(anotherEventHandler);

            _eventBus.AddSubcription<MockEvent, MockEventHandler>();
            _eventBus.AddSubcription<AnotherMockEvent, AnotherMockEventHandler2>();
            _eventBus.Commit();

            var mockEventBus = new Mock<IEventBus>();

            mockEventBus.Setup(x => x.PublishAsync(It.IsAny<MockEvent>()))
                .Returns(Task.Run(() =>
                {
                    _eventBus.PublishAsync(domainEvent);
                    _eventBus.PublishAsync(anotherDomainEvent);
                    WaitForSeconds(2);
                }));

            await mockEventBus.Object.PublishAsync(domainEvent);
            Assert.IsTrue(eventHandler.HasExecuted);
            Assert.IsTrue(anotherEventHandler.HasExecuted);
        }

        public override void OnTestCleanUp()
        {
            base.OnTestCleanUp();
        }
    }
}
