# IdentityServer4 for .NET 8 and .NET 10

IdentityServer4 is a free, open source [OpenID Connect](https://openid.net/connect/) and [OAuth 2.0](https://datatracker.ietf.org/doc/html/rfc6749) framework for ASP.NET Core. This branch is a continuation of the last Apache-2.0 IdentityServer4 release, upgraded for current .NET and expanded with protocol and hosting features that modern apps expect.

It remains licensed under [Apache 2.0](https://opensource.org/licenses/Apache-2.0).

## What's new in 10.x

- **.NET 8 and .NET 10** target frameworks for all libraries
- **CVE-2024-39694** open-redirect fix in local URL validation
- **Pushed Authorization Requests** (RFC 9126) at `/connect/par`
- **Health checks** via `AddHealthChecks().AddIdentityServer()`
- **`IClock` / `TimeProvider`** instead of the removed ASP.NET `ISystemClock`
- Package and test dependencies updated for current .NET

Duende IdentityServer is the commercial successor of the original project. This repository keeps the IdentityServer4 APIs and Apache-2.0 license so existing apps can move to current .NET without a product change.

## Packages

| Package | Role |
| --- | --- |
| `IdentityServer4` | Protocol implementation and ASP.NET Core host integration |
| `IdentityServer4.Storage` | Store contracts and models |
| `IdentityServer4.EntityFramework` | EF Core configuration and operational stores |
| `IdentityServer4.EntityFramework.Storage` | EF Core entities and stores |
| `IdentityServer4.AspNetIdentity` | ASP.NET Core Identity integration |

## How to build

* Install the [.NET 10 SDK](https://dotnet.microsoft.com/download) (the SDK also builds the `net8.0` TFM)
* Install Git
* Clone this repo
* Run `build.sh` or `build.ps1` from the repository root

The build packs each project into `./nuget` in dependency order: Storage → IdentityServer4 → EntityFramework.Storage → EntityFramework → AspNetIdentity.

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

Historical IdentityServer4 docs: [https://identityserver4.readthedocs.io](https://identityserver4.readthedocs.io).

PAR is described in [RFC 9126](https://www.rfc-editor.org/rfc/rfc9126). The discovery document advertises `pushed_authorization_request_endpoint` when the endpoint is enabled.

## Security

See [SECURITY.MD](SECURITY.MD). IdentityServer4 4.1.2 and earlier are affected by [CVE-2024-39694](https://github.com/IdentityServer/IdentityServer4/security/advisories/GHSA-55p7-v223-x366). This 10.x line includes the local-URL validation fix.

## Acknowledgements

IdentityServer4 is built using ASP.NET Core, IdentityModel, Newtonsoft.Json, xUnit, Fluent Assertions, MinVer, Bullseye, and SimpleExec — and the work of [every contributor](https://github.com/IdentityServer/IdentityServer4/graphs/contributors) to the original project.
