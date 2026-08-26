// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace ForgePoint.Identity.Models
{
    /// <summary>
    /// A stored OAuth 2.0 Pushed Authorization Request (RFC 9126).
    /// </summary>
    public class PushedAuthorizationRequest
    {
        /// <summary>
        /// The client that created the request.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Authorization parameters captured at the PAR endpoint.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
