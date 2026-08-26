// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace IdentityServer.IntegrationTests.Common
{
    internal static class JsonElementExtensions
    {
        public static T ToObject<T>(this JsonElement element)
        {
            if (typeof(T) == typeof(Dictionary<string, object>))
            {
                return (T)(object)ToDictionary(element);
            }

            return JsonSerializer.Deserialize<T>(element.GetRawText());
        }

        public static T ToObject<T>(this JsonElement? element)
        {
            if (element == null)
            {
                return default;
            }

            return element.Value.ToObject<T>();
        }

        public static Dictionary<string, object> ToDictionary(this JsonElement element)
        {
            var dict = new Dictionary<string, object>();
            foreach (var prop in element.EnumerateObject())
            {
                dict[prop.Name] = ConvertValue(prop.Value);
            }

            return dict;
        }

        private static object ConvertValue(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    if (element.TryGetInt64(out var l)) return l;
                    return element.GetDouble();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Object:
                    return ToDictionary(element);
                case JsonValueKind.Array:
                    return element.EnumerateArray().Select(ConvertValue).ToList();
                default:
                    return null;
            }
        }
    }
}
