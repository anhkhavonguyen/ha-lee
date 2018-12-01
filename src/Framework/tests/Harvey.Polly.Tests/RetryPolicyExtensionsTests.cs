using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.Polly.Tests
{
    [TestClass]
    public class RetryPolicyExtensionsTests
    {
        private Mock<IRetrivalPolicy> _mockRetrivalPolicy;
        private Mock<ILogger> _mockLogger;
        [TestInitialize]
        public void Setup()
        {
            _mockRetrivalPolicy = new Mock<IRetrivalPolicy>();
            _mockLogger = new Mock<ILogger>();
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task When_Exception_Was_Thrown_Then_Ignore_It()
        {
            _mockRetrivalPolicy.SetupGet(x => x.NumbersOfRetrival).Returns(1);
            _mockRetrivalPolicy.SetupGet(x => x.HandledExceptions).Returns(new List<Exception>() { new NotImplementedException() });
            _mockRetrivalPolicy.SetupGet(x => x.RetrivalStategy).Returns(RetrivalStategy.Immediate);
            _mockRetrivalPolicy.SetupGet(x => x.Delay).Returns(2);
            await _mockRetrivalPolicy.Object.ExecuteStrategyAsync(_mockLogger.Object, () => { throw new Exception(); });
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task When_Not_Implemented_Exception_Was_Thrown_Then_Handle_It()
        {
            _mockRetrivalPolicy.SetupGet(x => x.NumbersOfRetrival).Returns(1);
            _mockRetrivalPolicy.SetupGet(x => x.HandledExceptions).Returns(new List<Exception>() { new NotImplementedException() });
            _mockRetrivalPolicy.SetupGet(x => x.RetrivalStategy).Returns(RetrivalStategy.Immediate);
            _mockRetrivalPolicy.SetupGet(x => x.Delay).Returns(2);
            await _mockRetrivalPolicy.Object.ExecuteStrategyAsync(_mockLogger.Object, () => { throw new NotImplementedException(); });
        }
    }
}
