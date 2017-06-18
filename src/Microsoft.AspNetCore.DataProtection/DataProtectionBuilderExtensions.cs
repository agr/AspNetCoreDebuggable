// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.IO;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Win32;

#if !NETSTANDARD1_3 // [[ISSUE60]] Remove this #ifdef when Core CLR gets support for EncryptedXml
using System.Security.Cryptography.X509Certificates;
#endif

namespace Microsoft.AspNetCore.DataProtection
{
    /// <summary>
    /// Extensions for configuring data protection using an <see cref="IDataProtectionBuilder"/>.
    /// </summary>
    public static class DataProtectionBuilderExtensions
    {
        /// <summary>
        /// Sets the unique name of this application within the data protection system.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="applicationName">The application name.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// This API corresponds to setting the <see cref="DataProtectionOptions.ApplicationDiscriminator"/> property
        /// to the value of <paramref name="applicationName"/>.
        /// </remarks>
        public static IDataProtectionBuilder SetApplicationName(this IDataProtectionBuilder builder, string applicationName)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<DataProtectionOptions>(options =>
            {
                options.ApplicationDiscriminator = applicationName;
            });

            return builder;
        }

        /// <summary>
        /// Registers a <see cref="IKeyEscrowSink"/> to perform escrow before keys are persisted to storage.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="sink">The instance of the <see cref="IKeyEscrowSink"/> to register.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// Registrations are additive.
        /// </remarks>
        public static IDataProtectionBuilder AddKeyEscrowSink(this IDataProtectionBuilder builder, IKeyEscrowSink sink)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            builder.Services.AddSingleton<IKeyEscrowSink>(sink);
            return builder;
        }

        /// <summary>
        /// Registers a <see cref="IKeyEscrowSink"/> to perform escrow before keys are persisted to storage.
        /// </summary>
        /// <typeparam name="TImplementation">The concrete type of the <see cref="IKeyEscrowSink"/> to register.</typeparam>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// Registrations are additive. The factory is registered as <see cref="ServiceLifetime.Singleton"/>.
        /// </remarks>
        public static IDataProtectionBuilder AddKeyEscrowSink<TImplementation>(this IDataProtectionBuilder builder)
            where TImplementation : class, IKeyEscrowSink
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddSingleton<IKeyEscrowSink, TImplementation>();
            return builder;
        }

        /// <summary>
        /// Registers a <see cref="IKeyEscrowSink"/> to perform escrow before keys are persisted to storage.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="factory">A factory that creates the <see cref="IKeyEscrowSink"/> instance.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// Registrations are additive. The factory is registered as <see cref="ServiceLifetime.Singleton"/>.
        /// </remarks>
        public static IDataProtectionBuilder AddKeyEscrowSink(this IDataProtectionBuilder builder, Func<IServiceProvider, IKeyEscrowSink> factory)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            builder.Services.AddSingleton<IKeyEscrowSink>(factory);
            return builder;
        }

        /// <summary>
        /// Configures the key management options for the data protection system.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{KeyManagementOptions}"/> to configure the provided<see cref="KeyManagementOptions"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder AddKeyManagementOptions(this IDataProtectionBuilder builder, Action<KeyManagementOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);
            return builder;
        }

        /// <summary>
        /// Configures the data protection system not to generate new keys automatically.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// Calling this API corresponds to setting <see cref="KeyManagementOptions.AutoGenerateKeys"/>
        /// to 'false'. See that property's documentation for more information.
        /// </remarks>
        public static IDataProtectionBuilder DisableAutomaticKeyGeneration(this IDataProtectionBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.AutoGenerateKeys = false;
            });
            return builder;
        }

        /// <summary>
        /// Configures the data protection system to persist keys to the specified directory.
        /// This path may be on the local machine or may point to a UNC share.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="directory">The directory in which to store keys.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToFileSystem(this IDataProtectionBuilder builder, DirectoryInfo directory)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            Use(builder.Services, DataProtectionServiceDescriptors.IXmlRepository_FileSystem(directory));
            return builder;
        }

        /// <summary>
        /// Configures the data protection system to persist keys to the Windows registry.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="registryKey">The location in the registry where keys should be stored.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToRegistry(this IDataProtectionBuilder builder, RegistryKey registryKey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (registryKey == null)
            {
                throw new ArgumentNullException(nameof(registryKey));
            }

            Use(builder.Services, DataProtectionServiceDescriptors.IXmlRepository_Registry(registryKey));
            return builder;
        }

