//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using Harvey.Search.Abstractions;
//using Harvey.TestBase;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Harvey.Search.NEST.Tests
//{
//    public class MockSearchItem : SearchItem, ISearchItem
//    {

//        public MockSearchItem()
//        {

//        }

//        public MockSearchItem(Guid id) : base(id)
//        {
//        }

//        public string Name { get; set; }

//        public string Title { get; set; }
//    }

//    public class MockIndexedItem : IndexedItem<MockSearchItem>
//    {
//        public MockIndexedItem(MockSearchItem item) : base(item)
//        {
//        }

//        public override string IndexName => "search_service_test";
//    }

//    public class MockSearchQuery : SearchQuery<MockSearchItem>, ISearchQuery<MockSearchItem>
//    {
//        public override string IndexName => "search_service_test";

//        public override List<Expression<Func<MockSearchItem, object>>> Matches => new List<Expression<Func<MockSearchItem, object>>>()
//        {
//            x=>x.Name,
//            x=>x.Title
//        };
//    }

//    [TestClass]
//    public class SearchServiceTests : UnitTestsBase
//    {
//        private SearchSettings _settings = new SearchSettings()
//        {
//            Url = "http://localhost:9200"
//        };
//        private SearchService _searchService;
//        public override void OnTestInitialize()
//        {
//            base.OnTestInitialize();
//            _searchService = new SearchService(_settings);
//        }

//        [TestMethod]
//        public async Task When_Add_Document_Then_Document_Is_Created()
//        {
//            var document = new MockIndexedItem(new MockSearchItem(Guid.NewGuid())
//            {
//                Name = "Minh",
//                Title = "Harvey"
//            });
//            await _searchService.AddAsync(document);
//            var result = await _searchService.WithElasticClient(x =>
//           {
//               var doc = x.GetAsync<MockSearchItem>(document.Item.Id, idx => idx.Index(document.IndexName)).Result;
//               return Task.FromResult(doc);
//           });

//            Assert.IsNotNull(result);
//            Assert.AreEqual(document.Item.Id, result.Source.Id);
//        }


//        [TestMethod]
//        public async Task When_Update_Document_Then_Document_Is_Updated()
//        {
//            var document = new MockIndexedItem(new MockSearchItem(Guid.Parse("abdc5254-d41b-4ec2-a6dd-575c9e7fbc6e"))
//            {
//                Name = "Test Name Updated"
//            });
//            await _searchService.UpdateAsync(document);
//            var result = await _searchService.WithElasticClient(x =>
//            {
//                var doc = x.GetAsync<MockSearchItem>(document.Item.Id, idx => idx.Index(document.IndexName)).Result;
//                return Task.FromResult(doc);
//            });

//            Assert.IsNotNull(result);
//            Assert.AreEqual(document.Item.Id, result.Source.Id);
//        }

//        [TestMethod]
//        public async Task When_Delete_Document_Then_Document_Is_Deleted()
//        {
//            var document = new MockIndexedItem(new MockSearchItem(Guid.Parse("abdc5254-d41b-4ec2-a6dd-575c9e7fbc6e"))
//            {
//                Name = "Test Name Updated"
//            });
//            await _searchService.DeleteAsync(document);
//            //var result = await _searchService.WithElasticClient(x =>
//            //{
//            //    var doc = x.GetAsync<MockSearchItem>(document.Item.Id, idx => idx.Index(document.IndexName)).Result;
//            //    return Task.FromResult(doc);
//            //});

//            //Assert.IsNotNull(result);
//            //Assert.AreEqual(document.Item.Id, result.Source.Id);
//        }

//        [TestMethod]
//        public async Task Can_Search()
//        {
//            var result = await _searchService.FuzzySearchAsync(new MockSearchQuery()
//            {
//                QueryText = "mih",
//                NumberItemsPerPage = 10,
//                Page = 1
//            });
//        }


//        public override void OnTestCleanUp()
//        {
//            base.OnTestCleanUp();
//        }
//    }
//}
