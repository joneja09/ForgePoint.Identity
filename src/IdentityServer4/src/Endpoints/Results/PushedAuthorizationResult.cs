// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Net;
using System.Threading.Tasks;
using ForgePoint.Identity.Extensions;
using ForgePoint.Identity.Hosting;
using Microsoft.AspNetCore.Http;

namespace ForgePoint.Identity.Endpoints.Results
{
    internal class PushedAuthorizationResult : IEndpointResult
    {
        public string RequestUri { get; }
        public int ExpiresIn { get; }

        public PushedAuthorizationResult(string requestUri, int expiresIn)
        {
            RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
            ExpiresIn = expiresIn;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Created;
            context.Response.SetNoCache();

            await context.Response.WriteJsonAsync(new ResultDto
            {
                request_uri = RequestUri,
                expires_in = ExpiresIn
            });
        }

        internal class ResultDto
        {
            public string request_uri { get; set; }
            public int expires_in { get; set; }
        }
    }
}
