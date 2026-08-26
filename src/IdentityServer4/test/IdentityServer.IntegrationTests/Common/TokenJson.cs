// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IdentityServer.IntegrationTests.Common
{
    internal static class TokenJson
    {
        public static IList AsList(object value)
        {
            switch (value)
            {
                case null:
                    return null;
                case JArray jarr:
                    return jarr;
                case IList list:
                    return list;
                case JsonElement je when je.ValueKind == JsonValueKind.Array:
                    return JsonSerializer.Deserialize<List<object>>(je.GetRawText());
                default:
                    return null;
            }
        }

        public static T Deserialize<T>(object value)
        {
            if (value is T typed)
            {
                return typed;
            }

            if (value is JToken token)
            {
                return token.ToObject<T>();
            }

            if (value is JsonElement element)
            {
                return JsonSerializer.Deserialize<T>(element.GetRawText());
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
        }
    }
}
