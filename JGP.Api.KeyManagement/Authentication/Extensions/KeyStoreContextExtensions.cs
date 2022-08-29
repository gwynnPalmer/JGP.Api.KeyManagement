// ***********************************************************************
// Assembly         : JGP.Api.KeyManagement
// Author           : Joshua Gwynn-Palmer
// Created          : 08-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 08-29-2022
// ***********************************************************************
// <copyright file="KeyStoreContextExtensions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Api.KeyManagement.Authentication.Extensions
{
    using Storage;

    /// <summary>
    ///     Class KeyStoreContextExtensions.
    /// </summary>
    public static class KeyStoreContextExtensions
    {
        /// <summary>
        ///     Adds the service.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apiKeyAuthenticationSettings">The API key authentication settings.</param>
        internal static void AddService(this IKeyStoreContext context,
            ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
        {
            using var generator = new ApiKeyGenerator();
            var service = new Service
            {
                ServiceId = apiKeyAuthenticationSettings.ServiceId,
                ServiceName = apiKeyAuthenticationSettings.ServiceName,
                ApiKey = generator.GenerateApiKey()
            };
            context.Services.Add(service);
            context.SaveChanges();
        }

        /// <summary>
        ///     Services the exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apiKeyAuthenticationSettings">The API key authentication settings.</param>
        /// <returns><c>true</c> if Service Exists, <c>false</c> otherwise.</returns>
        internal static bool ServiceExists(this IKeyStoreContext context,
            ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
        {
            var service = context.Services
                .FirstOrDefault(k => k.ServiceId == apiKeyAuthenticationSettings.ServiceId);
            return service != null;
        }
    }
}