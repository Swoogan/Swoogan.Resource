using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Moq;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class GetTests
    {
        [TestMethod]
        public void Typed_Get()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<Customer>>();
            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };

            response.Setup(r => r.Data).Returns(customer);
            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Get<Customer>();

            Assert.AreEqual(customer, result);
        }

        // TODO: Make the two tests below system agnositic

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void Get_BadUrl()
        {
            var res = new Resource("http://localhost:3456/wak");
            var result = res.Get<Customer>();
            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        [ExpectedException(typeof(GetException))]
        public void Get_NotAuthorized()
        {
            var res = new Resource("http://localhost/wak");
            var result = res.Get<Customer>();
            Assert.IsNotNull(result);
        }
    }
}
