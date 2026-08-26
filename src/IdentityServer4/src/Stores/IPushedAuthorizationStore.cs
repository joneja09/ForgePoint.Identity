// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using ForgePoint.Identity.Models;

namespace ForgePoint.Identity.Stores
{
    /// <summary>
    /// Persistence for OAuth 2.0 Pushed Authorization Requests (RFC 9126).
    /// </summary>
    public interface IPushedAuthorizationStore
    {
        /// <summary>
        /// Stores a pushed authorization request and returns the request_uri.
        /// </summary>
        /// <param name="request">The request to store.</param>
        /// <returns>The request_uri bound to this request.</returns>
        Task<string> StoreAsync(PushedAuthorizationRequest request);

        /// <summary>
        /// Loads a previously pushed authorization request.
        /// </summary>
        /// <param name="requestUri">The request_uri returned from <see cref="StoreAsync"/>.</param>
        Task<PushedAuthorizationRequest> GetAsync(string requestUri);

        /// <summary>
        /// Removes a pushed authorization request (PAR requests are single-use).
        /// </summary>
        /// <param name="requestUri">The request_uri to remove.</param>
        Task RemoveAsync(string requestUri);
    }
}
