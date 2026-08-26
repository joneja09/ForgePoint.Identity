// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using ForgePoint.Identity.EntityFramework.Mappers;
using ForgePoint.Identity.Models;
using Xunit;

namespace ForgePoint.Identity.EntityFramework.UnitTests.Mappers
{
    public class PersistedGrantMappersTests
    {
        [Fact]
        public void CanMap()
        {
            var model = new PersistedGrant()
            {
                ConsumedTime = new System.DateTime(2020, 02, 03, 4, 5, 6)
            };
            
            var mappedEntity = model.ToEntity();
            mappedEntity.ConsumedTime.Value.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));
            
            var mappedModel = mappedEntity.ToModel();
            mappedModel.ConsumedTime.Value.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void UpdateEntity_overwrites_existing_values()
        {
            var entity = new Entities.PersistedGrant
            {
                Key = "old",
                Data = "old-data",
                ConsumedTime = null
            };

            var model = new PersistedGrant
            {
                Key = "new",
                Type = "refresh_token",
                SubjectId = "sub",
                SessionId = "sid",
                ClientId = "client",
                Description = "desc",
                CreationTime = new System.DateTime(2020, 1, 2, 3, 4, 5),
                Expiration = new System.DateTime(2020, 2, 3, 4, 5, 6),
                ConsumedTime = new System.DateTime(2020, 02, 03, 4, 5, 6),
                Data = "new-data"
            };

            model.UpdateEntity(entity);

            entity.Key.Should().Be("new");
            entity.Type.Should().Be("refresh_token");
            entity.SubjectId.Should().Be("sub");
            entity.SessionId.Should().Be("sid");
            entity.ClientId.Should().Be("client");
            entity.Description.Should().Be("desc");
            entity.CreationTime.Should().Be(new System.DateTime(2020, 1, 2, 3, 4, 5));
            entity.Expiration.Should().Be(new System.DateTime(2020, 2, 3, 4, 5, 6));
            entity.ConsumedTime.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));
            entity.Data.Should().Be("new-data");
        }
    }
}