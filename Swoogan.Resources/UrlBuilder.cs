using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Swoogan.Resource
{
    public class UrlBuilder
    {
        private readonly Dictionary<string, object> _queryParams = new Dictionary<string, object>();

        /*
        public List<string> ParseUrl(string url)
        {
            var parameters = new List<string>();
            var index = 0;
            while (index >= 0 && index < url.Length)
            {
                index = url.IndexOf('{', index);
                if (index > -1)
                {
                    var end = url.IndexOf('}', index) + 1;
                    if (end <= 0)
                        throw new MalformedUrlException();

                    parameters.Add(url.Substring(index, end - index));
                    index++;
                }
            }

            return parameters;
        }

        private Dictionary<string, string> GetParameters(object args, IRestRequest request)
        {
            var parameters = new Dictionary<string, string>();
            if (args != null)
            {
                if (args is IDictionary<string, object>)
                {
                    foreach (var param in (args as IDictionary<string, object>))
                        if (param.Value != null)
                            parameters.Add(param.Key, param.Value.ToString());
                }
                else if (args is object)
                {
                    var properties = args.GetType().GetProperties().Where(p => p.CanRead);
                    foreach (var p in properties)
                    {
                        var val = p.GetValue(args);
                        if (val != null)
                            parameters.Add(p.Name, val.ToString());
                    }
                }
            }
            return parameters;
        }
        */

        public string BuildUrl(string url, object parameters)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            if (parameters == null) return url;
            
            var finalUrl = url;
            var properties = parameters.GetType().GetProperties().Where(p => p.CanRead);

            foreach (var prop in properties)
                finalUrl = ReplaceParameter(prop.Name, prop.GetValue(parameters), finalUrl);

            return _queryParams.Count > 0 
                ? string.Concat(finalUrl, "?", BuildQueryString()) 
                : finalUrl;
        }

        public string BuildUrl(string url, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            if (parameters == null) return url;

            var finalUrl = url;

            foreach (var param in parameters)
                finalUrl = ReplaceParameter(param.Key, param.Value, finalUrl);

            return _queryParams.Count > 0
                ? string.Concat(finalUrl, "?", BuildQueryString())
                : finalUrl;
        }

        private string BuildQueryString()
        {
            var query = string.Empty;

            var items = new List<string>();

            foreach (var param in _queryParams)
            {
                items.Add(string.Concat(
                    HttpUtility.UrlEncode(param.Key), 
                    "=", 
                    HttpUtility.UrlEncode(param.Value.ToString())));
            }

            return string.Join("&", items);
        }

        private string ReplaceParameter(string name, object value, string url)
        {
            var index = url.IndexOf(":" + name);

            if (index == -1)
            {
                _queryParams.Add(name, value);
                return url;
            }
            else
            {
                var finalUrl = url.Remove(index, name.Length + 1);
                return finalUrl.Insert(index, HttpUtility.UrlEncode(value.ToString()));
            }
        }
    }
}
