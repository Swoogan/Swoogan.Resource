using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp.Extensions.MonoHttp;
using Params = System.Collections.Generic.IReadOnlyDictionary<string, object>;

namespace Swoogan.Resource.Url
{
    public class UrlBuilder
    {
        private readonly List<string> _usedParams = new List<string>();

        /// <summary>
        /// Do parameter substitution in a URL
        /// </summary>
        /// <param name="url">Template url to subsutitue values in</param>
        /// <param name="paramObject">
        /// Replace the template parameters with the properties on the object
        /// with the name of the url template parameter
        /// </param>
        /// <returns>Final url</returns>
        public string BuildUrl(string url, object paramObject, object defaultParamsObject = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            var parameters = ObjectToDictionary(paramObject);
            var defaultParams = ObjectToDictionary(defaultParamsObject);

            return AssembleUrl(url, parameters, defaultParams);
        }

        /// <summary>
        /// Do parameter substitution in a URL
        /// </summary>
        /// <param name="url">Template url to subsutitue values in</param>
        /// <param name="parameters">
        /// Replace the template parameters with the value from the dictionary
        /// where the key matches the name of the url template parameter
        /// </param>
        /// <returns>Final url</returns>
        public string BuildUrl(string url, Dictionary<string, object> parameters, object defaultParams = null)
        {
            return string.IsNullOrWhiteSpace(url)
                ? string.Empty
                : AssembleUrl(url, parameters ?? new Dictionary<string, object>(), ObjectToDictionary(defaultParams));
        }

        private string AssembleUrl(string url, Params parameters, Params defaultParams)
        {
            var lexer = new Lexer();
            lexer.Lex(url);

            _usedParams.Clear();

            var assembledUrl = lexer.Tokens.Aggregate("",
                (current, token) => current + AssembledUrl(parameters, defaultParams, token, current));

            return Cleanup(assembledUrl) + BuildQueryString(parameters, _usedParams);
        }

        private string AssembledUrl(Params parameters, Params defaultParams, Token token, string assembledUrl)
        {
            var val = EvaluateToken(token, parameters, defaultParams);
            return val == "/" && assembledUrl.EndsWith("/") ? "" : val;
        }

        private static Dictionary<string, object> ObjectToDictionary(object paramObject)
        {
            Dictionary<string, object> parameters;

            if (paramObject != null)
            {
                var properties = paramObject.GetType().GetProperties().Where(p => p.CanRead);
                parameters = properties.ToDictionary(x => x.Name, y => y.GetValue(paramObject));
            }
            else
            {
                parameters = new Dictionary<string, object>();
            }

            return parameters;
        }

        private static string Cleanup(string url)
        {
            return url.Replace(@"\:", ":");
        }

        private string EvaluateToken(Token token, Params parameters, Params defaultParams)
        {
            switch (token.Type)
            {
                case TokenType.Literal:
                    return token.Value;
                case TokenType.Parameter:
                    object paramValue;
                    if (parameters.TryGetValue(token.Value, out paramValue))
                        return HttpUtility.UrlEncode(paramValue.ToString());

                    if (!defaultParams.TryGetValue(token.Value, out paramValue))
                        return "";

                    if (paramValue is string)
                    {
                        var val = paramValue.ToString();
                        if (val.StartsWith("@"))
                            return HttpUtility.UrlEncode(val);
                    }
                        
                    _usedParams.Add(token.Value);
                    return HttpUtility.UrlEncode(paramValue.ToString());
                default:
                    throw new Exception("Uknown token type");
            }
        }

        /// <summary>
        /// Go through the leftover parameters and turn them into querystring values
        /// </summary>
        private static string BuildQueryString(IEnumerable<KeyValuePair<string, object>> parameters,
            ICollection<string> usedParams)
        {
            var queryParams =
                parameters.Where(p => !usedParams.Contains(p.Key))
                    .ToDictionary(p => p.Key, p => p.Value);

            return queryParams.Count > 0
                ? String.Concat("?", BuildQueryString(queryParams))
                : "";
        }

        private static string BuildQueryString(Dictionary<string, object> parameters)
        {
            var items = parameters.Select(MakeParam);
            return string.Join("&", items);
        }

        private static string MakeParam(KeyValuePair<string, object> param)
        {
            return string.Concat(
                HttpUtility.UrlEncode(param.Key),
                "=",
                HttpUtility.UrlEncode(param.Value.ToString()));
        }

    }
}
