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
            var builder = new UrlBuilder("http://localhost:9000");
            var url = builder.BuildUrl(null);
            Assert.AreEqual("http://localhost:9000", url);
        }

        [TestMethod]
        public void Object_Params()
        {
            var builder = new UrlBuilder("/wak/:userId/:orderId");
            var url = builder.BuildUrl(new { userId = 1, orderId = 2 });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Dictionary_Params()
        {
            var builder = new UrlBuilder("/wak/:userId/:orderId");
            var url = builder.BuildUrl(new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Null_One_Params()
        {
            var builder = new UrlBuilder("/wak/:userId");
            var url = builder.BuildUrl(null);
            Assert.AreEqual("/wak/", url);
        }

        [TestMethod]
        public void Null_Two_Params()
        {
            var builder = new UrlBuilder("/wak/:userId/:orderId");
            var url = builder.BuildUrl(null);
            Assert.AreEqual("/wak/", url);
        }

        [TestMethod]
        public void Null_Two_Dotted_Params()
        {
            var builder = new UrlBuilder("/wak/:userId.foo/:orderId.bar");
            var url = builder.BuildUrl(null);
            Assert.AreEqual("/wak/.foo/.bar", url);
        }

        [TestMethod]
        public void Three_Parameters_With_Two_Arguments()
        {
            var builder = new UrlBuilder("/wak/:a/:b/:c");
            var url = builder.BuildUrl(new { a = 1, c = 3 });
            Assert.AreEqual("/wak/1/3", url);
        }

        [TestMethod]
        public void Double_Token()
        {
            var builder = new UrlBuilder("/wak/:a:b");
            var url = builder.BuildUrl(new { a = 1, b = 3 });
            Assert.AreEqual("/wak/13", url);
        }

        [TestMethod]
        public void Null_Url()
        {
            var builder = new UrlBuilder(null);
            var url = builder.BuildUrl(null);
            Assert.AreEqual("", url);
        }

        [TestMethod]
        public void Null_Url_With_Parameters()
        {
            var builder = new UrlBuilder(null);
            var url = builder.BuildUrl(new { userId = 1, orderId = 2 });
            Assert.AreEqual("?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Object_QueryString()
        {
            var builder = new UrlBuilder("/wak");
            var url = builder.BuildUrl(new { userId = 1, orderId = 2 });
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Dictionary_QueryString()
        {
            var builder = new UrlBuilder("/wak");
            var url = builder.BuildUrl(new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Object_Both()
        {
            var builder = new UrlBuilder("/wak/:orderId");
            var url = builder.BuildUrl(new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak/2?userId=1", url);
        }

        [TestMethod]
        public void Object_DefaultParams_Null()
        {
            var builder = new UrlBuilder("/wak", null);
            var url = builder.BuildUrl(new { userId = 1, orderId = 2 });
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }

        [TestMethod]
        public void Object_DefaultParams_Static()
        {
            var builder = new UrlBuilder("/wak/:userId/:orderId", new { orderId = 2 });
            var url = builder.BuildUrl(new { userId = 1 });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Object_DefaultParams_From_Object()
        {
            var builder = new UrlBuilder("/wak/:userId", new { userId = "@Id" });
            var url = builder.BuildUrl(new { Id = 1 });
            Assert.AreEqual("/wak/1", url);
        }

        [TestMethod]
        public void Object_DefaultParams_Dont_Override()
        {
            var builder = new UrlBuilder("/wak/:userId/:orderId", new { orderId = 3 });
            var url = builder.BuildUrl(new { userId = 1, orderId = 2 });
            Assert.AreEqual("/wak/1/2", url);
        }

        [TestMethod]
        public void Dictionary_DefaultParams_Null()
        {
            var builder = new UrlBuilder("/wak", null);
            var url = builder.BuildUrl(new Dictionary<string, object> { { "userId", 1 }, { "orderId", 2 } });
            Assert.AreEqual("/wak?userId=1&orderId=2", url);
        }
    }
}
