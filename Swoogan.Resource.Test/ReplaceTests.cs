using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Moq;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class ReplaceTests
    {
        [TestMethod]
        public void Replace_With_Body()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.PUT)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };
            var res = new Resource("http://localhost/customer", null, client.Object, requester.Object);
            var response = res.Replace(customer);

            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(1, request.Parameters.Count);
            Assert.AreEqual("application/json", request.Parameters[0].ContentType);
            Assert.AreEqual(ParameterType.RequestBody, request.Parameters[0].Type);
            Assert.IsTrue(request.Parameters[0].Value is Customer);
            Assert.AreEqual(customer, request.Parameters[0].Value);
        }

        [TestMethod]
        public void Replace_With_Params()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.PUT)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };
            var res = new Resource("http://localhost/customer", null, client.Object, requester.Object);
            var response = res.Replace(customer);

            Assert.AreEqual(Method.PUT, request.Method);
            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(1, request.Parameters.Count);
            Assert.AreEqual("application/json", request.Parameters[0].ContentType);
            Assert.AreEqual(ParameterType.RequestBody, request.Parameters[0].Type);
            Assert.IsTrue(request.Parameters[0].Value is Customer);
            Assert.AreEqual(customer, request.Parameters[0].Value);
        }

        [TestMethod]
        public void Replace_With_Default_Params()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.PUT)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());
            client.SetupSet(c => c.BaseUrl = It.IsAny<Uri>());

            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };
            var res = new Resource("http://localhost/customer/:id", new { id = "@Id" }, client.Object, requester.Object);
            var response = res.Replace(customer);

            Assert.AreEqual(Method.PUT, request.Method);
            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(1, request.Parameters.Count);
            Assert.AreEqual("application/json", request.Parameters[0].ContentType);
            Assert.AreEqual(ParameterType.RequestBody, request.Parameters[0].Type);
            Assert.IsTrue(request.Parameters[0].Value is Customer);
            Assert.AreEqual(customer, request.Parameters[0].Value);

            client.VerifySet(x => x.BaseUrl = new Uri("http://localhost/customer/1"));
        }


        [TestMethod]
        public void Replace_Without_Body()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.PUT)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var res = new Resource("http://localhost/customer", null, client.Object, requester.Object);
            var response = res.Replace();

            Assert.AreEqual(Method.PUT, request.Method);
            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(1, request.Parameters.Count);            
        }
    }
}
