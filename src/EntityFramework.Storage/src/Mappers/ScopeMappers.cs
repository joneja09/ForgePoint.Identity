// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using ForgePoint.Identity.EntityFramework.Entities;

namespace ForgePoint.Identity.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for scopes.
    /// </summary>
    public static class ScopeMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.ApiScope ToModel(this ApiScope entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Models.ApiScope
            {
                Enabled = entity.Enabled,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                Required = entity.Required,
                Emphasize = entity.Emphasize,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                UserClaims = MappingHelpers.MapStrings(entity.UserClaims, x => x.Type),
                Properties = MappingHelpers.MapProperties(entity.Properties)
            };
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ApiScope ToEntity(this Models.ApiScope model)
        {
            if (model == null)
            {
                return null;
            }

            return new ApiScope
            {
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Required = model.Required,
                Emphasize = model.Emphasize,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                UserClaims = MappingHelpers.MapList(model.UserClaims, x => new ApiScopeClaim { Type = x }),
                Properties = MappingHelpers.MapProperties<ApiScopeProperty>(model.Properties)
            };
        }
    }
}
