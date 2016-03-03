﻿namespace Kraken.Security
{
    using System;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Kraken.Services;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Http;
    using Microsoft.Extensions.Primitives;

    public class ApiKeyMiddleware
    {
        public ApiKeyMiddleware(RequestDelegate next, IOctopusAuthenticationProxy octopusAuthenticationProxy)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            if (octopusAuthenticationProxy == null) throw new ArgumentNullException(nameof(octopusAuthenticationProxy));

            _next = next;
            _octopusAuthenticationProxy = octopusAuthenticationProxy;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            string apiKey;
            if (TryGetApiKey(httpContext.Request, out apiKey))
            {
                string userName;
                if (_octopusAuthenticationProxy.ValidateApiKey(apiKey, out userName))
                {
                    var principal = ClaimsPrincipalHelpers.CreatePrincipal(userName, apiKey);

                    httpContext.User = principal;
                    Thread.CurrentPrincipal = principal;
                }
            }

            return _next.Invoke(httpContext);
        }

        private static bool TryGetApiKey(HttpRequest request, out string apiKey)
        {
            StringValues headerValue;
            if (request.Headers.TryGetValue("Authorization", out headerValue))
            {
                AuthenticationHeaderValue authHeaderValue;
                if (AuthenticationHeaderValue.TryParse(headerValue, out authHeaderValue))
                {
                    apiKey = authHeaderValue.Parameter;
                    return true;
                }
            }

            if (request.Headers.TryGetValue("X-Octopus-ApiKey", out headerValue))
            {
                apiKey = headerValue;
                return true;
            }

            if (request.Headers.TryGetValue("X-NuGet-ApiKey", out headerValue))
            {
                apiKey = headerValue;
                return true;
            }

            if (request.Query.ContainsKey("apikey"))
            {
                apiKey = request.Query["apikey"];
                return true;
            }

            apiKey = null;
            return false;
        }

        private readonly RequestDelegate _next;
        private readonly IOctopusAuthenticationProxy _octopusAuthenticationProxy;
    }
}
