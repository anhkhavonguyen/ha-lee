using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.TestBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Tests.FieldFramework.Services
{
    [TestClass]
    public class FieldValueServiceTest : UnitTestsBase
    {
        private Mock<IFieldValueService> _mockFieldValueService;
        public override void OnTestInitialize()
        {
            _mockFieldValueService = new Mock<IFieldValueService>();
        }

        [TestMethod]
        public void When_Adding_New_Field_Value_Then_Object_Has_Id()
        {
            var id = new Guid("100afbe1-0c54-417f-9468-440f2af7148a");

            var data = Task<Guid>.Factory.StartNew(() =>
            {
                return new Guid("100afbe1-0c54-417f-9468-440f2af7148a");
            });

            var entityId = "04eb1d5d-a655-4394-9df5-92224f9c9882";
            var fieldId = "87da382c-37bc-4f85-96dc-dfec29a6020e";

            _mockFieldValueService.Setup(x => x.SaveAsync(entityId, fieldId, It.IsAny<FieldValue>()))
                .Returns(data);

            var result = _mockFieldValueService.Object.SaveAsync(entityId, fieldId, new FieldValue());

            Assert.AreEqual(id.ToString(), result.Result.ToString());
        }

        public override void OnTestCleanUp()
        {
            base.OnTestCleanUp();
        }
    }
}
