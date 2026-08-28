# ForgePoint.Identity

OpenID Connect and OAuth 2.0 framework for ASP.NET Core. Apache-2.0 continuation of IdentityServer4 by **ForgePoint Labs**.

This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

`AddIdentityServer()` and `IdentityServer*` type names are unchanged. C# namespaces are `ForgePoint.Identity.*`.

## Install

```bash
dotnet add package ForgePoint.Identity
```

**Target frameworks:** `net8.0`, `net10.0`

## Quick start

```csharp
using ForgePoint.Identity;
using ForgePoint.Identity.Models;

builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseSuccessEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseErrorEvents = true;
        options.PushedAuthorization.Required = false;
    })
    .AddInMemoryClients(clients)
    .AddInMemoryIdentityResources(identityResources)
    .AddInMemoryApiScopes(apiScopes)
    .AddDeveloperSigningCredential();

builder.Services.AddHealthChecks()
    .AddIdentityServer();

app.UseIdentityServer();
app.MapHealthChecks("/health");
```

Enable [Pushed Authorization Requests](https://www.rfc-editor.org/rfc/rfc9126) per client with `Client.RequirePushedAuthorization = true`, or for every client with `options.PushedAuthorization.Required = true`. The discovery document advertises `pushed_authorization_request_endpoint` at `/connect/par` when the endpoint is enabled.

## Related packages

| Package | Role |
| --- | --- |
| `ForgePoint.Identity` | Protocol implementation and ASP.NET Core host integration |
| `ForgePoint.Identity.Storage` | Store contracts and models (pulled in automatically) |
| `ForgePoint.Identity.EntityFramework` | EF Core configuration and operational stores |
| `ForgePoint.Identity.EntityFramework.Storage` | EF Core entities, DbContexts, and mappings |
| `ForgePoint.Identity.AspNetIdentity` | ASP.NET Core Identity integration |

## Upgrade from IdentityServer4

Swap the package id, then rewrite namespaces:

```bash
dotnet add package ForgePoint.Identity
python3 scripts/upgrade-namespaces/rewrite.py /path/to/your/app --all
```

Guide: [docs/upgrade.md](https://github.com/joneja09/ForgePoint.Identity/blob/main/docs/upgrade.md).

## License and security

Licensed under [Apache-2.0](https://opensource.org/licenses/Apache-2.0). This 10.x line includes the [CVE-2024-39694](https://nvd.nist.gov/vuln/detail/CVE-2024-39694) local-URL validation fix.

- Source: [github.com/joneja09/ForgePoint.Identity](https://github.com/joneja09/ForgePoint.Identity)
- Releases: [GitHub Releases](https://github.com/joneja09/ForgePoint.Identity/releases)
- Protocol docs: [docs/](https://github.com/joneja09/ForgePoint.Identity/tree/main/docs)
