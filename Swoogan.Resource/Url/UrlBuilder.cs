using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp.Extensions.MonoHttp;

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
        public string BuildUrl(string url, object paramObject)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

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

            return AssembleUrl(url, parameters);
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
        public string BuildUrl(string url, Dictionary<string, object> parameters)
        {
            return string.IsNullOrWhiteSpace(url)
                ? string.Empty
                : AssembleUrl(url, parameters ?? new Dictionary<string, object>());
        }

        private string AssembleUrl(string url, IReadOnlyDictionary<string, object> parameters)
        {
            var lexer = new Lexer();
            lexer.Lex(url);

            _usedParams.Clear();

            var assembledUrl = lexer.Tokens.Aggregate("",
                (current, token) => current + AssembledUrl(parameters, token, current));

            return Cleanup(assembledUrl) + BuildQueryString(parameters, _usedParams);
        }

        private string AssembledUrl(IReadOnlyDictionary<string, object> parameters, Token token, string assembledUrl)
        {
            var val = EvaluateToken(token, parameters);
            return val == "/" && assembledUrl.EndsWith("/") ? "" : val;
        }

        private static string Cleanup(string url)
        {
            return url.Replace(@"\:", ":");
        }

        private string EvaluateToken(Token token, IReadOnlyDictionary<string, object> parameters)
        {
            switch (token.Type)
            {
                case TokenType.Literal:
                    return token.Value;
                case TokenType.Parameter:
                    object paramValue;
                    if (!parameters.TryGetValue(token.Value, out paramValue)) return "";
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
