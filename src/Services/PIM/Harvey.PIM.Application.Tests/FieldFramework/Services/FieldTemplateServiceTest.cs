using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.TestBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Tests.FieldFramework.Services
{
    [TestClass]
    public class FieldTemplateServiceTest : UnitTestsBase
    {
        private Mock<IFieldTemplateService> _mockFieldTemplateService;
        public override void OnTestInitialize()
        {
            _mockFieldTemplateService = new Mock<IFieldTemplateService>();
        }

        [TestMethod]
        public void When_Adding_New_Field_Template_Then_Object_Has_Id()
        {
            var id = new Guid("fbd07b64-a1a4-415e-b8e2-4c19e320958c");

            var data = Task<FieldTemplateModel>.Factory.StartNew(() =>
            {
                return new FieldTemplateModel
                {
                    Id = new Guid("fbd07b64 - a1a4 - 415e-b8e2 - 4c19e320958c")
                };
            });

            _mockFieldTemplateService.Setup(x => x.SaveAsync(It.IsAny<FieldTemplateModel>()))
                .Returns(data);

            var result = _mockFieldTemplateService.Object.SaveAsync(new FieldTemplateModel());

            Assert.AreEqual(id.ToString(), result.Result.ToString());
        }

        [TestMethod]
        public void Get_Field_Template_By_Id()
        {
            var id = new Guid("fbd07b64-a1a4-415e-b8e2-4c19e320958c");

            var data = Task<FieldTemplateModel>.Factory.StartNew(() =>
            {
                return new FieldTemplateModel
                {
                    Id = new Guid("fbd07b64 - a1a4 - 415e-b8e2 - 4c19e320958c"),
                    Name = "TemplateProductA"
                };
            });

            _mockFieldTemplateService.Setup(x => x.GetAsync(id))
                .Returns(data);

            var result = _mockFieldTemplateService.Object.GetAsync(id);

            Assert.AreEqual(id, result.Result.Id.ToString());
            Assert.IsNotNull(data);
        }

        public override void OnTestCleanUp()
        {
            base.OnTestCleanUp();
        }
    }
}
