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
        private readonly Params _defaultParams;
        private readonly List<Token> _urlTokens;

        /// <summary>
        /// Create a UrlBuilder
        /// </summary>
        /// <param name="urlTemplate">Url template to build urls from</param>
        /// <param name="defaultParams">
        /// An object that contains the default values for 
        /// parameters in the Url template
        /// </param>
        public UrlBuilder(string urlTemplate, object defaultParams = null)
        {
            var lexer = new Lexer();
            lexer.Lex(urlTemplate);

            _urlTokens = lexer.Tokens;
            _defaultParams = ObjectToDictionary(defaultParams);
        }


        /// <summary>
        /// Create a UrlBuilder
        /// </summary>
        /// <param name="urlTemplate">Url template to build urls from</param>
        /// <param name="defaultParams">
        /// A dictionary containing the default values for 
        /// parameters in the Url template
        /// </param>
        public UrlBuilder(string urlTemplate, Params defaultParams)
        {
            if (string.IsNullOrWhiteSpace(urlTemplate))
                throw new ArgumentException("Invalid url template", "urlTemplate");

            var lexer = new Lexer();
            lexer.Lex(urlTemplate);

            _urlTokens = lexer.Tokens;
            _defaultParams = defaultParams;
        }


        /// <summary>
        /// Converts the parameter object to a dictionary and adds
        /// the values of any default parameters that have static values
        /// </summary>
        public Params AugmentParameters(object paramObject = null)
        {
            var parameters = ObjectToDictionary(paramObject);
            foreach (var pair in _defaultParams.Where(x => !parameters.ContainsKey(x.Key)))
                parameters[pair.Key] = pair.Value;

            return parameters;
        }

        /// <summary>
        /// Do parameter substitution in a URL
        /// </summary>
        /// <param name="paramObject">
        /// Replace the template parameters with the properties on the object
        /// with the name of the url template parameter
        /// </param>
        /// <returns>Final url</returns>
        public string BuildUrl(object paramObject)
        {
            var parameters = ObjectToDictionary(paramObject);
            return AssembleUrl(parameters);
        }

        /// <summary>
        /// Do parameter substitution in a URL
        /// </summary>
        /// <param name="parameters">
        /// Replace the template parameters with the value from the dictionary
        /// where the key matches the name of the url template parameter
        /// </param>
        /// <returns>Final url</returns>
        public string BuildUrl(Dictionary<string, object> parameters)
        {
            return AssembleUrl(parameters ?? new Dictionary<string, object>());
        }

        private string AssembleUrl(Params parameters)
        {
            _usedParams.Clear();

            var assembledUrl = _urlTokens.Aggregate("",
                (current, token) => current + AssembledUrl(parameters, token, current));

            return Cleanup(assembledUrl) + BuildQueryString(parameters, _usedParams);
        }

        private string AssembledUrl(Params parameters, Token token, string assembledUrl)
        {
            var val = EvaluateToken(token, parameters);
            return val == "/" && assembledUrl.EndsWith("/") ? "" : val;
        }

        private static Dictionary<string, object> ObjectToDictionary(object paramObject)
        {
            if (paramObject == null)
                return new Dictionary<string, object>();

            var properties = paramObject.GetType().GetProperties().Where(p => p.CanRead);
            return properties.ToDictionary(x => x.Name, y => y.GetValue(paramObject));
        }

        private static string Cleanup(string url)
        {
            return url.Replace(@"\:", ":");
        }

        private string EvaluateToken(Token token, Params parameters)
        {
            switch (token.Type)
            {
                case TokenType.Literal:
                    return token.Value;
                case TokenType.Parameter:
                    object paramValue;
                    if (parameters.TryGetValue(token.Value, out paramValue))
                    {
                        _usedParams.Add(token.Value);
                        return HttpUtility.UrlEncode(paramValue.ToString());
                    }

                    if (!_defaultParams.TryGetValue(token.Value, out paramValue))
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
