using RestSharp;
using System;
using System.Collections.Generic;

namespace Swoogan.Resource
{
    public class StaticResource<T> where T : class, new()
    {
        private readonly string _url;
        private readonly IRestClient _client;
        private readonly IRequester _requester;
        private readonly List<string> _urlParameters = new List<string>();

        public StaticResource(string url) : this(url, new RestClient())
        {
            if (url == null) throw new ArgumentNullException("url");
        }

        public StaticResource(string url, IRestClient client) : this(url, client, new Requester())
        {
            if (url == null) throw new ArgumentNullException("url");
            if (client == null) throw new ArgumentNullException("client");
        }

        public StaticResource(string url, IRestClient client, IRequester requester)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (client == null) throw new ArgumentNullException("client");
            if (requester == null) throw new ArgumentNullException("requester");

            _client = client;
            _requester = requester;
            _url = url;
        }

        public List<T> Query(Dictionary<string, object> parameters)
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute<List<T>>(request);
            return response.Data;
        }

        public List<T> Query(object parameters = null)
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute<List<T>>(request);
            return response.Data;
        }

        public T Get(object parameters = null)
        {
            var request = _requester.NewRequest();

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute<T>(request);
            return response.Data;
        }

        public IRestResponse Create(T data = null, object parameters = null)
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

        public IRestResponse Update(T data = null, object parameters = null)
        {
            var request = _requester.NewRequest(Method.PATCH);
            request.AddJsonBody(data);

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            var response = _client.Execute(request);
            return response;
        }

        public IRestResponse Replace(T data = null, object parameters = null)
        {
            var request = _requester.NewRequest(Method.PUT);
            request.AddJsonBody(data);

            var builder = new UrlBuilder();
            _client.BaseUrl = new Uri(builder.BuildUrl(_url, parameters));

            request.AddJsonBody(data);
            return _client.Execute(request);
        }

        public IRestResponse Remove(T parameters = null, object data = null)
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
