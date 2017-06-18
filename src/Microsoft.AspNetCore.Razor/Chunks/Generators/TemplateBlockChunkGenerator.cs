// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Parser.SyntaxTree;

namespace Microsoft.AspNetCore.Razor.Chunks.Generators
{
    public class TemplateBlockChunkGenerator : ParentChunkGenerator
    {
        public override void GenerateStartParentChunk(Block target, ChunkGeneratorContext context)
        {
            context.ChunkTreeBuilder.StartParentChunk<TemplateChunk>(target);
        }

        public override void GenerateEndParentChunk(Block target, ChunkGeneratorContext context)
        {
            context.ChunkTreeBuilder.EndParentChunk();
        }
    }
}
