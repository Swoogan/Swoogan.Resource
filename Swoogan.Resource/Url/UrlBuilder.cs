﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Params = System.Collections.Generic.Dictionary<string, object>;

namespace Swoogan.Resource.Url
{
    public class UrlBuilder
    {
        private readonly List<string> _usedParams = new List<string>();
        private readonly Params _defaultValues;
        private readonly List<Token> _urlTokens;

        /// <summary>
        /// Create a UrlBuilder
        /// </summary>
        /// <param name="urlTemplate">Url template to build urls from</param>
        /// <param name="defaultValues">
        /// An object that contains the default values for 
        /// parameters in the Url template
        /// </param>
        public UrlBuilder(string urlTemplate, object defaultValues = null)
        {
            var lexer = new Lexer();
            lexer.Lex(urlTemplate);

            _urlTokens = lexer.Tokens;
            _defaultValues = ObjectToDictionary(defaultValues);
        }

        /// <summary>
        /// Create a UrlBuilder
        /// </summary>
        /// <param name="urlTemplate">Url template to build urls from</param>
        /// <param name="defaultValues">
        /// A dictionary containing the default values for 
        /// parameters in the Url template
        /// </param>
        public UrlBuilder(string urlTemplate, Params defaultValues)
        {
            if (string.IsNullOrWhiteSpace(urlTemplate))
                throw new ArgumentException("Invalid url template", "urlTemplate");

            var lexer = new Lexer();
            lexer.Lex(urlTemplate);

            _urlTokens = lexer.Tokens;
            _defaultValues = defaultValues;
        }
        
        /// <summary>
        /// Do parameter substitution in a URL
        /// </summary>
        /// <param name="paramObject">
        /// Replace the template parameters with the properties on the object
        /// with the name of the url template parameter
        /// </param>
        /// <param name="dataObject"></param>
        /// <returns>Final url</returns>
        public string BuildUrl(object paramObject, object dataObject = null)
        {
            var parameters = AugmentParameters(ObjectToDictionary(paramObject), dataObject);
            return AssembleUrl(parameters);
        }

        /// <summary>
        /// Do parameter substitution in a URL
        /// </summary>
        /// <param name="parameters">
        /// Replace the template parameters with the value from the dictionary
        /// where the key matches the name of the url template parameter
        /// </param>
        /// <param name="dataObject"></param>
        /// <returns>Final url</returns>
        public string BuildUrl(Params parameters, object dataObject)
        {
            parameters = AugmentParameters(parameters, dataObject);
            return AssembleUrl(parameters ?? new Dictionary<string, object>());
        }

        private Params AugmentParameters(Params arguments, object dataObject = null)
        {
            if (_defaultValues == null) return arguments;

            if (arguments == null)
                arguments = new Params();

            var data = ObjectToDictionary(dataObject);
            var @params = _urlTokens.Where(x => x.Type == TokenType.Parameter).Select(x => x.Value).ToList();

            foreach (var pair in _defaultValues.Where(x => !arguments.ContainsKey(x.Key)))
            {
                var propValue = pair.Value.ToString();
                if (propValue.StartsWith("@"))
                {
                    // If the default parameter values starts with '@' it is replaced 
                    // with property values from the dataObject. If there is a parameter
                    // in the url template replace it with the value of the property in 
                    // the dataObject as specified by the _defaultValues
                    var name = propValue.TrimStart('@');
                    if (@params.Contains(pair.Key) && data.ContainsKey(name))
                        arguments[pair.Key] = data[name];
                }
                else
                {
                    arguments[pair.Key] = _defaultValues[pair.Key];
                }
            }

            return arguments;
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

        private static Params ObjectToDictionary(object paramObject)
        {
            if (paramObject == null)
                return new Params();

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
                    if (!parameters.TryGetValue(token.Value, out paramValue))
                        return "";

                    _usedParams.Add(token.Value);
                    return HttpUtility.UrlEncode(paramValue.ToString());
                default:
                    throw new Exception("Uknown token type");
            }
        }

        /// <summary>
        /// Go through the leftover parameters and turn them into querystring values
        /// </summary>
        private static string BuildQueryString(Params parameters,
            ICollection<string> usedParams)
        {
            var queryParams =
                parameters.Where(p => !usedParams.Contains(p.Key))
                    .ToDictionary(p => p.Key, p => p.Value);

            return queryParams.Count > 0
                ? String.Concat("?", BuildQueryString(queryParams))
                : "";
        }

        private static string BuildQueryString(Params parameters)
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
