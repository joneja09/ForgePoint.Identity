// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using ForgePoint.Identity.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Health check registration helpers for IdentityServer.
    /// </summary>
    public static class IdentityServerHealthCheckBuilderExtensions
    {
        /// <summary>
        /// Adds a health check that verifies IdentityServer signing material is available.
        /// </summary>
        /// <param name="builder">The health checks builder.</param>
        /// <param name="name">The health check name. Defaults to "identityserver".</param>
        /// <param name="tags">Optional tags.</param>
        /// <returns>The health checks builder.</returns>
        public static IHealthChecksBuilder AddIdentityServer(this IHealthChecksBuilder builder, string name = "identityserver", params string[] tags)
        {
            return builder.AddCheck<IdentityServerHealthCheck>(name, tags: tags);
        }
    }
}
