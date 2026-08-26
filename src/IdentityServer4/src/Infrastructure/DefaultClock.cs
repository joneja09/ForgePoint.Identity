// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4
{
    /// <summary>
    /// Default <see cref="IClock"/> implementation backed by <see cref="TimeProvider"/>.
    /// </summary>
    public class DefaultClock : IClock
    {
        private readonly TimeProvider _timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultClock"/> class.
        /// </summary>
        /// <param name="timeProvider">The time provider. Defaults to <see cref="TimeProvider.System"/>.</param>
        public DefaultClock(TimeProvider timeProvider = null)
        {
            _timeProvider = timeProvider ?? TimeProvider.System;
        }

        /// <inheritdoc />
        public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();
    }
}
