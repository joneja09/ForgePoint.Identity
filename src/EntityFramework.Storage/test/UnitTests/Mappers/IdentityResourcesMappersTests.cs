// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using ForgePoint.Identity.EntityFramework.Mappers;
using ForgePoint.Identity.Models;
using Xunit;

namespace ForgePoint.Identity.EntityFramework.UnitTests.Mappers
{
    public class IdentityResourcesMappersTests
    {
        [Fact]
        public void CanMapIdentityResources()
        {
            var model = new IdentityResource();
            var mappedEntity = model.ToEntity();
            var mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }
    }
}