using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class UrlBuilderTests
    {
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
        public void Null_Params()
        {
            var builder = new UrlBuilder();
            var url = builder.BuildUrl("/wak/:userId/:orderId", null);
            Assert.AreEqual("/wak/:userId/:orderId", url);
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
    }
}
