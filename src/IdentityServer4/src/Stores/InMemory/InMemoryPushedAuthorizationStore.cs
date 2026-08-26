// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// In-memory store for pushed authorization requests. Intended for tests and development.
    /// </summary>
    public class InMemoryPushedAuthorizationStore : IPushedAuthorizationStore
    {
        private readonly ConcurrentDictionary<string, (PushedAuthorizationRequest Request, DateTimeOffset Expires)> _items =
            new ConcurrentDictionary<string, (PushedAuthorizationRequest, DateTimeOffset)>();

        private readonly IHandleGenerationService _handles;
        private readonly IClock _clock;
        private readonly IdentityServerOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryPushedAuthorizationStore"/> class.
        /// </summary>
        public InMemoryPushedAuthorizationStore(
            IHandleGenerationService handles = null,
            IClock clock = null,
            IdentityServerOptions options = null)
        {
            _handles = handles ?? new DefaultHandleGenerationService();
            _clock = clock ?? new DefaultClock();
            _options = options ?? new IdentityServerOptions();
        }

        /// <inheritdoc />
        public async Task<string> StoreAsync(PushedAuthorizationRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var handle = await _handles.GenerateAsync();
            var expires = _clock.UtcNow.AddSeconds(_options.PushedAuthorization.Lifetime);
            _items[handle] = (request, expires);
            return Constants.PushedAuthorizationRequestUriPrefix + handle;
        }

        /// <inheritdoc />
        public Task<PushedAuthorizationRequest> GetAsync(string requestUri)
        {
            var handle = ExtractHandle(requestUri);
            if (handle != null && _items.TryGetValue(handle, out var item))
            {
                if (item.Expires >= _clock.UtcNow)
                {
                    return Task.FromResult(item.Request);
                }

                _items.TryRemove(handle, out _);
            }

            return Task.FromResult<PushedAuthorizationRequest>(null);
        }

        /// <inheritdoc />
        public Task RemoveAsync(string requestUri)
        {
            var handle = ExtractHandle(requestUri);
            if (handle != null)
            {
                _items.TryRemove(handle, out _);
            }

            return Task.CompletedTask;
        }

        private static string ExtractHandle(string requestUri)
        {
            if (requestUri.IsMissing()) return null;
            if (!requestUri.StartsWith(Constants.PushedAuthorizationRequestUriPrefix, StringComparison.Ordinal)) return null;
            return requestUri.Substring(Constants.PushedAuthorizationRequestUriPrefix.Length);
        }
    }
}
