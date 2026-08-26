// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Endpoints
{
    /// <summary>
    /// OAuth 2.0 Pushed Authorization Request endpoint (RFC 9126).
    /// </summary>
    internal class PushedAuthorizationEndpoint : IEndpointHandler
    {
        private readonly IClientSecretValidator _clientValidator;
        private readonly IPushedAuthorizationStore _store;
        private readonly IdentityServerOptions _options;
        private readonly ILogger _logger;

        public PushedAuthorizationEndpoint(
            IClientSecretValidator clientValidator,
            IPushedAuthorizationStore store,
            IdentityServerOptions options,
            ILogger<PushedAuthorizationEndpoint> logger)
        {
            _clientValidator = clientValidator;
            _store = store;
            _options = options;
            _logger = logger;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            _logger.LogDebug("Start pushed authorization request.");

            if (!HttpMethods.IsPost(context.Request.Method) || !context.Request.HasApplicationFormContentType())
            {
                _logger.LogWarning("Invalid HTTP request for PAR endpoint");
                return Error(OidcConstants.AuthorizeErrors.InvalidRequest, "Invalid HTTP request");
            }

            var clientResult = await _clientValidator.ValidateAsync(context);
            if (clientResult.Client == null)
            {
                return Error(OidcConstants.TokenErrors.InvalidClient, "Invalid client");
            }

            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();

            if (form.Get(OidcConstants.AuthorizeRequest.RequestUri).IsPresent())
            {
                _logger.LogError("request_uri is not allowed at the PAR endpoint");
                return Error(OidcConstants.AuthorizeErrors.InvalidRequest, "request_uri is not allowed at the PAR endpoint");
            }

            var clientId = form.Get(OidcConstants.AuthorizeRequest.ClientId);
            if (clientId.IsPresent() && clientId != clientResult.Client.ClientId)
            {
                _logger.LogError("client_id in body does not match authenticated client");
                return Error(OidcConstants.AuthorizeErrors.InvalidRequest, "client_id does not match authenticated client");
            }

            if (clientId.IsMissing())
            {
                form[OidcConstants.AuthorizeRequest.ClientId] = clientResult.Client.ClientId;
            }

            // never persist the client secret with the authorization request
            form.Remove("client_secret");

            var parameters = new Dictionary<string, string>();
            foreach (var key in form.AllKeys)
            {
                if (key != null)
                {
                    parameters[key] = form[key];
                }
            }

            var requestUri = await _store.StoreAsync(new PushedAuthorizationRequest
            {
                ClientId = clientResult.Client.ClientId,
                Parameters = parameters
            });

            _logger.LogDebug("Issued pushed authorization request_uri for client {clientId}", clientResult.Client.ClientId);

            return new PushedAuthorizationResult(requestUri, _options.PushedAuthorization.Lifetime);
        }

        private TokenErrorResult Error(string error, string errorDescription = null)
        {
            return new TokenErrorResult(new ResponseHandling.TokenErrorResponse
            {
                Error = error,
                ErrorDescription = errorDescription
            });
        }
    }
}
