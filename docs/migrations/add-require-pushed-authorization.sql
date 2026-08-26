-- Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
-- Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

-- Additive schema change for IdentityServer4 10: RFC 9126 PAR client flag.
-- Safe to run against existing IdentityServer4 4.x databases.

IF COL_LENGTH('dbo.Clients', 'RequirePushedAuthorization') IS NULL
BEGIN
    ALTER TABLE [dbo].[Clients]
    ADD [RequirePushedAuthorization] bit NOT NULL
        CONSTRAINT [DF_Clients_RequirePushedAuthorization] DEFAULT (0);
END
GO
