using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Moq;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class PostTests
    {
        [TestMethod]
        public void Typed_Post()
        {
            var client = new Mock<IRestClient>();
            var requester = new Mock<IRequester>();

            RestRequest request = null;
            requester.Setup(r => r.NewRequest(It.IsAny<string>(), Method.POST)).Returns<string, Method>((s, m) =>
            {
                request = new RestRequest(s, m);
                return request;
            });

            client.Setup(c => c.Execute<Customer>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Customer>());

            var customer = new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" };
            var res = new Resource("http://localhost/customer", client.Object, requester.Object);
            var response = res.Create(customer);

            Assert.AreEqual(DataFormat.Json, request.RequestFormat);
            Assert.AreEqual("{\"Id\":1,\"FirstName\":\"Colin\",\"LastName\":\"Svingen\"}", request.Parameters[0].Value);
        }
    }
}
