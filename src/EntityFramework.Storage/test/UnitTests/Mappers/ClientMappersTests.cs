// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using FluentAssertions;
using ForgePoint.Identity.EntityFramework.Mappers;
using Xunit;
using Client = ForgePoint.Identity.Models.Client;

namespace ForgePoint.Identity.EntityFramework.UnitTests.Mappers
{
    public class ClientMappersTests
    {
        [Fact]
        public void Can_Map()
        {
            var model = new Client();
            var mappedEntity = model.ToEntity();
            var mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void Properties_Map()
        {
            var model = new Client()
            {
                Properties =
                {
                    {"foo1", "bar1"},
                    {"foo2", "bar2"},
                }
            };


            var mappedEntity = model.ToEntity();

            mappedEntity.Properties.Count.Should().Be(2);
            var foo1 = mappedEntity.Properties.FirstOrDefault(x => x.Key == "foo1");
            foo1.Should().NotBeNull();
            foo1.Value.Should().Be("bar1");
            var foo2 = mappedEntity.Properties.FirstOrDefault(x => x.Key == "foo2");
            foo2.Should().NotBeNull();
            foo2.Value.Should().Be("bar2");



            var mappedModel = mappedEntity.ToModel();

            mappedModel.Properties.Count.Should().Be(2);
            mappedModel.Properties.ContainsKey("foo1").Should().BeTrue();
            mappedModel.Properties.ContainsKey("foo2").Should().BeTrue();
            mappedModel.Properties["foo1"].Should().Be("bar1");
            mappedModel.Properties["foo2"].Should().Be("bar2");
        }

        [Fact]
        public void duplicates_properties_in_db_map()
        {
            var entity = new ForgePoint.Identity.EntityFramework.Entities.Client
            {
                Properties = new System.Collections.Generic.List<Entities.ClientProperty>()
                {
                    new Entities.ClientProperty{Key = "foo1", Value = "bar1"},
                    new Entities.ClientProperty{Key = "foo1", Value = "bar2"},
                }
            };

            Action modelAction = () => entity.ToModel();
            modelAction.Should().Throw<Exception>();
        }

        [Fact]
        public void missing_values_should_use_defaults()
        {
            var entity = new ForgePoint.Identity.EntityFramework.Entities.Client
            {
                ProtocolType = null,
                ClientSecrets = new System.Collections.Generic.List<Entities.ClientSecret>
                {
                    new Entities.ClientSecret
                    {
                        Type = null
                    }
                }
            };

            var def = new Client
            {
                ClientSecrets = { new Models.Secret("foo") }
            };

            var model = entity.ToModel();
            model.ProtocolType.Should().Be(def.ProtocolType);
            model.ClientSecrets.First().Type.Should().Be(def.ClientSecrets.First().Type);
        }

        [Fact]
        public void collections_and_signing_algorithms_map()
        {
            var model = new Client
            {
                ClientId = "client",
                AllowedGrantTypes = { "authorization_code" },
                AllowedScopes = { "openid", "api" },
                RedirectUris = { "https://client/callback" },
                PostLogoutRedirectUris = { "https://client/logout" },
                AllowedCorsOrigins = { "https://client" },
                IdentityProviderRestrictions = { "google" },
                AllowedIdentityTokenSigningAlgorithms = { "RS256", "PS256" },
                Claims = { new Models.ClientClaim("role", "admin") },
                IncludeJwtId = true,
                AccessTokenType = Models.AccessTokenType.Reference,
                RefreshTokenUsage = Models.TokenUsage.ReUse,
                RefreshTokenExpiration = Models.TokenExpiration.Sliding
            };

            var mappedEntity = model.ToEntity();
            mappedEntity.AllowedGrantTypes.Select(x => x.GrantType).Should().BeEquivalentTo(new[] { "authorization_code" });
            mappedEntity.AllowedScopes.Select(x => x.Scope).Should().BeEquivalentTo(new[] { "openid", "api" });
            mappedEntity.RedirectUris.Select(x => x.RedirectUri).Should().BeEquivalentTo(new[] { "https://client/callback" });
            mappedEntity.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri).Should().BeEquivalentTo(new[] { "https://client/logout" });
            mappedEntity.AllowedCorsOrigins.Select(x => x.Origin).Should().BeEquivalentTo(new[] { "https://client" });
            mappedEntity.IdentityProviderRestrictions.Select(x => x.Provider).Should().BeEquivalentTo(new[] { "google" });
            mappedEntity.AllowedIdentityTokenSigningAlgorithms.Should().Be("RS256,PS256");
            mappedEntity.Claims.Should().ContainSingle(x => x.Type == "role" && x.Value == "admin");
            mappedEntity.IncludeJwtId.Should().BeTrue();
            mappedEntity.AccessTokenType.Should().Be((int)Models.AccessTokenType.Reference);
            mappedEntity.RefreshTokenUsage.Should().Be((int)Models.TokenUsage.ReUse);
            mappedEntity.RefreshTokenExpiration.Should().Be((int)Models.TokenExpiration.Sliding);

            var mappedModel = mappedEntity.ToModel();
            mappedModel.ClientId.Should().Be("client");
            mappedModel.AllowedGrantTypes.Should().BeEquivalentTo(new[] { "authorization_code" });
            mappedModel.AllowedScopes.Should().BeEquivalentTo(new[] { "openid", "api" });
            mappedModel.RedirectUris.Should().BeEquivalentTo(new[] { "https://client/callback" });
            mappedModel.PostLogoutRedirectUris.Should().BeEquivalentTo(new[] { "https://client/logout" });
            mappedModel.AllowedCorsOrigins.Should().BeEquivalentTo(new[] { "https://client" });
            mappedModel.IdentityProviderRestrictions.Should().BeEquivalentTo(new[] { "google" });
            mappedModel.AllowedIdentityTokenSigningAlgorithms.Should().BeEquivalentTo(new[] { "RS256", "PS256" });
            mappedModel.Claims.Should().ContainSingle(x => x.Type == "role" && x.Value == "admin" && x.ValueType == System.Security.Claims.ClaimValueTypes.String);
            mappedModel.IncludeJwtId.Should().BeTrue();
            mappedModel.AccessTokenType.Should().Be(Models.AccessTokenType.Reference);
            mappedModel.RefreshTokenUsage.Should().Be(Models.TokenUsage.ReUse);
            mappedModel.RefreshTokenExpiration.Should().Be(Models.TokenExpiration.Sliding);
        }
    }
}