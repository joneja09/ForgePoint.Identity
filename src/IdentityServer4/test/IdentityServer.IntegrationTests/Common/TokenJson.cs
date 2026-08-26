// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stj = System.Text.Json.JsonSerializer;

namespace IdentityServer.IntegrationTests.Common
{
    internal static class TokenJson
    {
        public static List<object> AsList(object value)
        {
            switch (value)
            {
                case null:
                    return null;
                case JArray jarr:
                    return jarr.Cast<object>().ToList();
                case JsonElement je when je.ValueKind == JsonValueKind.Array:
                    return Stj.Deserialize<List<object>>(je.GetRawText());
                case IEnumerable enumerable when value is not string:
                    return enumerable.Cast<object>().ToList();
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
                return Stj.Deserialize<T>(element.GetRawText());
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
        }
    }
}
