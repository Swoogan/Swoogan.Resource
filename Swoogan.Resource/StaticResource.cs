using RestSharp;
using System;
using System.Collections.Generic;

namespace Swoogan.Resource
{
    public class StaticResource<T> where T : class, new()
    {
        private readonly Resource _resource;
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

            _resource = new Resource(url, client, requester);
        }

        public List<T> Query(Dictionary<string, object> parameters)
        {
            return _resource.Query<T>(parameters);
        }

        public List<T> Query(object parameters = null)
        {
            return _resource.Query<T>(parameters);
        }

        public T Get(object parameters = null)
        {
            return _resource.Get<T>(parameters);
        }

        public IRestResponse Create(T data = null, object parameters = null)
        {
            return _resource.Create(data, parameters);
        }

        public IRestResponse Update(T data = null, object parameters = null)
        {
            return _resource.Update(data, parameters);
        }

        public IRestResponse Replace(T data = null, object parameters = null)
        {
            return _resource.Replace(data, parameters);
        }

        public IRestResponse Remove(T parameters = null, object data = null)
        {
            return _resource.Remove(data, parameters);
        }
    }
}
