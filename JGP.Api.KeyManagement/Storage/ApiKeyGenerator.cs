﻿// ***********************************************************************
// Assembly         : JGP.Api.KeyManagement
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyGenerator.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Api.KeyManagement.Storage
{
    using System.Security.Cryptography;

    /// <summary>
    ///     Interface IApiKeyGenerator
    ///     Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IApiKeyGenerator : IDisposable
    {
        /// <summary>
        ///     Generates the API key.
        /// </summary>
        /// <returns>System.String.</returns>
        string GenerateApiKey();
    }

    /// <summary>
    ///     Class ApiKeyGenerator.
    ///     Implements the <see cref="IApiKeyGenerator" />
    /// </summary>
    /// <seealso cref="IApiKeyGenerator" />
    public class ApiKeyGenerator : IApiKeyGenerator
    {
        /// <summary>
        ///     The generator.
        /// </summary>
        private readonly RandomNumberGenerator _generator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyGenerator" /> class.
        /// </summary>
        public ApiKeyGenerator()
        {
            _generator = RandomNumberGenerator.Create();
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            _generator.Dispose();
        }

        /// <summary>
        ///     Generates the API key.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GenerateApiKey()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            return "JGP-" + Convert.ToBase64String(bytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "")
                .Substring(0, 32);
        }
    }
}