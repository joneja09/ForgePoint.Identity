// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IdentityServer4.Hosting
{
    /// <summary>
    /// Health check that verifies IdentityServer has signing credentials configured.
    /// </summary>
    public class IdentityServerHealthCheck : IHealthCheck
    {
        private readonly IKeyMaterialService _keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerHealthCheck"/> class.
        /// </summary>
        public IdentityServerHealthCheck(IKeyMaterialService keys)
        {
            _keys = keys;
        }

        /// <inheritdoc />
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var credential = await _keys.GetSigningCredentialsAsync();
            if (credential == null)
            {
                return HealthCheckResult.Unhealthy("IdentityServer has no signing credentials configured.");
            }

            return HealthCheckResult.Healthy("IdentityServer is ready.");
        }
    }
}