#if !NETSTANDARD1_3 // [[ISSUE60]] Remove this #ifdef when Core CLR gets support for EncryptedXml

        /// <summary>
        /// Configures keys to be encrypted to a given certificate before being persisted to storage.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="certificate">The certificate to use when encrypting keys.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder ProtectKeysWithCertificate(this IDataProtectionBuilder builder, X509Certificate2 certificate)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            Use(builder.Services, DataProtectionServiceDescriptors.IXmlEncryptor_Certificate(certificate));
            return builder;
        }

        /// <summary>
        /// Configures keys to be encrypted to a given certificate before being persisted to storage.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="thumbprint">The thumbprint of the certificate to use when encrypting keys.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder ProtectKeysWithCertificate(this IDataProtectionBuilder builder, string thumbprint)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (thumbprint == null)
            {
                throw new ArgumentNullException(nameof(thumbprint));
            }

            // Make sure the thumbprint corresponds to a valid certificate.
            if (new CertificateResolver().ResolveCertificate(thumbprint) == null)
            {
                throw Error.CertificateXmlEncryptor_CertificateNotFound(thumbprint);
            }

            var services = builder.Services;

            // ICertificateResolver is necessary for this type to work correctly, so register it
            // if it doesn't already exist.
            services.TryAdd(DataProtectionServiceDescriptors.ICertificateResolver_Default());
            Use(services, DataProtectionServiceDescriptors.IXmlEncryptor_Certificate(thumbprint));
            return builder;
        }

