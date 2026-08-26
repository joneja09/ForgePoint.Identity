using Microsoft.AspNetCore.Authentication;
using System;
using IdentityServer4;

namespace IdentityServer.UnitTests.Common
{
    class MockSystemClock : IClock
    {
        public DateTimeOffset Now { get; set; }

        public DateTimeOffset UtcNow
        {
            get
            {
                return Now;
            }
        }
    }
}
