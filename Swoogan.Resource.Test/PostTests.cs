using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Moq;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class PostTests
    {
        [TestMethod]
        public void With_Body()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.POST)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };
            var res = new Resource("http://localhost/customer", client.Object, requester.Object);
            var response = res.Create(customer);

            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(1, request.Parameters.Count);
            Assert.AreEqual(null, request.Parameters[0].ContentType);
            Assert.AreEqual("application/json", request.Parameters[0].Name);
            Assert.AreEqual(ParameterType.RequestBody, request.Parameters[0].Type);
            Assert.AreEqual("{\"Id\":1,\"FirstName\":\"Colin\",\"LastName\":\"Svingen\"}", request.Parameters[0].Value);
        }

        [TestMethod]
        public void With_Params()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.POST)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };
            var res = new Resource("http://localhost/customer", client.Object, requester.Object);
            var response = res.Create(customer);

            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(1, request.Parameters.Count);
            Assert.AreEqual(null, request.Parameters[0].ContentType);
            Assert.AreEqual("application/json", request.Parameters[0].Name);
            Assert.AreEqual(ParameterType.RequestBody, request.Parameters[0].Type);
            Assert.AreEqual("{\"Id\":1,\"FirstName\":\"Colin\",\"LastName\":\"Svingen\"}", request.Parameters[0].Value);
        }

        [TestMethod]
        public void Without_Body()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(Method.POST)).Returns<Method>(m =>
            {
                request = new RestRequest(m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var res = new Resource("http://localhost/customer", client.Object, requester.Object);
            var response = res.Create();

            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual(0, request.Parameters.Count);            
        }
    }
}
