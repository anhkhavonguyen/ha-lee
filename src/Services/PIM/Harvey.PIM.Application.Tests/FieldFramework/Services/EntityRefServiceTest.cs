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
    public class EntityRefServiceTest : UnitTestsBase
    {
        private Mock<IEntityRefService> _mockEntityRefService;
        public override void OnTestInitialize()
        {
            _mockEntityRefService = new Mock<IEntityRefService>();
        }
        [TestMethod]
        public void Get_All_EntityRef()
        {
            var data = Task<IList<EntityRefModel>>.Factory.StartNew(() =>
            {
                return new List<EntityRefModel>
                {
                    new EntityRefModel
                    {
                        Id = new Guid("fbd07b64-a1a4-415e-b8e2-4c19e320958c"),
                        Name = "Product",
                        Namespace = "Domain"
                    },
                    new EntityRefModel
                    {
                        Id = new Guid("22c33c55-6992-41ee-871f-b7f12ca50b29"),
                        Name = "Variant",
                        Namespace = "Domain",
                    }
                };
            });

            _mockEntityRefService.Setup(x => x.GetAsync())
                .Returns(data);

            var result = _mockEntityRefService.Object.GetAsync();

            Assert.AreEqual(2, result.Result.Count());
            Assert.AreEqual(result.Result[0].Id.ToString(), "fbd07b64-a1a4-415e-b8e2-4c19e320958c");
            Assert.AreEqual(result.Result[1].Name.ToString(), "Variant");
            Assert.IsNotNull(data);
        }
        public override void OnTestCleanUp()
        {
            base.OnTestCleanUp();
        }
    }
}
