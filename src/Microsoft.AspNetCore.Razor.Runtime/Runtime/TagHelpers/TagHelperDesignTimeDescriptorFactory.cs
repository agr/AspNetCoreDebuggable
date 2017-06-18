// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Compilation.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Runtime.TagHelpers
{
    /// <summary>
    /// Factory for providing <see cref="TagHelperDesignTimeDescriptor"/>s from <see cref="Type"/>s and
    /// <see cref="TagHelperAttributeDesignTimeDescriptor"/>s from <see cref="PropertyInfo"/>s.
    /// </summary>
    public class TagHelperDesignTimeDescriptorFactory
    {
        private readonly ConcurrentDictionary<int, XmlDocumentationProvider> _documentationProviderCache =
            new ConcurrentDictionary<int, XmlDocumentationProvider>();

        /// <summary>
        /// Creates a <see cref="TagHelperDesignTimeDescriptor"/> from the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> to create a <see cref="TagHelperDesignTimeDescriptor"/> from.
        /// </param>
        /// <returns>A <see cref="TagHelperDesignTimeDescriptor"/> that describes design time specific information
        /// for the given <paramref name="type"/>.</returns>
        public TagHelperDesignTimeDescriptor CreateDescriptor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var id = XmlDocumentationProvider.GetId(type);
            var documentationDescriptor = CreateDocumentationDescriptor(type.GetTypeInfo().Assembly, id);

            var outputElementHintAttribute = type
                .GetTypeInfo()
                .GetCustomAttributes(inherit: false)
                ?.OfType<OutputElementHintAttribute>()
                .FirstOrDefault();
            var outputElementHint = outputElementHintAttribute?.OutputElement;

            if (documentationDescriptor != null || outputElementHint != null)
            {
                return new TagHelperDesignTimeDescriptor
                {
                    Summary = documentationDescriptor?.Summary,
                    Remarks = documentationDescriptor?.Remarks,
                    OutputElementHint = outputElementHint
                };
            }

            return null;
        }

        /// <summary>
        /// Creates a <see cref="TagHelperAttributeDesignTimeDescriptor"/> from the given
        /// <paramref name="propertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// The <see cref="PropertyInfo"/> to create a <see cref="TagHelperAttributeDesignTimeDescriptor"/> from.
        /// </param>
        /// <returns>A <see cref="TagHelperAttributeDesignTimeDescriptor"/> that describes design time specific
        /// information for the given <paramref name="propertyInfo"/>.</returns>
        public TagHelperAttributeDesignTimeDescriptor CreateAttributeDescriptor(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var id = XmlDocumentationProvider.GetId(propertyInfo);
            var declaringAssembly = propertyInfo.DeclaringType.GetTypeInfo().Assembly;
            var documentationDescriptor = CreateDocumentationDescriptor(declaringAssembly, id);
            if (documentationDescriptor != null)
            {
                return new TagHelperAttributeDesignTimeDescriptor
                {
                    Summary = documentationDescriptor.Summary,
                    Remarks = documentationDescriptor.Remarks
                };
            }

            return null;
        }

        /// <summary>
        /// Retrieves <paramref name="assembly"/>'s location on disk.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The path to the given <paramref name="assembly"/>.</returns>
        public virtual string GetAssemblyLocation(Assembly assembly)
        {
            var assemblyLocation = assembly.Location;

            return assemblyLocation;
        }

        private XmlDocumentationProvider GetXmlDocumentationProvider(Assembly assembly)
        {
            var hashCodeCombiner = HashCodeCombiner.Start();
            hashCodeCombiner.Add(assembly);
            hashCodeCombiner.Add(CultureInfo.CurrentCulture);
            var cacheKey = hashCodeCombiner.CombinedHash;

            var documentationProvider = _documentationProviderCache.GetOrAdd(cacheKey, valueFactory: _ =>
            {
                var assemblyLocation = GetAssemblyLocation(assembly);

                // Couldn't resolve a valid assemblyLocation.
                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    return null;
                }

                var xmlDocumentationFile = GetXmlDocumentationFile(assemblyLocation);

                // Only want to process the file if it exists.
                if (xmlDocumentationFile != null)
                {
                    return new XmlDocumentationProvider(xmlDocumentationFile.FullName);
                }

                return null;
            });

            return documentationProvider;
        }

        private DocumentationDescriptor CreateDocumentationDescriptor(Assembly assembly, string id)
        {
            var documentationProvider = GetXmlDocumentationProvider(assembly);

            if (documentationProvider != null)
            {
                var summary = documentationProvider.GetSummary(id);
                var remarks = documentationProvider.GetRemarks(id);

                if (!string.IsNullOrEmpty(summary) || !string.IsNullOrEmpty(remarks))
                {
                    return new DocumentationDescriptor
                    {
                        Summary = summary,
                        Remarks = remarks
                    };
                }
            }

            return null;
        }

        private static FileInfo GetXmlDocumentationFile(string assemblyLocation)
        {
            try
            {
                var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
                var assemblyName = Path.GetFileName(assemblyLocation);
                var assemblyXmlDocumentationName = Path.ChangeExtension(assemblyName, ".xml");

                // Check for a localized XML file for the current culture.
                var xmlDocumentationFile = GetLocalizedXmlDocumentationFile(
                    CultureInfo.CurrentCulture,
                    assemblyDirectory,
                    assemblyXmlDocumentationName);

                if (xmlDocumentationFile == null)
                {
                    // Check for a culture-neutral XML file next to the assembly
                    xmlDocumentationFile = new FileInfo(
                        Path.Combine(assemblyDirectory, assemblyXmlDocumentationName));

                    if (!xmlDocumentationFile.Exists)
                    {
                        xmlDocumentationFile = null;
                    }
                }

                return xmlDocumentationFile;
            }
            catch (ArgumentException)
            {
                // Could not resolve XML file.
                return null;
            }
        }

        private static IEnumerable<string> ExpandPaths(
            CultureInfo culture,
            string assemblyDirectory,
            string assemblyXmlDocumentationName)
        {
            // Following the fall-back process defined by:
            // https://msdn.microsoft.com/en-us/library/sb6a8618.aspx#cpconpackagingdeployingresourcesanchor1
            do
            {
                var cultureName = culture.Name;
                var cultureSpecificFileName =
                    Path.ChangeExtension(assemblyXmlDocumentationName, cultureName + ".xml");

                // Look for a culture specific XML file next to the assembly.
                yield return Path.Combine(assemblyDirectory, cultureSpecificFileName);

                // Look for an XML file with the same name as the assembly in a culture specific directory.
                yield return Path.Combine(assemblyDirectory, cultureName, assemblyXmlDocumentationName);

                // Look for a culture specific XML file in a culture specific directory.
                yield return Path.Combine(assemblyDirectory, cultureName, cultureSpecificFileName);

                culture = culture.Parent;
            } while (culture != null && culture != CultureInfo.InvariantCulture);
        }

        private static FileInfo GetLocalizedXmlDocumentationFile(
            CultureInfo culture,
            string assemblyDirectory,
            string assemblyXmlDocumentationName)
        {
            var localizedXmlPaths = ExpandPaths(culture, assemblyDirectory, assemblyXmlDocumentationName);
            var xmlDocumentationFile = localizedXmlPaths
                .Select(path => new FileInfo(path))
                .FirstOrDefault(file => file.Exists);

            return xmlDocumentationFile;
        }

        private class DocumentationDescriptor
        {
            public string Summary { get; set; }
            public string Remarks { get; set; }
        }
    }
}