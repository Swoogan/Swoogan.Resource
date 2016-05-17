using System;
using RestSharp;

namespace Swoogan.Resource
{
    public class Requester : IRequester
    {
        public IRestRequest NewRequest()
        {
            return new RestRequest
            {
                RequestFormat = DataFormat.Json
            };
        }

        public IRestRequest NewRequest(Method method)
        {
            return new RestRequest(method)
            {
                RequestFormat = DataFormat.Json
            };
        }

        public IRestRequest NewRequest(string resource)
        {
            return new RestRequest(resource)
            {
                RequestFormat = DataFormat.Json
            };
        }

        public IRestRequest NewRequest(Uri resource)
        {
            return new RestRequest(resource)
            {
                RequestFormat = DataFormat.Json
            };
        }

        public IRestRequest NewRequest(string resource, Method method)
        {
            return new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json
            };
        }

        public IRestRequest NewRequest(Uri resource, Method method)
        {
            return new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json
            };
        }
    }
}
