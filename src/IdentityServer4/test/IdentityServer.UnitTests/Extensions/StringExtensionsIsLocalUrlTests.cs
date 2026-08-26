// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using ForgePoint.Identity.Extensions;
using Xunit;

namespace IdentityServer.UnitTests.Extensions
{
    public class StringExtensionsIsLocalUrlTests
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/foo")]
        [InlineData("/foo/bar")]
        [InlineData("~/")]
        [InlineData("~/foo")]
        public void local_paths_are_accepted(string url)
        {
            url.IsLocalUrl().Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("//evil.com")]
        [InlineData("/\\evil.com")]
        [InlineData("~/\\evil.com")]
        [InlineData("http://evil.com")]
        [InlineData("https://evil.com")]
        [InlineData("evil.com")]
        public void non_local_urls_are_rejected(string url)
        {
            url.IsLocalUrl().Should().BeFalse();
        }

        [Fact]
        public void urls_with_control_characters_are_rejected()
        {
            ("/" + '\t' + "/evil.com").IsLocalUrl().Should().BeFalse();
            ("/" + '\n' + "evil.com").IsLocalUrl().Should().BeFalse();
            ("/" + '\0' + "evil.com").IsLocalUrl().Should().BeFalse();
            ("/~/" + '\t' + "x").IsLocalUrl().Should().BeFalse();
        }

        [Fact]
        public void urls_with_embedded_backslashes_are_rejected()
        {
            "/foo\\bar".IsLocalUrl().Should().BeFalse();
        }
    }
}
