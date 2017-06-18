// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
#if NET451
using System.Globalization;
#endif

namespace Microsoft.AspNetCore.Razor.Editor
{
    internal static class RazorEditorTrace
    {
        private static bool? _enabled;

        private static bool IsEnabled()
        {
            if (_enabled == null)
            {
                bool enabled;
                if (Boolean.TryParse(Environment.GetEnvironmentVariable("RAZOR_EDITOR_TRACE"), out enabled))
                {
#if NET451
                    // No Trace in CoreCLR

                    Trace.WriteLine(RazorResources.FormatTrace_Startup(
                        enabled ? RazorResources.Trace_Enabled : RazorResources.Trace_Disabled));
#endif
                    _enabled = enabled;
                }
                else
                {
                    _enabled = false;
                }
            }
            return _enabled.Value;
        }

        [Conditional("EDITOR_TRACING")]
        public static void TraceLine(string format, params object[] args)
        {
            if (IsEnabled())
            {
#if NET451
                // No Trace in CoreCLR

                Trace.WriteLine(RazorResources.FormatTrace_Format(
                    string.Format(CultureInfo.CurrentCulture, format, args)));
#endif
            }
        }
    }
}
