using System.Collections.Generic;
using RestSharp;

namespace Swoogan.Resource
{
    public interface IStaticResource<T> where T : class, new()
    {
        IRestResponse Create(T data = null, object parameters = null);
        T Get(object parameters = null);
        List<T> Query(Dictionary<string, object> parameters);
        List<T> Query(object parameters = null);
        IRestResponse Remove(T parameters = null, object data = null);
        IRestResponse Replace(T data = null, object parameters = null);
        IRestResponse Update(T data = null, object parameters = null);
    }
}