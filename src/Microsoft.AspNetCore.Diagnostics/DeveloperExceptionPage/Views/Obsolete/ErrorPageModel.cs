// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Diagnostics.Views
{
    /// <summary>
    /// Holds data to be displayed on the error page.
    /// </summary>
    [Obsolete("This type is for internal use only and will be removed in a future version.")]
    public class ErrorPageModel
    {
        /// <summary>
        /// Options for what output to display.
        /// </summary>
        public DeveloperExceptionPageOptions Options { get; set; }

        /// <summary>
        /// Detailed information about each exception in the stack.
        /// </summary>
        public IEnumerable<ErrorDetails> ErrorDetails { get; set; }

        /// <summary>
        /// Parsed query data.
        /// </summary>
        public IQueryCollection Query { get; set; }

        /// <summary>
        /// Request cookies.
        /// </summary>
        public IRequestCookieCollection Cookies { get; set; }

        /// <summary>
        /// Request headers.
        /// </summary>
        public IDictionary<string, StringValues> Headers { get; set; }
    }
}
