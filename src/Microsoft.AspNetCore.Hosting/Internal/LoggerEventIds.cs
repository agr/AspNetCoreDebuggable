// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Hosting.Internal
{
    internal static class LoggerEventIds
    {
        public const int RequestStarting = 1;
        public const int RequestFinished = 2;
        public const int Starting = 3;
        public const int Started = 4;
        public const int Shutdown = 5;
        public const int ApplicationStartupException = 6;
    }
}
