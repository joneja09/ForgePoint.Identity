# ForgePoint.Identity.Storage

Models and storage interfaces for [ForgePoint.Identity](https://www.nuget.org/packages/ForgePoint.Identity): clients, resources, secrets, persisted grants, and the store contracts used to persist them.

Apache-2.0 continuation of IdentityServer4 by **ForgePoint Labs**. This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

You usually do not reference this package directly. `ForgePoint.Identity` depends on it. Reference it when you implement custom stores.

## Install

```bash
dotnet add package ForgePoint.Identity.Storage
```

**Target frameworks:** `net8.0`, `net10.0`  
**Namespaces:** `ForgePoint.Identity`, `ForgePoint.Identity.Models`, `ForgePoint.Identity.Stores`

## What it contains

**Models** (`ForgePoint.Identity.Models`): `Client`, `ApiResource`, `ApiScope`, `IdentityResource`, `Secret`, `PersistedGrant`, `AuthorizationCode`, `RefreshToken`, `DeviceCode`, `Consent`.

**Stores** (`ForgePoint.Identity.Stores`): `IClientStore`, `IResourceStore`, `IPersistedGrantStore`, `IDeviceFlowStore`, `IAuthorizationCodeStore`, `IRefreshTokenStore`, `IReferenceTokenStore`, `IUserConsentStore`.

`Client.RequirePushedAuthorization` is the per-client PAR flag used by the protocol package.

## Custom stores

```csharp
using ForgePoint.Identity.Models;
using ForgePoint.Identity.Stores;

public class MyClientStore : IClientStore
{
    public Task<Client> FindClientByIdAsync(string clientId)
    {
        // load from your database
        return Task.FromResult<Client>(null);
    }
}

builder.Services.AddIdentityServer()
    .AddClientStore<MyClientStore>();
```

In-memory and EF implementations live in `ForgePoint.Identity` and `ForgePoint.Identity.EntityFramework`.

## Related packages

| Package | Role |
| --- | --- |
| `ForgePoint.Identity` | Protocol implementation and ASP.NET Core host integration |
| `ForgePoint.Identity.Storage` | Store contracts and models |
| `ForgePoint.Identity.EntityFramework` | EF Core configuration and operational stores |
| `ForgePoint.Identity.EntityFramework.Storage` | EF Core entities, DbContexts, and mappings |
| `ForgePoint.Identity.AspNetIdentity` | ASP.NET Core Identity integration |

## Upgrade from IdentityServer4

`IdentityServer4.Storage` → `ForgePoint.Identity.Storage`. Namespaces `IdentityServer4.*` → `ForgePoint.Identity.*`. See the [upgrade guide](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/upgrade.md).

## License

[Apache-2.0](https://opensource.org/licenses/Apache-2.0). Source: [github.com/joneja09/ForgePoint.Identity](https://github.com/joneja09/ForgePoint.Identity).
