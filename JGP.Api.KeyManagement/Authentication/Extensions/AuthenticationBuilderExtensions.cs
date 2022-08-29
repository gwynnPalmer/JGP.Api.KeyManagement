// ***********************************************************************
// Assembly         : JGP.Api.KeyManagement
// Author           : Joshua Gwynn-Palmer
// Created          : 08-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 08-29-2022
// ***********************************************************************
// <copyright file="AuthenticationBuilderExtensions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Api.KeyManagement.Authentication.Extensions
{
    using Microsoft.AspNetCore.Authentication;

    /// <summary>
    ///     Class AuthenticationBuilderExtensions.
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        ///     Adds the API key management.
        /// </summary>
        /// <param name="authenticationBuilder">The authentication builder.</param>
        /// <param name="apiKeyAuthenticationSettings">The apiKeyAuthenticationSettings.</param>
        /// <returns>AuthenticationBuilder.</returns>
        public static AuthenticationBuilder AddApiKeyManagement(this AuthenticationBuilder authenticationBuilder,
            ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthenticationSettings, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationSettings.DefaultScheme,
                options =>
                {
                    options.ServiceId = apiKeyAuthenticationSettings.ServiceId;
                    options.ServiceName = apiKeyAuthenticationSettings.ServiceName;
                });
        }
    }
}