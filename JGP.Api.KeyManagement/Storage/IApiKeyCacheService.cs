// ***********************************************************************
// Assembly         : JGP.Api.KeyManagement
// Author           : Joshua Gwynn-Palmer
// Created          : 07-31-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-31-2022
// ***********************************************************************
// <copyright file="IApiKeyCacheService.cs" company="JGP.Api.KeyManagement">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Api.KeyManagement.Storage;

/// <summary>
///     Interface IApiKeyCacheService
///     Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IApiKeyCacheService : IDisposable
{
    /// <summary>
    ///     Gets the service asynchronous.
    /// </summary>
    /// <param name="serviceId">The service identifier.</param>
    /// <param name="serviceName">Name of the service.</param>
    /// <returns>ValueTask&lt;ServiceRecord&gt;.</returns>
    ValueTask<ServiceRecord> GetServiceAsync(Guid serviceId, string serviceName);

    /// <summary>
    ///     Gets the services asynchronous.
    /// </summary>
    /// <returns>ValueTask&lt;List&lt;ServiceRecord&gt;&gt;.</returns>
    ValueTask<List<ServiceRecord>> GetServicesAsync();
}