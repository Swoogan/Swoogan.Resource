using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class ParseUrlTests
    {
        [TestMethod]
        public void Good()
        {
            var resource = new Resource("http://localhost/");
            var result = resource.ParseUrl("/wak/{id}");
            var expected = new List<string> { "{id}" };
            CollectionAssert.AreEqual(expected, result);

        }

        [TestMethod]
        public void NoParams()
        {
            var resource = new Resource("http://localhost/");
            resource.ParseUrl("/wak");
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedUrlException))]
        public void NoClose()
        {
            var resource = new Resource("http://localhost/");
            resource.ParseUrl("/wak/{id");
        }
    }
}
