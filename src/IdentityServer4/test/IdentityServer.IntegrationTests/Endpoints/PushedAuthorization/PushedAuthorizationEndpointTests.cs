// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.IntegrationTests.Common;
using ForgePoint.Identity.Models;
using ForgePoint.Identity.Test;
using Xunit;

namespace IdentityServer.IntegrationTests.Endpoints.PushedAuthorization
{
    public class PushedAuthorizationEndpointTests
    {
        private const string Category = "Pushed Authorization endpoint";

        private readonly IdentityServerPipeline _pipeline = new IdentityServerPipeline();

        public PushedAuthorizationEndpointTests()
        {
            _pipeline.Clients.Add(new Client
            {
                ClientId = "client1",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = false,
                RedirectUris = { "https://client1/callback" },
                AllowedScopes = { "openid" }
            });

            _pipeline.IdentityScopes.Add(new IdentityResources.OpenId());
            _pipeline.Users.Add(new TestUser
            {
                SubjectId = "bob",
                Username = "bob",
                Password = "bob"
            });

            _pipeline.Initialize();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task discovery_should_advertise_par_endpoint()
        {
            var result = await _pipeline.BackChannelClient.GetAsync("https://server/.well-known/openid-configuration");
            var json = await result.Content.ReadAsStringAsync();
            var data = Newtonsoft.Json.Linq.JObject.Parse(json);
            data["pushed_authorization_request_endpoint"].ToString().Should().Be("https://server/connect/par");
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task par_should_return_request_uri_and_authorize_should_consume_it()
        {
            var parResponse = await _pipeline.BackChannelClient.RequestPushedAuthorizationAsync();
            parResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var json = await parResponse.Content.ReadAsStringAsync();
            json.Should().Contain("urn:ietf:params:oauth:request_uri:");
            json.Should().Contain("expires_in");

            var requestUri = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("request_uri").GetString();
            var authorizeUrl = IdentityServerPipeline.AuthorizeEndpoint +
                               "?client_id=client1&request_uri=" + System.Net.WebUtility.UrlEncode(requestUri);

            _pipeline.BrowserClient.AllowAutoRedirect = false;
            var result = await _pipeline.BrowserClient.GetAsync(authorizeUrl);
            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
            result.Headers.Location.ToString().ToLowerInvariant().Should().Contain("/account/login");
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task par_request_uri_is_single_use()
        {
            var parResponse = await _pipeline.BackChannelClient.RequestPushedAuthorizationAsync();
            var json = await parResponse.Content.ReadAsStringAsync();
            var requestUri = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("request_uri").GetString();

            var authorizeUrl = IdentityServerPipeline.AuthorizeEndpoint +
                               "?client_id=client1&request_uri=" + System.Net.WebUtility.UrlEncode(requestUri);

            _pipeline.BrowserClient.AllowAutoRedirect = true;
            var first = await _pipeline.BrowserClient.GetAsync(authorizeUrl);
            first.StatusCode.Should().Be(HttpStatusCode.OK);

            var second = await _pipeline.BrowserClient.GetAsync(authorizeUrl);
            _pipeline.ErrorWasCalled.Should().BeTrue();
            _pipeline.ErrorMessage.Error.Should().Be("invalid_request_uri");
        }
    }

    internal static class PushedAuthorizationClientExtensions
    {
        public static Task<HttpResponseMessage> RequestPushedAuthorizationAsync(this HttpClient client)
        {
            return client.PostAsync("https://server/connect/par", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", "client1" },
                { "client_secret", "secret" },
                { "response_type", "code" },
                { "redirect_uri", "https://client1/callback" },
                { "scope", "openid" }
            }));
        }
    }
}
