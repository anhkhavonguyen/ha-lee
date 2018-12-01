using Harvey.Domain;
using Harvey.EventBus.EventStore.Marten.Tests.MockModels;
using Harvey.TestBase;
using Marten;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.EventBus.EventStore.Marten.Tests
{
    [TestClass]
    public class MartenEventStoreTests : UnitTestsBase
    {
        private MartenEventStore _martenEventStore;
        private readonly string _connectionString = "Server=localhost;port=5432;Database=harvey_marten_event_store;UserId=postgres;Password=123456";
        public override void OnTestInitialize()
        {
            _martenEventStore = new MartenEventStore(_connectionString);
        }

        [TestMethod]
        public void When_Event_Has_Not_Appended_To_Store_Then_It_Is_Not_Existed()
        {
            var result = _martenEventStore.Existed<EventBase>(Guid.NewGuid());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task When_Append_New_Event_Then_It_Should_Be_Stored_At_Database()
        {
            var @event = new MockEvent("50001");
            var id = @event.Id;
            var executeResult = await _martenEventStore.AppendEventAsync(@event);
            Assert.IsTrue(executeResult);
            var result = _martenEventStore.Existed<MockEvent>(id);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task When_Append_New_Event_Then_We_Can_Get_It_From_Database()
        {
            var streamId = Guid.NewGuid().ToString();
            var @event = new MockEvent(streamId);
            var @event2 = new MockEvent(streamId);
            var executeResult = await _martenEventStore.AppendEventAsync(@event);
            var executeResult2 = await _martenEventStore.AppendEventAsync(@event2);
            Assert.IsTrue(executeResult);
            Assert.IsTrue(executeResult2);
            var results = await _martenEventStore.ReadEventsAsync(streamId);
            Assert.IsTrue(results.Count() == 2);
        }

        [TestMethod]
        public async Task When_Append_New_Event_Then_We_Can_Get_It_From_Database_It_Should_Order_By_Date_Decending()
        {
            var @event = new MockEvent("100");
            var @event2 = new MockEvent("100");
            var id = event2.Id;
            var executeResult = await _martenEventStore.AppendEventAsync(@event);
            var executeResult2 = await _martenEventStore.AppendEventAsync(@event2);
            Assert.IsTrue(executeResult);
            Assert.IsTrue(executeResult2);
            var results = await _martenEventStore.ReadEventsAsync("100");
            Assert.IsTrue(results.Last().Id == id);
        }

        [TestMethod]
        public async Task When_Append_New_Different_Events_Then_We_Can_Get_Correct_Event_Type()
        {
            var @event = new MockEvent("102");
            var @event2 = new AnotherMockEvent("102");
            var executeResult = await _martenEventStore.AppendEventAsync(@event);
            var executeResult2 = await _martenEventStore.AppendEventAsync(@event2);
            Assert.IsTrue(executeResult);
            Assert.IsTrue(executeResult2);
            var results = await _martenEventStore.ReadEventsAsync("102");
            Assert.IsInstanceOfType(results.Last(), typeof(AnotherMockEvent));
            Assert.IsInstanceOfType(results.First(), typeof(MockEvent));
        }

        public override void OnTestCleanUp()
        {
            //DocumentStore.For(cfg =>
            //{
            //    cfg.Connection(_connectionString);
            //    cfg.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            //}).Advanced.Clean.CompletelyRemoveAll();
        }
    }
}
