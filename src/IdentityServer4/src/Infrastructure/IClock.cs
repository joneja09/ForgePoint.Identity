// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4
{
    /// <summary>
    /// Abstraction over the system clock so IdentityServer can be tested
    /// and can use <see cref="TimeProvider"/> on modern .NET.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// The current UTC time.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
