using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class QueryResponseTests
    {
        [TestMethod]
        public void Query_Response_Typed()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<List<Customer>>>();
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" },
                new Customer { Id = 2, FirstName = "Leha", LastName = "Svingen" },
                new Customer { Id = 2, FirstName = "Jimmy", LastName = "Bob" },
            };

            response.Setup(r => r.Data).Returns(customers);
            response.Setup(r => r.ResponseStatus).Returns(ResponseStatus.Completed);
            client.Setup(c => c.Execute<List<Customer>>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.QueryResponse<Customer>();

            Assert.AreEqual(response.Object, result);
            Assert.AreEqual(customers, result.Data);
        }
    }
}
