using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using IdentityModel;
using IdentityServer.UnitTests.Common;
using ForgePoint.Identity.Configuration;
using ForgePoint.Identity.Extensions;
using ForgePoint.Identity.Models;
using Xunit;

namespace IdentityServer.UnitTests.Extensions
{
    public class JwtPayloadCreationTests
    {
        private Token _token;
        
        public JwtPayloadCreationTests()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Scope, "scope1"),
                new Claim(JwtClaimTypes.Scope, "scope2"),
                new Claim(JwtClaimTypes.Scope, "scope3"),
            };
            
            _token = new Token(OidcConstants.TokenTypes.AccessToken)
            {
                CreationTime = DateTime.UtcNow,
                Issuer = "issuer",
                Lifetime = 60,
                Claims = claims.Distinct(new ClaimComparer()).ToList(),
                ClientId = "client"
            };
        }
        
        [Fact]
        public void Should_create_scopes_as_array_by_default()
        {
            var options = new IdentityServerOptions();
            var payload = _token.CreateJwtPayload(new StubClock(), options, TestLogger.Create<JwtPayloadCreationTests>());

            payload.Should().NotBeNull();
            var scopes = payload.Claims.Where(c => c.Type == JwtClaimTypes.Scope).ToArray();
            scopes.Count().Should().Be(3);
            scopes[0].Value.Should().Be("scope1");
            scopes[1].Value.Should().Be("scope2");
            scopes[2].Value.Should().Be("scope3");
        }
        
        [Fact]
        public void Should_create_scopes_as_string()
        {
            var options = new IdentityServerOptions
            {
                EmitScopesAsSpaceDelimitedStringInJwt = true
            };
            
            var payload = _token.CreateJwtPayload(new StubClock(), options, TestLogger.Create<JwtPayloadCreationTests>());

            payload.Should().NotBeNull();
            var scopes = payload.Claims.Where(c => c.Type == JwtClaimTypes.Scope).ToList();
            scopes.Count().Should().Be(1);
            scopes.First().Value.Should().Be("scope1 scope2 scope3");
        }

        [Fact]
        public void Should_serialize_json_confirmation_claim()
        {
            _token.Confirmation = "{\"x5t#S256\":\"foo\"}";

            var payload = _token.CreateJwtPayload(new StubClock(), new IdentityServerOptions(), TestLogger.Create<JwtPayloadCreationTests>());

            payload.Should().NotBeNull();
            payload.ContainsKey(JwtClaimTypes.Confirmation).Should().BeTrue();
            payload[JwtClaimTypes.Confirmation].ToString().Should().Contain("x5t#S256");
        }
    }
}
