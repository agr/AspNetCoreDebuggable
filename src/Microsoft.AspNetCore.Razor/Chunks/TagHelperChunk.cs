// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.Compilation.TagHelpers;

namespace Microsoft.AspNetCore.Razor.Chunks
{
    /// <summary>
    /// A <see cref="ParentChunk"/> that represents a special HTML tag.
    /// </summary>
    public class TagHelperChunk : ParentChunk
    {
        /// <summary>
        /// Instantiates a new <see cref="TagHelperChunk"/>.
        /// </summary>
        /// <param name="tagName">The tag name associated with the tag helpers HTML element.</param>
        /// <param name="tagMode">HTML syntax of the element in the Razor source.</param>
        /// <param name="attributes">The attributes associated with the tag helpers HTML element.</param>
        /// <param name="descriptors">
        /// The <see cref="TagHelperDescriptor"/>s associated with this tag helpers HTML element.
        /// </param>
        public TagHelperChunk(
            string tagName,
            TagMode tagMode,
            IList<TagHelperAttributeTracker> attributes,
            IEnumerable<TagHelperDescriptor> descriptors)
        {
            TagName = tagName;
            TagMode = tagMode;
            Attributes = attributes;
            Descriptors = descriptors;
        }

        /// <summary>
        /// The HTML attributes.
        /// </summary>
        public IList<TagHelperAttributeTracker> Attributes { get; set; }

        /// <summary>
        /// The <see cref="TagHelperDescriptor"/>s that are associated with the tag helpers HTML element.
        /// </summary>
        public IEnumerable<TagHelperDescriptor> Descriptors { get; set; }

        /// <summary>
        /// The HTML tag name.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets the HTML syntax of the element in the Razor source.
        /// </summary>
        public TagMode TagMode { get; }
    }
}