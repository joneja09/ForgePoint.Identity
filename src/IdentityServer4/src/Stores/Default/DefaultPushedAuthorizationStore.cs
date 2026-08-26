// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using ForgePoint.Identity.Configuration;
using ForgePoint.Identity.Extensions;
using ForgePoint.Identity.Models;
using ForgePoint.Identity.Services;
using ForgePoint.Identity.Stores.Serialization;
using Microsoft.Extensions.Logging;

namespace ForgePoint.Identity.Stores
{
    /// <summary>
    /// Default PAR store backed by <see cref="IPersistedGrantStore"/>.
    /// </summary>
    public class DefaultPushedAuthorizationStore : DefaultGrantStore<PushedAuthorizationRequest>, IPushedAuthorizationStore
    {
        private readonly IClock _clock;
        private readonly IdentityServerOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPushedAuthorizationStore"/> class.
        /// </summary>
        public DefaultPushedAuthorizationStore(
            IPersistedGrantStore store,
            IPersistentGrantSerializer serializer,
            IHandleGenerationService handleGenerationService,
            IClock clock,
            IdentityServerOptions options,
            ILogger<DefaultPushedAuthorizationStore> logger)
            : base(IdentityServerConstants.PersistedGrantTypes.PushedAuthorization, store, serializer, handleGenerationService, logger)
        {
            _clock = clock;
            _options = options;
        }

        /// <inheritdoc />
        public async Task<string> StoreAsync(PushedAuthorizationRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.ClientId.IsMissing()) throw new ArgumentException("ClientId is required", nameof(request));

            var lifetime = _options.PushedAuthorization.Lifetime;
            var handle = await CreateItemAsync(
                request,
                request.ClientId,
                subjectId: null,
                sessionId: null,
                description: "par",
                created: _clock.UtcNow.UtcDateTime,
                lifetime: lifetime);

            return Constants.PushedAuthorizationRequestUriPrefix + handle;
        }

        /// <inheritdoc />
        public Task<PushedAuthorizationRequest> GetAsync(string requestUri)
        {
            var handle = ExtractHandle(requestUri);
            if (handle == null)
            {
                return Task.FromResult<PushedAuthorizationRequest>(null);
            }

            return GetItemAsync(handle);
        }

        /// <inheritdoc />
        public Task RemoveAsync(string requestUri)
        {
            var handle = ExtractHandle(requestUri);
            if (handle == null)
            {
                return Task.CompletedTask;
            }

            return RemoveItemAsync(handle);
        }

        private static string ExtractHandle(string requestUri)
        {
            if (requestUri.IsMissing())
            {
                return null;
            }

            if (!requestUri.StartsWith(Constants.PushedAuthorizationRequestUriPrefix, StringComparison.Ordinal))
            {
                return null;
            }

            return requestUri.Substring(Constants.PushedAuthorizationRequestUriPrefix.Length);
        }
    }
}
