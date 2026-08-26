using Microsoft.AspNetCore.Authentication;
using System;
using ForgePoint.Identity;

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
