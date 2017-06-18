// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Chunks.Generators;

namespace Microsoft.AspNetCore.Razor.Parser.SyntaxTree
{
    public class BlockBuilder
    {
        public BlockBuilder()
        {
            Reset();
        }

        public BlockBuilder(Block original)
        {
            Type = original.Type;
            Children = new List<SyntaxTreeNode>(original.Children);
            ChunkGenerator = original.ChunkGenerator;
        }

        public BlockType? Type { get; set; }
        public List<SyntaxTreeNode> Children { get; private set; }
        public IParentChunkGenerator ChunkGenerator { get; set; }

        public virtual Block Build()
        {
            return new Block(this);
        }

        public virtual void Reset()
        {
            Type = null;
            Children = new List<SyntaxTreeNode>();
            ChunkGenerator = ParentChunkGenerator.Null;
        }
    }
}
