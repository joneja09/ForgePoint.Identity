// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer4.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for API resources.
    /// </summary>
    public static class ApiResourceMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.ApiResource ToModel(this ApiResource entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Models.ApiResource
            {
                Enabled = entity.Enabled,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                AllowedAccessTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.Convert(entity.AllowedAccessTokenSigningAlgorithms),
                ApiSecrets = MappingHelpers.MapList(entity.Secrets, MappingHelpers.ToSecret),
                Scopes = MappingHelpers.MapStrings(entity.Scopes, x => x.Scope),
                UserClaims = MappingHelpers.MapStrings(entity.UserClaims, x => x.Type),
                Properties = MappingHelpers.MapProperties(entity.Properties)
            };
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ApiResource ToEntity(this Models.ApiResource model)
        {
            if (model == null)
            {
                return null;
            }

            return new ApiResource
            {
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                AllowedAccessTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.Convert(model.AllowedAccessTokenSigningAlgorithms),
                Secrets = MappingHelpers.MapList(model.ApiSecrets, MappingHelpers.ToEntitySecret<ApiResourceSecret>),
                Scopes = MappingHelpers.MapList(model.Scopes, x => new ApiResourceScope { Scope = x }),
                UserClaims = MappingHelpers.MapList(model.UserClaims, x => new ApiResourceClaim { Type = x }),
                Properties = MappingHelpers.MapProperties<ApiResourceProperty>(model.Properties)
            };
        }
    }
}
