// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Configuration
{
    /// <summary>
    /// Options for OAuth 2.0 Pushed Authorization Requests (RFC 9126).
    /// </summary>
    public class PushedAuthorizationOptions
    {
        /// <summary>
        /// Lifetime of a pushed authorization request in seconds. Defaults to 60.
        /// </summary>
        public int Lifetime { get; set; } = 60;

        /// <summary>
        /// When true, every client must use pushed authorization requests. Defaults to false.
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
