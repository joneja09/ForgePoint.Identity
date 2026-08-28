# ForgePoint.Identity.AspNetIdentity

ASP.NET Core Identity integration for [ForgePoint.Identity](https://www.nuget.org/packages/ForgePoint.Identity): profile service, resource-owner password validator, and claim/cookie defaults that match OpenID Connect.

Apache-2.0 continuation of IdentityServer4 by **ForgePoint Labs**. This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

## Install

```bash
dotnet add package ForgePoint.Identity.AspNetIdentity
```

Register ASP.NET Core Identity in the host as well (`Microsoft.AspNetCore.Identity.EntityFrameworkCore` or another store).

**Target frameworks:** `net8.0`, `net10.0`  
**Namespace:** `ForgePoint.Identity.AspNetIdentity`  
DI extensions live in `Microsoft.Extensions.DependencyInjection`.

## Usage

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(identityResources)
    .AddInMemoryApiScopes(apiScopes)
    .AddInMemoryClients(clients)
    .AddAspNetIdentity<ApplicationUser>();

app.UseIdentityServer();
```

`AddAspNetIdentity<TUser>()` wires:

- `IProfileService` and `IResourceOwnerPasswordValidator` for `TUser`
- `IUserClaimsPrincipalFactory<TUser>` so subject, name, and role claims use IdentityModel claim types
- ASP.NET Identity application and external cookies (`SameSite=None`, essential) so authorize requests can run in an iframe

Call `AddIdentity` (or equivalent) **before** `AddAspNetIdentity`.

Walk-through using the same APIs: [IdentityServer4 ASP.NET Identity quickstart](https://identityserver4.readthedocs.io/en/latest/quickstarts/6_aspnet_identity.html).

## Related packages

| Package | Role |
| --- | --- |
| `ForgePoint.Identity` | Protocol implementation and ASP.NET Core host integration |
| `ForgePoint.Identity.Storage` | Store contracts and models |
| `ForgePoint.Identity.EntityFramework` | EF Core configuration and operational stores |
| `ForgePoint.Identity.EntityFramework.Storage` | EF Core entities, DbContexts, and mappings |
| `ForgePoint.Identity.AspNetIdentity` | ASP.NET Core Identity integration |

## Upgrade from IdentityServer4

`IdentityServer4.AspNetIdentity` → `ForgePoint.Identity.AspNetIdentity`. `AddAspNetIdentity<TUser>()` is unchanged. See the [upgrade guide](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/upgrade.md).

## License

[Apache-2.0](https://opensource.org/licenses/Apache-2.0). Source: [github.com/joneja09/ForgePoint.Identity](https://github.com/joneja09/ForgePoint.Identity).
