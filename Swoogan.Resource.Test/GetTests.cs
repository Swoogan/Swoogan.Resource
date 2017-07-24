using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Moq;
using System.Collections.Generic;
using System.Net;

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
            response.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);
            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Get<Customer>();

            Assert.AreEqual(customer, result);
        }

        [TestMethod]
        public void Typed_Get_Null()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<Customer>>();

            response.Setup(r => r.Data).Returns<Customer>(null);
            response.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Error);
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);
            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Get<Customer>();

            Assert.IsNull(result);
        }



        [TestMethod]
        [ExpectedException(typeof(GetException))]
        public void Typed_Get_Error()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<Customer>>();

            response.Setup(r => r.Data).Returns<Customer>(null);
            response.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Error);
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Forbidden);
            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Get<Customer>();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void Typed_Get_UnknownError()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<Customer>>();

            response.Setup(r => r.Data).Returns<Customer>(null);
            response.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Error);
            response.Setup(r => r.StatusCode).Returns(0);
            response.Setup(r => r.ErrorException).Returns(new System.Exception());
            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Get<Customer>();
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
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<Customer>>();
            response.Setup(r => r.Data).Returns<Customer>(null);
            response.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);
            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Get<Customer>();
            Assert.IsNull(result);
        }
    }
}
