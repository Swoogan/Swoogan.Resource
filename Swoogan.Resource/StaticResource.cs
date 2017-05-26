using Marvin.JsonPatch;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Swoogan.Resource
{
    public class StaticResource<T> : IStaticResource<T> where T : class, new()
    {
        private readonly Resource _resource;
        private readonly List<string> _urlParameters = new List<string>();

        /// <summary>
        /// Create a resource that uses the same object type for all actions
        /// </summary>
        /// <param name="url">
        /// Url template for the resource
        /// eg: http://localhost:8000/api/customer/:id
        /// where `:id` indicates a parameter token that will be 
        /// replaced with a value on the action methods
        /// </param>
        /// <param name="defaultParams">
        /// Default value for any of parameters in the url template.
        /// Values prefaced with an @ will be drawn from the resource object
        /// <code>
        /// var resource = new StaticResource<Customer>("http://localhost:8000/api/customer/:id", new { id: "@Id" });
        /// resource.Update(new Customer { Id: 1, Name = "Colin" });
        /// </code>
        /// </param>
        public StaticResource(string url, object defaultParams = null) : this(url, defaultParams, new RestClient())
        {
            if (url == null) throw new ArgumentNullException("url");
        }

        public StaticResource(string url, object defaultParams, IRestClient client) : this(url, defaultParams, client, new Requester())
        {
            if (url == null) throw new ArgumentNullException("url");
            if (client == null) throw new ArgumentNullException("client");
        }

        public StaticResource(string url, object defaultParams, IRestClient client, IRequester requester)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (client == null) throw new ArgumentNullException("client");
            if (requester == null) throw new ArgumentNullException("requester");

            _resource = new Resource(url, defaultParams, client, requester);
        }

        public List<T> Query(Dictionary<string, object> parameters)
        {
            return _resource.Query<T>(parameters);
        }

        public List<T> Query(object parameters = null)
        {
            return _resource.Query<T>(parameters);
        }

        public IRestResponse<List<T>> QueryResponse(object parameters = null)
        {
            return _resource.QueryResponse<T>(parameters);
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

        public IRestResponse Update(JsonPatchDocument<T> patchDocument, object parameters = null)
        {
            return _resource.Update<T>(patchDocument, parameters);
        }

        public IRestResponse Replace(T data = null, object parameters = null)
        {
            return _resource.Replace(data, parameters);
        }

        public IRestResponse Remove(object parameters = null, T data = null)
        {
            return _resource.Remove(parameters, data);
        }
    }
}
