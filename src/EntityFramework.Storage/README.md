# ForgePoint.Identity.EntityFramework.Storage

Entity Framework Core persistence layer for [ForgePoint.Identity](https://www.nuget.org/packages/ForgePoint.Identity): DbContexts, entities, and handwritten entity/model mappings.

Apache-2.0 continuation of IdentityServer4 by **ForgePoint Labs**. This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

You usually consume this through `ForgePoint.Identity.EntityFramework`. Reference it directly for design-time migrations, custom `DbContext` types, or `ToModel` / `ToEntity` mapping.

## Install

```bash
dotnet add package ForgePoint.Identity.EntityFramework.Storage
```

**Target frameworks:** `net8.0`, `net10.0`  
**Namespaces:** `ForgePoint.Identity.EntityFramework`, `.DbContexts`, `.Entities`, `.Mappers`, `.Storage`

## What it contains

- **`ConfigurationDbContext`** / **`IConfigurationDbContext`:** clients, identity resources, API resources, API scopes.
- **`PersistedGrantDbContext`** / **`IPersistedGrantDbContext`:** persisted grants and device flow codes.
- **Entities** under `ForgePoint.Identity.EntityFramework.Entities`.
- **Mappers** (`ToModel`, `ToEntity`, `UpdateEntity`) with no AutoMapper dependency.

Register the contexts without the protocol host (for example in a migrations project):

```csharp
using ForgePoint.Identity.EntityFramework.Storage;

services.AddConfigurationDbContext(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
});
services.AddOperationalDbContext(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
});
```

Host apps should use `AddConfigurationStore` / `AddOperationalStore` from `ForgePoint.Identity.EntityFramework` instead.

## Related packages

| Package | Role |
| --- | --- |
| `ForgePoint.Identity` | Protocol implementation and ASP.NET Core host integration |
| `ForgePoint.Identity.Storage` | Store contracts and models |
| `ForgePoint.Identity.EntityFramework` | EF Core configuration and operational stores |
| `ForgePoint.Identity.EntityFramework.Storage` | EF Core entities, DbContexts, and mappings |
| `ForgePoint.Identity.AspNetIdentity` | ASP.NET Core Identity integration |

## Upgrade from IdentityServer4

`IdentityServer4.EntityFramework.Storage` → `ForgePoint.Identity.EntityFramework.Storage`. Table names do not change. See the [upgrade guide](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/upgrade.md).

## License

[Apache-2.0](https://opensource.org/licenses/Apache-2.0). Source: [github.com/joneja09/ForgePoint.Identity](https://github.com/joneja09/ForgePoint.Identity).
