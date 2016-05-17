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
        IRestResponse Remove(object parameters = null, T data = null);
        IRestResponse Replace(T data = null, object parameters = null);
        IRestResponse Update(T data = null, object parameters = null);
    }
}