#endif

        /// <summary>
        /// Configures keys to be encrypted with Windows DPAPI before being persisted to
        /// storage. The encrypted key will only be decryptable by the current Windows user account.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// This API is only supported on Windows platforms.
        /// </remarks>
        public static IDataProtectionBuilder ProtectKeysWithDpapi(this IDataProtectionBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ProtectKeysWithDpapi(protectToLocalMachine: false);
        }

        /// <summary>
        /// Configures keys to be encrypted with Windows DPAPI before being persisted to
        /// storage.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="protectToLocalMachine">'true' if the key should be decryptable by any
        /// use on the local machine, 'false' if the key should only be decryptable by the current
        /// Windows user account.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// This API is only supported on Windows platforms.
        /// </remarks>
        public static IDataProtectionBuilder ProtectKeysWithDpapi(this IDataProtectionBuilder builder, bool protectToLocalMachine)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Use(builder.Services, DataProtectionServiceDescriptors.IXmlEncryptor_Dpapi(protectToLocalMachine));
            return builder;
        }

        /// <summary>
        /// Configures keys to be encrypted with Windows CNG DPAPI before being persisted
        /// to storage. The keys will be decryptable by the current Windows user account.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// See https://msdn.microsoft.com/en-us/library/windows/desktop/hh706794(v=vs.85).aspx
        /// for more information on DPAPI-NG. This API is only supported on Windows 8 / Windows Server 2012 and higher.
        /// </remarks>
        public static IDataProtectionBuilder ProtectKeysWithDpapiNG(this IDataProtectionBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ProtectKeysWithDpapiNG(
                protectionDescriptorRule: DpapiNGXmlEncryptor.GetDefaultProtectionDescriptorString(),
                flags: DpapiNGProtectionDescriptorFlags.None);
        }

        /// <summary>
        /// Configures keys to be encrypted with Windows CNG DPAPI before being persisted to storage.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="protectionDescriptorRule">The descriptor rule string with which to protect the key material.</param>
        /// <param name="flags">Flags that should be passed to the call to 'NCryptCreateProtectionDescriptor'.
        /// The default value of this parameter is <see cref="DpapiNGProtectionDescriptorFlags.None"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// See https://msdn.microsoft.com/en-us/library/windows/desktop/hh769091(v=vs.85).aspx
        /// and https://msdn.microsoft.com/en-us/library/windows/desktop/hh706800(v=vs.85).aspx
        /// for more information on valid values for the the <paramref name="protectionDescriptorRule"/>
        /// and <paramref name="flags"/> arguments.
        /// This API is only supported on Windows 8 / Windows Server 2012 and higher.
        /// </remarks>
        public static IDataProtectionBuilder ProtectKeysWithDpapiNG(this IDataProtectionBuilder builder, string protectionDescriptorRule, DpapiNGProtectionDescriptorFlags flags)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (protectionDescriptorRule == null)
            {
                throw new ArgumentNullException(nameof(protectionDescriptorRule));
            }

            Use(builder.Services, DataProtectionServiceDescriptors.IXmlEncryptor_DpapiNG(protectionDescriptorRule, flags));
            return builder;
        }

        /// <summary>
        /// Sets the default lifetime of keys created by the data protection system.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="lifetime">The lifetime (time before expiration) for newly-created keys.
        /// See <see cref="KeyManagementOptions.NewKeyLifetime"/> for more information and
        /// usage notes.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder SetDefaultKeyLifetime(this IDataProtectionBuilder builder, TimeSpan lifetime)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (lifetime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(Resources.FormatLifetimeMustNotBeNegative(nameof(lifetime)));
            }

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.NewKeyLifetime = lifetime;
            });

            return builder;
        }

        /// <summary>
        /// Configures the data protection system to use the specified cryptographic algorithms
        /// by default when generating protected payloads.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="settings">Information about what cryptographic algorithms should be used.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder UseCryptographicAlgorithms(this IDataProtectionBuilder builder, AuthenticatedEncryptionSettings settings)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return UseCryptographicAlgorithmsCore(builder, settings);
        }

        /// <summary>
        /// Configures the data protection system to use custom Windows CNG algorithms.
        /// This API is intended for advanced scenarios where the developer cannot use the
        /// algorithms specified in the <see cref="EncryptionAlgorithm"/> and
        /// <see cref="ValidationAlgorithm"/> enumerations.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="settings">Information about what cryptographic algorithms should be used.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// This API is only available on Windows.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static IDataProtectionBuilder UseCustomCryptographicAlgorithms(this IDataProtectionBuilder builder, CngCbcAuthenticatedEncryptionSettings settings)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return UseCryptographicAlgorithmsCore(builder, settings);
        }

        /// <summary>
        /// Configures the data protection system to use custom Windows CNG algorithms.
        /// This API is intended for advanced scenarios where the developer cannot use the
        /// algorithms specified in the <see cref="EncryptionAlgorithm"/> and
        /// <see cref="ValidationAlgorithm"/> enumerations.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="settings">Information about what cryptographic algorithms should be used.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// This API is only available on Windows.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static IDataProtectionBuilder UseCustomCryptographicAlgorithms(this IDataProtectionBuilder builder, CngGcmAuthenticatedEncryptionSettings settings)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return UseCryptographicAlgorithmsCore(builder, settings);
        }

        /// <summary>
        /// Configures the data protection system to use custom algorithms.
        /// This API is intended for advanced scenarios where the developer cannot use the
        /// algorithms specified in the <see cref="EncryptionAlgorithm"/> and
        /// <see cref="ValidationAlgorithm"/> enumerations.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <param name="settings">Information about what cryptographic algorithms should be used.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static IDataProtectionBuilder UseCustomCryptographicAlgorithms(this IDataProtectionBuilder builder, ManagedAuthenticatedEncryptionSettings settings)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return UseCryptographicAlgorithmsCore(builder, settings);
        }

        private static IDataProtectionBuilder UseCryptographicAlgorithmsCore(IDataProtectionBuilder builder, IInternalAuthenticatedEncryptionSettings settings)
        {
            settings.Validate(); // perform self-test
            Use(builder.Services, DataProtectionServiceDescriptors.IAuthenticatedEncryptorConfiguration_FromSettings(settings));
            return builder;
        }

        /// <summary>
        /// Configures the data protection system to use the <see cref="EphemeralDataProtectionProvider"/>
        /// for data protection services.
        /// </summary>
        /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        /// <remarks>
        /// If this option is used, payloads protected by the data protection system will
        /// be permanently undecipherable after the application exits.
        /// </remarks>
        public static IDataProtectionBuilder UseEphemeralDataProtectionProvider(this IDataProtectionBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Use(builder.Services, DataProtectionServiceDescriptors.IDataProtectionProvider_Ephemeral());
            return builder;
        }

        /*
         * UTILITY ISERVICECOLLECTION METHODS
         */

        private static void RemoveAllServicesOfType(IServiceCollection services, Type serviceType)
        {
            // We go backward since we're modifying the collection in-place.
            for (var i = services.Count - 1; i >= 0; i--)
            {
                if (services[i]?.ServiceType == serviceType)
                {
                    services.RemoveAt(i);
                }
            }
        }

        private static void Use(IServiceCollection services, ServiceDescriptor descriptor)
        {
            RemoveAllServicesOfType(services, descriptor.ServiceType);
            services.Add(descriptor);
        }
    }
}