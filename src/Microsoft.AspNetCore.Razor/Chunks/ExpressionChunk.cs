// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Chunks
{
    public class ExpressionChunk : Chunk
    {
        public string Code { get; set; }

        public override string ToString()
        {
            return Start + " = " + Code;
        }
    }
}
