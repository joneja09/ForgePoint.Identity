# ForgePoint.Identity

ForgePoint.Identity is a free, open source [OpenID Connect](https://openid.net/connect/) and [OAuth 2.0](https://datatracker.ietf.org/doc/html/rfc6749) framework for ASP.NET Core. It is maintained by **ForgePoint Labs** as an Apache-2.0 continuation of the last IdentityServer4 release, upgraded for current .NET and expanded with protocol and hosting features that modern apps expect.

It remains licensed under [Apache 2.0](https://opensource.org/licenses/Apache-2.0). This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

`AddIdentityServer()` stays the same. C# namespaces move from `IdentityServer4.*` to `ForgePoint.Identity.*` — see the [upgrade guide](docs/upgrade.md) and `scripts/upgrade-namespaces`.

## What's new in 10.x

- **ForgePoint.Identity** package IDs and `ForgePoint.Identity.*` namespaces (was IdentityServer4)
- Upgrade script for existing apps: `scripts/upgrade-namespaces`
- **.NET 8 and .NET 10** target frameworks for all libraries
- **CVE-2024-39694** open-redirect fix in local URL validation
- **Pushed Authorization Requests** (RFC 9126) at `/connect/par`
- **Health checks** via `AddHealthChecks().AddIdentityServer()`
- **`IClock` / `TimeProvider`** instead of the removed ASP.NET `ISystemClock`
- **Handwritten EF entity/model mappings** instead of AutoMapper (no AutoMapper license required)
- Package and test dependencies updated for current .NET

Duende IdentityServer is the commercial successor of the original IdentityServer project.

## Packages

| Package | Role |
| --- | --- |
| `ForgePoint.Identity` | Protocol implementation and ASP.NET Core host integration |
| `ForgePoint.Identity.Storage` | Store contracts and models |
| `ForgePoint.Identity.EntityFramework` | EF Core configuration and operational stores |
| `ForgePoint.Identity.EntityFramework.Storage` | EF Core entities and stores |
| `ForgePoint.Identity.AspNetIdentity` | ASP.NET Core Identity integration |

```bash
dotnet add package ForgePoint.Identity
```

## How to build

* Install the [.NET 10 SDK](https://dotnet.microsoft.com/download) (the SDK also builds the `net8.0` TFM)
* Install Git
* Clone this repo
* Run `build.sh` or `build.ps1` from the repository root

The build packs each project into `./nuget` in dependency order: Storage → Identity → EntityFramework.Storage → EntityFramework → AspNetIdentity.

## Releasing

Version numbers come from [MinVer](https://github.com/adamralph/minver) git tags (`10.0.0`, not `v10.0.0`).

1. Optionally add a `NUGET_API_KEY` Actions secret (a nuget.org API key for the ForgePoint.Identity package prefix). Without it, a GitHub Release is still created and the `.nupkg` files are attached.
2. Tag main and push:

```bash
git checkout main
git pull
git tag 10.0.0
git push origin 10.0.0
```

The Release workflow packs the libraries, creates the GitHub Release, and publishes to nuget.org when `NUGET_API_KEY` is set.

## Quick start

```csharp
builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseSuccessEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseErrorEvents = true;
        options.PushedAuthorization.Required = false; // set true to require PAR globally
    })
    .AddInMemoryClients(Clients.Get())
    .AddInMemoryIdentityResources(Resources.IdentityResources)
    .AddInMemoryApiScopes(Resources.ApiScopes)
    .AddDeveloperSigningCredential();

builder.Services.AddHealthChecks()
    .AddIdentityServer();

app.UseIdentityServer();
app.MapHealthChecks("/health");
```

Enable PAR per client with `Client.RequirePushedAuthorization = true`, or for every client with `options.PushedAuthorization.Required = true`.

Existing IdentityServer4 4.x configuration databases need one additive column:

```sql
ALTER TABLE [Clients] ADD [RequirePushedAuthorization] bit NOT NULL DEFAULT 0;
```

A script is included at `docs/migrations/add-require-pushed-authorization.sql`.

## Documentation

Upgrade from IdentityServer4: [docs/upgrade.md](docs/upgrade.md).

Historical IdentityServer4 docs: [https://identityserver4.readthedocs.io](https://identityserver4.readthedocs.io).

PAR is described in [RFC 9126](https://www.rfc-editor.org/rfc/rfc9126). The discovery document advertises `pushed_authorization_request_endpoint` when the endpoint is enabled.

## Security

See [SECURITY.MD](SECURITY.MD). IdentityServer4 4.1.2 and earlier are affected by [CVE-2024-39694](https://github.com/IdentityServer/IdentityServer4/security/advisories/GHSA-55p7-v223-x366). This 10.x line includes the local-URL validation fix.

## Acknowledgements

ForgePoint.Identity is built using ASP.NET Core, IdentityModel, Newtonsoft.Json, xUnit, Fluent Assertions, MinVer, Bullseye, and SimpleExec — and the work of [every contributor](https://github.com/IdentityServer/IdentityServer4/graphs/contributors) to the original IdentityServer4 project.
