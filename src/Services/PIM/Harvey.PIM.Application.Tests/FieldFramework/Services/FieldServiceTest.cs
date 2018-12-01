using Harvey.Domain;
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
    public class FieldServiceTest : UnitTestsBase
    {
        private Mock<IFieldService> _mockFieldService;
        public override void OnTestInitialize()
        {
            _mockFieldService = new Mock<IFieldService>();
        }

        [TestMethod]
        public void When_Adding_New_Field_Then_Field_Has_Id()
        {
            var id = new Guid("59ffb1d7-fc7c-4390-a755-d775481fdad2");

            var data = Task<Guid>.Factory.StartNew(() =>
            {
                return new Guid("59ffb1d7-fc7c-4390-a755-d775481fdad2");
            });

            _mockFieldService.Setup(x => x.SaveAsync(It.IsAny<Field>()))
                .Returns(data);

            var result = _mockFieldService.Object.SaveAsync(new Field(FieldType.Text));

            Assert.AreEqual(id.ToString(), result.Result.ToString());
        }

        public override void OnTestCleanUp()
        {
            base.OnTestCleanUp();
        }
    }
}
