// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Extension methods for client.
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Returns true if the client is an implicit-only client.
        /// </summary>
        public static bool IsImplicitOnly(this Client client)
        {
            return client != null &&
                client.AllowedGrantTypes != null &&
                client.AllowedGrantTypes.Count == 1 &&
                client.AllowedGrantTypes.First() == GrantType.Implicit;
        }

        /// <summary>
        /// Constructs a list of SecurityKey from a Secret collection
        /// </summary>
        /// <param name="secrets">The secrets</param>
        /// <returns></returns>
        public static Task<List<SecurityKey>> GetKeysAsync(this IEnumerable<Secret> secrets)
        {
            var secretList = secrets.ToList().AsReadOnly();
            var keys = new List<SecurityKey>();

            var certificates = GetCertificates(secretList)
                                .Select(c => (SecurityKey)new X509SecurityKey(c))
                                .ToList();
            keys.AddRange(certificates);

            foreach (var secret in secretList.Where(s => s.Type == IdentityServerConstants.SecretTypes.JsonWebKey))
            {
                try
                {
                    keys.Add(ParseJsonWebKey(secret.Value));
                }
                catch
                {
                    // Skip malformed JWKs so one invalid secret does not disable the others.
                }
            }

            return Task.FromResult(keys);
        }

        /// <summary>
        /// IdentityModel 8 parses JWKs with System.Text.Json, which rejects tabs, trailing commas,
        /// and Newtonsoft PascalCase property names. Fall back to Newtonsoft so existing stored secrets still load.
        /// </summary>
        private static Microsoft.IdentityModel.Tokens.JsonWebKey ParseJsonWebKey(string json)
        {
            try
            {
                return new Microsoft.IdentityModel.Tokens.JsonWebKey(json);
            }
            catch
            {
                var key = JsonConvert.DeserializeObject<Microsoft.IdentityModel.Tokens.JsonWebKey>(json);
                if (key == null || string.IsNullOrEmpty(key.Kty))
                {
                    throw;
                }

                return key;
            }
        }

        private static List<X509Certificate2> GetCertificates(IEnumerable<Secret> secrets)
        {
            return secrets
                .Where(s => s.Type == IdentityServerConstants.SecretTypes.X509CertificateBase64)
                .Select(LoadCertificate)
                .Where(c => c != null)
                .ToList();
        }

        private static X509Certificate2 LoadCertificate(Secret secret)
        {
            var raw = Convert.FromBase64String(secret.Value);
#if NET9_0_OR_GREATER
            return X509CertificateLoader.LoadCertificate(raw);
#else
            return new X509Certificate2(raw);
#endif
        }
    }
}