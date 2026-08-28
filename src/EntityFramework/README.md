# ForgePoint.Identity.EntityFramework

Entity Framework Core configuration APIs for [ForgePoint.Identity](https://www.nuget.org/packages/ForgePoint.Identity): `AddConfigurationStore` and `AddOperationalStore`.

Apache-2.0 continuation of IdentityServer4 by **ForgePoint Labs**. This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

This package pulls in `ForgePoint.Identity` and `ForgePoint.Identity.EntityFramework.Storage`.

## Install

```bash
dotnet add package ForgePoint.Identity.EntityFramework
```

Also add an EF Core database provider (for example `Microsoft.EntityFrameworkCore.SqlServer` or `Microsoft.EntityFrameworkCore.Sqlite`).

**Target frameworks:** `net8.0`, `net10.0`  
**Namespace:** `ForgePoint.Identity.EntityFramework`  
DI extensions live in `Microsoft.Extensions.DependencyInjection`.

## Usage

```csharp
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString);
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString);
        options.EnableTokenCleanup = true;
    })
    .AddConfigurationStoreCache();
```

- **Configuration store:** clients, resources, CORS (`ConfigurationDbContext`).
- **Operational store:** codes, tokens, consents, device flow (`PersistedGrantDbContext`).
- **`AddConfigurationStoreCache()`:** optional in-memory cache in front of the configuration store.

Table names (`Clients`, `PersistedGrants`, and so on) are unchanged from IdentityServer4.

## Database migrations

Create and apply EF Core migrations against `ConfigurationDbContext` and `PersistedGrantDbContext`. IdentityServer4 4.x configuration databases that use PAR need one additive column:

```sql
ALTER TABLE [Clients] ADD [RequirePushedAuthorization] bit NOT NULL DEFAULT 0;
```

Script: [docs/migrations/add-require-pushed-authorization.sql](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/migrations/add-require-pushed-authorization.sql).

The namespace rename does **not** require a schema migration. Run the [upgrade script](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/upgrade.md) over your `Migrations` folder so snapshots use `ForgePoint.Identity.EntityFramework.Entities.*`, then do not add a new EF migration just for the rename.

## Related packages

| Package | Role |
| --- | --- |
| `ForgePoint.Identity` | Protocol implementation and ASP.NET Core host integration |
| `ForgePoint.Identity.Storage` | Store contracts and models |
| `ForgePoint.Identity.EntityFramework` | EF Core configuration and operational stores |
| `ForgePoint.Identity.EntityFramework.Storage` | EF Core entities, DbContexts, and mappings |
| `ForgePoint.Identity.AspNetIdentity` | ASP.NET Core Identity integration |

## Upgrade from IdentityServer4

`IdentityServer4.EntityFramework` → `ForgePoint.Identity.EntityFramework`. See the [upgrade guide](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/upgrade.md).

## License

[Apache-2.0](https://opensource.org/licenses/Apache-2.0). Source: [github.com/joneja09/ForgePoint.Identity](https://github.com/joneja09/ForgePoint.Identity).
