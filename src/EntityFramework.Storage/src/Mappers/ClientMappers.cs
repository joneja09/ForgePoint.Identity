// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using ForgePoint.Identity.Models;

namespace ForgePoint.Identity.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for clients.
    /// </summary>
    public static class ClientMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.Client ToModel(this Entities.Client entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new Models.Client
            {
                Enabled = entity.Enabled,
                ClientId = entity.ClientId,
                RequireClientSecret = entity.RequireClientSecret,
                ClientName = entity.ClientName,
                Description = entity.Description,
                ClientUri = entity.ClientUri,
                LogoUri = entity.LogoUri,
                RequireConsent = entity.RequireConsent,
                AllowRememberConsent = entity.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = entity.AlwaysIncludeUserClaimsInIdToken,
                AllowedGrantTypes = entity.AllowedGrantTypes == null
                    ? Array.Empty<string>()
                    : entity.AllowedGrantTypes.Select(x => x.GrantType).ToList(),
                RequirePkce = entity.RequirePkce,
                AllowPlainTextPkce = entity.AllowPlainTextPkce,
                RequireRequestObject = entity.RequireRequestObject,
                RequirePushedAuthorization = entity.RequirePushedAuthorization,
                AllowAccessTokensViaBrowser = entity.AllowAccessTokensViaBrowser,
                RedirectUris = MappingHelpers.MapStrings(entity.RedirectUris, x => x.RedirectUri),
                PostLogoutRedirectUris = MappingHelpers.MapStrings(entity.PostLogoutRedirectUris, x => x.PostLogoutRedirectUri),
                FrontChannelLogoutUri = entity.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired = entity.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri = entity.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = entity.BackChannelLogoutSessionRequired,
                AllowOfflineAccess = entity.AllowOfflineAccess,
                AllowedScopes = MappingHelpers.MapStrings(entity.AllowedScopes, x => x.Scope),
                IdentityTokenLifetime = entity.IdentityTokenLifetime,
                AllowedIdentityTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.Convert(entity.AllowedIdentityTokenSigningAlgorithms),
                AccessTokenLifetime = entity.AccessTokenLifetime,
                AuthorizationCodeLifetime = entity.AuthorizationCodeLifetime,
                ConsentLifetime = entity.ConsentLifetime,
                AbsoluteRefreshTokenLifetime = entity.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime = entity.SlidingRefreshTokenLifetime,
                RefreshTokenUsage = (TokenUsage)entity.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = entity.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration = (TokenExpiration)entity.RefreshTokenExpiration,
                AccessTokenType = (AccessTokenType)entity.AccessTokenType,
                EnableLocalLogin = entity.EnableLocalLogin,
                IdentityProviderRestrictions = MappingHelpers.MapStrings(entity.IdentityProviderRestrictions, x => x.Provider),
                IncludeJwtId = entity.IncludeJwtId,
                Claims = MappingHelpers.MapList(entity.Claims, x => new ClientClaim(x.Type, x.Value, ClaimValueTypes.String)),
                AlwaysSendClientClaims = entity.AlwaysSendClientClaims,
                ClientClaimsPrefix = entity.ClientClaimsPrefix,
                PairWiseSubjectSalt = entity.PairWiseSubjectSalt,
                AllowedCorsOrigins = MappingHelpers.MapStrings(entity.AllowedCorsOrigins, x => x.Origin),
                Properties = MappingHelpers.MapProperties(entity.Properties),
                UserSsoLifetime = entity.UserSsoLifetime,
                UserCodeType = entity.UserCodeType,
                DeviceCodeLifetime = entity.DeviceCodeLifetime,
                ClientSecrets = MappingHelpers.MapList(entity.ClientSecrets, MappingHelpers.ToSecret)
            };

            if (entity.ProtocolType != null)
            {
                model.ProtocolType = entity.ProtocolType;
            }

            return model;
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.Client ToEntity(this Models.Client model)
        {
            if (model == null)
            {
                return null;
            }

            return new Entities.Client
            {
                Enabled = model.Enabled,
                ClientId = model.ClientId,
                ProtocolType = model.ProtocolType,
                RequireClientSecret = model.RequireClientSecret,
                ClientName = model.ClientName,
                Description = model.Description,
                ClientUri = model.ClientUri,
                LogoUri = model.LogoUri,
                RequireConsent = model.RequireConsent,
                AllowRememberConsent = model.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = model.AlwaysIncludeUserClaimsInIdToken,
                AllowedGrantTypes = MappingHelpers.MapList(model.AllowedGrantTypes, x => new Entities.ClientGrantType { GrantType = x }),
                RequirePkce = model.RequirePkce,
                AllowPlainTextPkce = model.AllowPlainTextPkce,
                RequireRequestObject = model.RequireRequestObject,
                RequirePushedAuthorization = model.RequirePushedAuthorization,
                AllowAccessTokensViaBrowser = model.AllowAccessTokensViaBrowser,
                RedirectUris = MappingHelpers.MapList(model.RedirectUris, x => new Entities.ClientRedirectUri { RedirectUri = x }),
                PostLogoutRedirectUris = MappingHelpers.MapList(model.PostLogoutRedirectUris, x => new Entities.ClientPostLogoutRedirectUri { PostLogoutRedirectUri = x }),
                FrontChannelLogoutUri = model.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired = model.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri = model.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = model.BackChannelLogoutSessionRequired,
                AllowOfflineAccess = model.AllowOfflineAccess,
                AllowedScopes = MappingHelpers.MapList(model.AllowedScopes, x => new Entities.ClientScope { Scope = x }),
                IdentityTokenLifetime = model.IdentityTokenLifetime,
                AllowedIdentityTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.Convert(model.AllowedIdentityTokenSigningAlgorithms),
                AccessTokenLifetime = model.AccessTokenLifetime,
                AuthorizationCodeLifetime = model.AuthorizationCodeLifetime,
                ConsentLifetime = model.ConsentLifetime,
                AbsoluteRefreshTokenLifetime = model.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime = model.SlidingRefreshTokenLifetime,
                RefreshTokenUsage = (int)model.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = model.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration = (int)model.RefreshTokenExpiration,
                AccessTokenType = (int)model.AccessTokenType,
                EnableLocalLogin = model.EnableLocalLogin,
                IdentityProviderRestrictions = MappingHelpers.MapList(model.IdentityProviderRestrictions, x => new Entities.ClientIdPRestriction { Provider = x }),
                IncludeJwtId = model.IncludeJwtId,
                Claims = MappingHelpers.MapList(model.Claims, x => new Entities.ClientClaim { Type = x.Type, Value = x.Value }),
                AlwaysSendClientClaims = model.AlwaysSendClientClaims,
                ClientClaimsPrefix = model.ClientClaimsPrefix,
                PairWiseSubjectSalt = model.PairWiseSubjectSalt,
                AllowedCorsOrigins = MappingHelpers.MapList(model.AllowedCorsOrigins, x => new Entities.ClientCorsOrigin { Origin = x }),
                Properties = MappingHelpers.MapProperties<Entities.ClientProperty>(model.Properties),
                UserSsoLifetime = model.UserSsoLifetime,
                UserCodeType = model.UserCodeType,
                DeviceCodeLifetime = model.DeviceCodeLifetime,
                ClientSecrets = MappingHelpers.MapList(model.ClientSecrets, MappingHelpers.ToEntitySecret<Entities.ClientSecret>)
            };
        }
    }
}
