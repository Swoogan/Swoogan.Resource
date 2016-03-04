using System;
using RestSharp;

namespace Swoogan.Resource
{
    public interface IRequester
    {
        IRestRequest NewRequest();
        IRestRequest NewRequest(Uri resource);
        IRestRequest NewRequest(string resource);
        IRestRequest NewRequest(Method method);
        IRestRequest NewRequest(Uri resource, Method method);
        IRestRequest NewRequest(string resource, Method method);
    }
}