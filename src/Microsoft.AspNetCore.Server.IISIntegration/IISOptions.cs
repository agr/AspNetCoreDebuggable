// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace Microsoft.AspNetCore.Builder
{
    public class IISOptions
    {
        /// <summary>
        /// If true the authentication middleware alter the request user coming in and respond to generic challenges.
        /// If false the authentication middleware will only provide identity and respond to challenges when explicitly indicated
        /// by the AuthenticationScheme.
        /// </summary>
        public bool AutomaticAuthentication { get; set; } = true;

        /// <summary>
        /// If true authentication middleware will try to authenticate using platform handler windows authentication
        /// If false authentication middleware won't be added
        /// </summary>
        public bool ForwardWindowsAuthentication { get; set; } = true;

        /// <summary>
        /// Populates the ITLSConnectionFeature if the MS-ASPNETCORE-CLIENTCERT request header is present.
        /// </summary>
        public bool ForwardClientCertificate { get; set; } = true;

        /// <summary>
        /// Additional information about the authentication type which is made available to the application.
        /// </summary>
        public IList<AuthenticationDescription> AuthenticationDescriptions { get; } = new List<AuthenticationDescription>()
        {
            new AuthenticationDescription()
            {
                AuthenticationScheme = IISDefaults.Negotiate
            },
            new AuthenticationDescription()
            {
                AuthenticationScheme = IISDefaults.Ntlm
            }
        };
    }
}