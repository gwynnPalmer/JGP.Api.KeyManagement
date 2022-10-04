// ***********************************************************************
// Assembly         : JGP.Api.KeyManagement
// Author           : Joshua Gwynn-Palmer
// Created          : 08-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 08-29-2022
// ***********************************************************************
// <copyright file="ConfigurationExtensions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Api.KeyManagement.Authentication.Extensions;

using Microsoft.Extensions.Configuration;

/// <summary>
///     Class ConfigurationExtensions.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Configures the API key authentication settings.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>ApiKeyAuthenticationSettings.</returns>
    internal static ApiKeyAuthenticationSettings ConfigureApiKeyAuthenticationSettings(this IConfiguration configuration)
    {
        // Setup Settings.
        var apiKeyAuthenticationSettings = new ApiKeyAuthenticationSettings();
        configuration.GetSection(ApiKeyAuthenticationSettings.ConfigurationSectionName)
            .Bind(apiKeyAuthenticationSettings);

        // Setup Environmental Variables.
        var serviceId = Environment.GetEnvironmentVariable("ServiceId");
        var serviceName = Environment.GetEnvironmentVariable("ServiceName");
        var url = Environment.GetEnvironmentVariable("Url");

        apiKeyAuthenticationSettings.ServiceId = string.IsNullOrEmpty(serviceId)
            ? apiKeyAuthenticationSettings.ServiceId
            : Guid.Parse(serviceId);

        apiKeyAuthenticationSettings.ServiceName = string.IsNullOrEmpty(serviceName)
            ? apiKeyAuthenticationSettings.ServiceName
            : serviceName;

        apiKeyAuthenticationSettings.Url = string.IsNullOrEmpty(url)
            ? apiKeyAuthenticationSettings.Url
            : url;

        return apiKeyAuthenticationSettings;
    }
}