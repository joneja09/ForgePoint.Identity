// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using ForgePoint.Identity.EntityFramework.Entities;

namespace ForgePoint.Identity.EntityFramework.Mappers
{
    internal static class MappingHelpers
    {
        public static List<TOut> MapList<TIn, TOut>(IEnumerable<TIn> source, Func<TIn, TOut> map)
        {
            if (source == null)
            {
                return new List<TOut>();
            }

            return source.Select(map).ToList();
        }

        public static ICollection<string> MapStrings<T>(IEnumerable<T> source, Func<T, string> selector)
        {
            if (source == null)
            {
                return new HashSet<string>();
            }

            return new HashSet<string>(source.Select(selector));
        }

        public static Dictionary<string, string> MapProperties(IEnumerable<Property> source)
        {
            if (source == null)
            {
                return new Dictionary<string, string>();
            }

            // ToDictionary throws on duplicate keys, matching previous AutoMapper behavior.
            return source.ToDictionary(x => x.Key, x => x.Value);
        }

        public static List<TProperty> MapProperties<TProperty>(IDictionary<string, string> source)
            where TProperty : Property, new()
        {
            if (source == null)
            {
                return new List<TProperty>();
            }

            return source.Select(x => new TProperty { Key = x.Key, Value = x.Value }).ToList();
        }

        public static Models.Secret ToSecret(Entities.Secret entity)
        {
            if (entity == null)
            {
                return null;
            }

            var secret = new Models.Secret
            {
                Description = entity.Description,
                Value = entity.Value,
                Expiration = entity.Expiration
            };

            // Preserve the model default (SharedSecret) when the entity type is null.
            if (entity.Type != null)
            {
                secret.Type = entity.Type;
            }

            return secret;
        }

        public static TSecret ToEntitySecret<TSecret>(Models.Secret model)
            where TSecret : Entities.Secret, new()
        {
            if (model == null)
            {
                return null;
            }

            return new TSecret
            {
                Description = model.Description,
                Value = model.Value,
                Expiration = model.Expiration,
                Type = model.Type
            };
        }
    }
}
