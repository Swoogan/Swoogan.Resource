using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Swoogan.Resource.Url;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class UrlBuilderTests
    {
        [TestMethod]
        public void Basic_Url()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("http://localhost:9000", null);
            Assert.AreEqual("http://localhost:9000", url);
        }

        [TestMethod]
        public void Object_Params()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId/:orderId", new { userId = 1, orderId = 2 });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Dictionary_Params()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId/:orderId", new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Null_One_Params()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId", null);
            Assert.AreEqual("/wak/", url);
        }

        [TestMethod]
        public void Null_Two_Params()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId/:orderId", null);
            Assert.AreEqual("/wak/", url);
        }

        [TestMethod]
        public void Null_Two_Dotted_Params()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId.foo/:orderId.bar", null);
            Assert.AreEqual("/wak/.foo/.bar", url);
        }

        [TestMethod]
        public void Three_Parameters_With_Two_Arguments()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:a/:b/:c", new { a = 1, c = 3 });
            Assert.AreEqual("/wak/1/3", url);
        }

        [TestMethod]
        public void Double_Token()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:a:b", new { a = 1, b = 3 });
            Assert.AreEqual("/wak/13", url);
        }

        [TestMethod]
        public void Null_Url()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl(null, new { userId = 1, orderId = 2 });
            Assert.AreEqual("", url);
        }

        [TestMethod]
        public void Object_QueryString()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak", new { userId = 1, orderId = 2 });
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Dictionary_QueryString()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak", new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Object_Both()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:orderId", new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak/2?userId=1", url);
        }

        [TestMethod]
        public void Object_DefaultParams_Null()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak", new { userId = 1, orderId = 2 }, null);
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Object_DefaultParams_Static()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId/:orderId", new { userId = 1 }, new { orderId = 2 });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Object_DefaultParams_From_Object()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId", new { Id = 1 }, new { userId = "@Id" });
            Assert.AreEqual("/wak/1", url);
        }

        [TestMethod]
        public void Object_DefaultParams_Dont_Override()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId/:orderId", new { userId = 1, orderId = 2 }, new { orderId = 3 });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Dictionary_DefaultParams_Null()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak", new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } }, null);
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }
    }
}
