// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Antiforgery.Internal
{
    public class DefaultAntiforgeryTokenStore : IAntiforgeryTokenStore
    {
        private readonly AntiforgeryOptions _options;

        public DefaultAntiforgeryTokenStore(IOptions<AntiforgeryOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;
        }

        public string GetCookieToken(HttpContext httpContext)
        {
            Debug.Assert(httpContext != null);

            var requestCookie = httpContext.Request.Cookies[_options.CookieName];
            if (string.IsNullOrEmpty(requestCookie))
            {
                // unable to find the cookie.
                return null;
            }

            return requestCookie;
        }

        public async Task<AntiforgeryTokenSet> GetRequestTokensAsync(HttpContext httpContext)
        {
            Debug.Assert(httpContext != null);

            var cookieToken = httpContext.Request.Cookies[_options.CookieName];

            // We want to delay reading the form as much as possible, for example in case of large file uploads,
            // request token could be part of the header.
            StringValues requestToken;
            if (_options.HeaderName != null)
            {
                requestToken = httpContext.Request.Headers[_options.HeaderName];
            }

            // Fall back to reading form instead
            if (requestToken.Count == 0 && httpContext.Request.HasFormContentType)
            {
                // Check the content-type before accessing the form collection to make sure
                // we report errors gracefully.
                var form = await httpContext.Request.ReadFormAsync();
                requestToken = form[_options.FormFieldName];
            }

            return new AntiforgeryTokenSet(requestToken, cookieToken, _options.FormFieldName, _options.HeaderName);
        }

        public void SaveCookieToken(HttpContext httpContext, string token)
        {
            Debug.Assert(httpContext != null);
            Debug.Assert(token != null);

            var options = new CookieOptions();
            options.HttpOnly = true;
            options.Domain = _options.CookieDomain;
            // Note: don't use "newCookie.Secure = _options.RequireSSL;" since the default
            // value of newCookie.Secure is populated out of band.
            if (_options.RequireSsl)
            {
                options.Secure = true;
            }
            SetCookiePath(httpContext, options);

            httpContext.Response.Cookies.Append(_options.CookieName, token, options);
        }

        private void SetCookiePath(HttpContext httpContext, CookieOptions cookieOptions)
        {
            if (_options.CookiePath != null)
            {
                cookieOptions.Path = _options.CookiePath.ToString();
            }
            else
            {
                var pathBase = httpContext.Request.PathBase.ToString();
                if (!string.IsNullOrEmpty(pathBase))
                {
                    cookieOptions.Path = pathBase;
                }
            }
        }
    }
}
