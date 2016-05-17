using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Swoogan.Resource.Test
{
    [TestClass]
    public class QueryTest
    {
        [TestMethod]
        public void TypedQuery()
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
            client.Setup(c => c.Execute<List<Customer>>(It.IsAny<IRestRequest>())).Returns(response.Object);

            var res = new Resource("http://localhost/wak", null, client.Object);
            var result = res.Query<Customer>();

            CollectionAssert.AreEqual(customers, result);
        }

        [TestMethod]
        public void TypedQuery_WithDictArgs()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<List<Customer>>>();
            var requester = new Mock<IRequester>();

            string val = string.Empty;
            requester.Setup(r => r.NewRequest(It.IsAny<string>())).Returns<string>(
                s => { val = s; return new RestRequest(s); }
            );

            client.Setup(c => c.Execute<List<Customer>>(It.IsAny<IRestRequest>())).Returns(response.Object);
            client.SetupSet(c => c.BaseUrl = It.IsAny<Uri>()).Verifiable();

            var res = new Resource("http://localhost/wak", null, client.Object, requester.Object);
            var result = res.Query<Customer>(new Dictionary<string, object> { { "LastName", "Svingen" } });

            client.VerifySet(c => c.BaseUrl = new Uri("http://localhost/wak?LastName=Svingen"));
        }

        [TestMethod]
        public void TypedQuery_WithObjArgs()
        {
            var client = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<List<Customer>>>();
            var requester = new Mock<IRequester>();
            
            requester.Setup(r => r.NewRequest()).Returns(new RestRequest());

            client.Setup(c => c.Execute<List<Customer>>(It.IsAny<IRestRequest>())).Returns(response.Object);
            client.SetupSet(c => c.BaseUrl = It.IsAny<Uri>()).Verifiable();

            var res = new Resource("http://localhost/wak", null, client.Object, requester.Object);
            var result = res.Query<Customer>(new { LastName = "Svingen" });

            client.VerifySet(c => c.BaseUrl = new Uri("http://localhost/wak?LastName=Svingen"));
        }

 
        /*

                [TestMethod]
                public void UntypedQueryDict()
                {
                    var client = new Mock<IRestClient>();
                    var response = new Mock<IRestResponse<List<Dictionary<string, object>>>>();
                    var customers = new List<Dictionary<string,object>>
                    {
                        new Dictionary<string,object> { { "Id", 1 } , { "FirstName", "Colin" }, {"LastName", "Svingen" } },
                        new Dictionary<string,object> { { "Id", 2 } , { "FirstName", "Jimmy" }, {"LastName", "Bob" } },
                    };

                    response.Setup(r => r.Data).Returns(customers);
                    response.Setup(r => r.ContentType).Returns("application/json");
                    client.Setup(c => c.Execute<List<Dictionary<string, object>>>(It.IsAny<IRestRequest>())).Returns(response.Object);

                    var res = new Resource("http://localhost/wak", client.Object);
                    var result = res.QueryDict();

                    Assert.AreEqual(customers, result);
                }

                [TestMethod]
                public void UntypedQueryList()
                {
                    var client = new Mock<IRestClient>();
                    var response = new Mock<IRestResponse<List<object>>>();
                    var customers = new List<object>
                    {
                        new Customer { Id = 1, FirstName = "Colin", LastName = "Svingen" },
                        new Customer { Id = 2, FirstName = "Jimmy", LastName = "Bob" },
                    };

                    response.Setup(r => r.Data).Returns(customers);
                    response.Setup(r => r.ContentType).Returns("application/json");
                    client.Setup(c => c.Execute<List<object>>(It.IsAny<IRestRequest>())).Returns(response.Object);

                    var res = new Resource("http://localhost/wak", client.Object);
                    var result = res.QueryList();

                    Assert.AreEqual(customers, result);
                }

                [TestMethod]
                public void QueryExpando()
                {
                    var client = new RestClient();
                    var response = new Mock<IRestResponse<List<ExpandoObject>>>();

                    dynamic cust = new ExpandoObject();
                    cust.Id = 1;
                    cust.FirstName = "Colin";
                    cust.LastName = "Svingen";

                    var customers = new List<ExpandoObject>{ cust };

                    //response.Setup(r => r.Data).Returns(customers);
                    response.Setup(r => r.Content).Returns("{Id: 1, FirstName: 'Colin', LastName: 'Svingen'}");
                    response.Setup(r => r.ContentType).Returns("application/json");
                    //client.Setup(c => c.Execute<List<ExpandoObject>>(It.IsAny<IRestRequest>())).Returns(response.Object);
                    client.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(response.Object);

                    var res = new Resource("http://localhost/wak", client.Object);
                    var result = res.QueryExpando();

                    Assert.AreEqual(customers, result);
                }
         */
    }
}
