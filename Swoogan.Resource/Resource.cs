using RestSharp;
using System;
using System.Collections.Generic;
using Swoogan.Resource.Url;

namespace Swoogan.Resource
{
    // args go into url if param present, query string otherwise
    public class Resource : IResource
    {
        private readonly string _url;
        private readonly IRestClient _client;
        private readonly IRequester _requester;
        private readonly List<string> _urlParameters = new List<string>();

        public Resource(string url) : this(url, new RestClient())
        {
            if (url == null) throw new ArgumentNullException("url");
        }

        public Resource(string url, IRestClient client) : this(url, client, new Requester())
        {
            if (url == null) throw new ArgumentNullException("url");
            if (client == null) throw new ArgumentNullException("client");
        }

        public Resource(string url, IRestClient client, IRequester requester)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (client == null) throw new ArgumentNullException("client");
            if (requester == null) throw new ArgumentNullException("requester");

            _client = client;
            _requester = requester;
            _url = url;
        }

        public List<object> Query(object parameters = null)
        {
            // client.Authenticator = new HttpBasicAuthenticator("username", "password");
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute(request);
            
            var obj = SimpleJson.DeserializeObject(response.Content);
            
            var result = new List<object>();
            return result;
        }

        public List<T> Query<T>(Dictionary<string, object> parameters) where T : class
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute<List<T>>(request);
            return response.Data;
        }

        public List<T> Query<T>(object parameters = null) where T : class
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute<List<T>>(request);
            return response.Data;
        }

        public object Get(object parameters = null)
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute(request);
            return SimpleJson.DeserializeObject(response.Content);
        }

        public T Get<T>(object parameters = null) where T : class, new()
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            var uriString = builder.BuildUrl(_url, parameters);
            _client.BaseUrl = new Uri(uriString);

            var response = _client.Execute<T>(request);
            return response.Data;
        }

        public IRestResponse Create(object data = null, object parameters = null)
        {
            var request = _requester.NewRequest(Method.POST);

            if (data != null)
            {
                request.AddJsonBody(data);                
            }
            else
            {
                request.RequestFormat = DataFormat.Json;
            }

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute(request);
            return response;
        }

        public IRestResponse Update(object data = null, object parameters = null)
        {
            var request = _requester.NewRequest(Method.PATCH);
            request.AddJsonBody(data);

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute(request);
            return response;
        }

        public IRestResponse Replace(object data = null, object parameters = null)
        {
            var request = _requester.NewRequest(Method.PUT);
            request.AddJsonBody(data);

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            request.AddJsonBody(data);
            return _client.Execute(request);
        }

        public IRestResponse Remove(object parameters = null, object data = null)
        {
            var request = _requester.NewRequest(Method.DELETE);
            request.AddJsonBody(data);

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            request.AddJsonBody(data);
            return _client.Execute(request);
        }
    }
}
