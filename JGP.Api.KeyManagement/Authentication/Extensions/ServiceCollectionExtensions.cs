// ***********************************************************************
// Assembly         : JGP.Api.KeyManagement
// Author           : Joshua Gwynn-Palmer
// Created          : 08-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 08-29-2022
// ***********************************************************************
// <copyright file="ServiceCollectionExtensions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Api.KeyManagement.Authentication.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Storage;

    /// <summary>
    ///     Class ServiceCollectionExtensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the API key management.
        ///     To be used by the API to register the required services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddApiKeyManagement(this IServiceCollection services, IConfiguration configuration)
        {
            var apiKeyAuthenticationSettings = configuration.ConfigureApiKeyAuthenticationSettings();

            // Add Settings.
            services.AddSingleton(apiKeyAuthenticationSettings);

            // Setup KeyStore Context.
            services.RegisterTransientKeyStoreContext(configuration);

            // Setup additional Services.
            services.AddMemoryCache();
            services.AddTransient<IApiKeyCacheService, ApiKeyCacheService>();

            // Ensure Key exists!
            services.EnsureKeyExists(apiKeyAuthenticationSettings);
        }

        /// <summary>
        ///     Registers the API key options.
        ///     To be used by the consumer of the API Services;
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void RegisterApiKeyOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterTransientKeyStoreContext(configuration);

            var context = services.GetKeyStoreContext();
            var records = context.Services.AsNoTracking().Select(service => new ServiceRecord(service)).ToList();
            services.AddSingleton(_ => Options.Create(new ApiConfiguration { Services = records }));
        }

        /// <summary>
        ///     Ensures the key exists.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="apiKeyAuthenticationSettings">The apiKeyAuthenticationSettings.</param>
        /// <exception cref="System.ArgumentException">
        ///     ApiKeyAuthenticationSettings are not configured! ServiceName and ServiceId
        ///     are required.
        /// </exception>
        private static void EnsureKeyExists(this IServiceCollection services,
            ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
        {
            if (string.IsNullOrEmpty(apiKeyAuthenticationSettings.ServiceName) ||
                apiKeyAuthenticationSettings.ServiceId == Guid.Empty)
            {
                throw new ArgumentException(
                    "ApiKeyAuthenticationSettings are not configured! ServiceName and ServiceId are required.");
            }

            var context = services.GetKeyStoreContext();
            if (context.ServiceExists(apiKeyAuthenticationSettings)) return;

            context.AddService(apiKeyAuthenticationSettings);
        }

        /// <summary>
        ///     Gets the key store context.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IKeyStoreContext.</returns>
        /// <exception cref="System.ArgumentException">KeyStoreContext is not configured!</exception>
        private static IKeyStoreContext GetKeyStoreContext(this IServiceCollection services)
        {
            var context = services.BuildServiceProvider().GetService<IKeyStoreContext>();
            if (context == null)
            {
                throw new ArgumentException("KeyStoreContext is not configured!");
            }

            return context;
        }

        /// <summary>
        ///     Registers the transient key store context.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        private static void RegisterTransientKeyStoreContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Setup KeyStore Context.
            var keyStoreConnectionString = configuration.GetConnectionString("KeyStore");
            services.AddDbContext<KeyStoreContext>(options => options.UseSqlServer(keyStoreConnectionString,
                builder => builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));
            services.AddTransient<IKeyStoreContext, KeyStoreContext>();
        }
    }
}