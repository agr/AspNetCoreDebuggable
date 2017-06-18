﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.WebUtilities
{
    public static class Base64UrlTextEncoder
    {
        /// <summary>
        /// Encodes supplied data into Base64 and replaces any URL encodable characters into non-URL encodable
        /// characters.
        /// </summary>
        /// <param name="data">Data to be encoded.</param>
        /// <returns>Base64 encoded string modified with non-URL encodable characters</returns>
        public static string Encode(byte[] data)
        {
            return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Decodes supplied string by replacing the non-URL encodable characters with URL encodable characters and
        /// then decodes the Base64 string.
        /// </summary>
        /// <param name="text">The string to be decoded.</param>
        /// <returns>The decoded data.</returns>
        public static byte[] Decode(string text)
        {
            return Convert.FromBase64String(Pad(text.Replace('-', '+').Replace('_', '/')));
        }

        private static string Pad(string text)
        {
            var padding = 3 - ((text.Length + 3) % 4);
            if (padding == 0)
            {
                return text;
            }
            return text + new string('=', padding);
        }
    }
}
