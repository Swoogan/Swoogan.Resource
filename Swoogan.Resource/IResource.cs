using System.Collections.Generic;
using RestSharp;

namespace Swoogan.Resource
{
    public interface IResource
    {
        IRestResponse Create(object data = null, object parameters = null);
        object Get(object parameters = null);
        T Get<T>(object parameters = null) where T : class, new();
//        List<object> Query(object parameters = null);
        List<T> Query<T>(Dictionary<string, object> parameters) where T : class;
        List<T> Query<T>(object parameters = null) where T : class;
        IRestResponse Remove(object parameters = null, object data = null);
        IRestResponse Replace(object data = null, object parameters = null);
        IRestResponse Update(object data = null, object parameters = null);
    }
}