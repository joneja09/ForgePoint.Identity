# Upgrade from IdentityServer4 to ForgePoint.Identity

ForgePoint.Identity is a ForgePoint Labs Apache-2.0 continuation of IdentityServer4. APIs such as `AddIdentityServer()`, `IdentityServerOptions`, and `IdentityServerConstants` keep their type names. Package IDs and C# namespaces change.

This project is not affiliated with or endorsed by Duende Software or the original IdentityServer4 authors.

## 1. Swap NuGet packages

| IdentityServer4 | ForgePoint.Identity |
| --- | --- |
| `IdentityServer4` | `ForgePoint.Identity` |
| `IdentityServer4.Storage` | `ForgePoint.Identity.Storage` |
| `IdentityServer4.EntityFramework` | `ForgePoint.Identity.EntityFramework` |
| `IdentityServer4.EntityFramework.Storage` | `ForgePoint.Identity.EntityFramework.Storage` |
| `IdentityServer4.AspNetIdentity` | `ForgePoint.Identity.AspNetIdentity` |

```xml
<PackageReference Include="ForgePoint.Identity" Version="10.0.0-*" />
```

If you previously restored a local 10.x build of these packages, delete the matching folders under `~/.nuget/packages` (they are lowercase, for example `forgepoint.identity`) so restore does not reuse an older nupkg with the same MinVer version.

## 2. Rewrite namespaces

| IdentityServer4 | ForgePoint.Identity |
| --- | --- |
| `IdentityServer4` | `ForgePoint.Identity` |
| `IdentityServer4.Models` | `ForgePoint.Identity.Models` |
| `IdentityServer4.Stores` | `ForgePoint.Identity.Stores` |
| `IdentityServer4.EntityFramework` | `ForgePoint.Identity.EntityFramework` |
| `IdentityServer4.AspNetIdentity` | `ForgePoint.Identity.AspNetIdentity` |

```csharp
using ForgePoint.Identity;
using ForgePoint.Identity.Models;

builder.Services.AddIdentityServer()
    .AddInMemoryClients(clients)
    .AddDeveloperSigningCredential();

app.UseIdentityServer();
```

`AddIdentityServer()`, `UseIdentityServer()`, and `AddHealthChecks().AddIdentityServer()` stay the same and still live in `Microsoft.Extensions.DependencyInjection`.

## 3. Run the upgrade script

From a clone of this repository (or copy `scripts/upgrade-namespaces` into your app):

```bash
python3 scripts/upgrade-namespaces/rewrite.py /path/to/your/app --all --dry-run
python3 scripts/upgrade-namespaces/rewrite.py /path/to/your/app --all
```

or

```bash
./scripts/upgrade-namespaces/upgrade.sh /path/to/your/app
```

```powershell
./scripts/upgrade-namespaces/upgrade.ps1 -Path C:\path\to\your\app
```

`--all` updates:

- `.cs`, `.cshtml`, `.razor` namespace and `using` directives
- `.csproj` / `.props` / `.targets` `PackageReference`, `PackageVersion`, and `PackageId` values
- EF Core snapshots and migrations that store CLR type names as `IdentityServer4.EntityFramework.Entities.*`

The script does **not** rename:

- `AddIdentityServer`, `UseIdentityServer`, `IdentityServerOptions`, `IdentityServerConstants`
- `AssemblyName`, `RootNamespace`, `ProjectReference` paths, or `PackageTags`
- `InternalsVisibleTo("IdentityServer4...")` friend-test assemblies

Review the diff, then build.

This script is for consuming apps. Do not run `--packages` against this repository: on-disk project file names stay `IdentityServer4*.csproj` even though the NuGet package ids are `ForgePoint.Identity*`.

## 4. Entity Framework

Table names (`Clients`, `PersistedGrants`, and so on) do not change. Existing databases do not need a schema migration for the namespace rename.

Do run the script over your `Migrations` folder so snapshots use `ForgePoint.Identity.EntityFramework.Entities.*`. After that, do **not** add a new EF migration just for the rename — that would generate a noisy no-op or a false table rebuild.

IdentityServer4 4.x databases still need the PAR column if you use pushed authorization:

```sql
ALTER TABLE [Clients] ADD [RequirePushedAuthorization] bit NOT NULL DEFAULT 0;
```

See `docs/migrations/add-require-pushed-authorization.sql`.

## 5. Satellite packages this repository does not ship

| IdentityServer4 package | What to use instead |
| --- | --- |
| `IdentityServer4.AccessTokenValidation` | `Microsoft.AspNetCore.Authentication.JwtBearer` for JWTs, and [IdentityModel.AspNetCore.OAuth2Introspection](https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection) for reference tokens. Same-host APIs can use `AddLocalApiAuthentication()`. See [docs/topics/apis.rst](topics/apis.rst) and [docs/topics/add_apis.rst](topics/add_apis.rst). |
| `IdentityServer4.Templates` (`dotnet new is4*`) | Copy a folder from [samples/Quickstarts](https://github.com/joneja09/ForgePoint.Identity/tree/main/samples/Quickstarts), or `dotnet new web` and add `ForgePoint.Identity`. See [docs/quickstarts/0_overview.rst](quickstarts/0_overview.rst). |
| `IdentityServer4.KeyManagement` | Not ported. Use `AddDeveloperSigningCredential()` in development. In production load a certificate or raw key with `AddSigningCredential` and publish retired keys with `AddValidationKey`. See [docs/topics/crypto.rst](topics/crypto.rst). |

`AddIdentityServerAuthentication()` from AccessTokenValidation is not available. Do not add a `PackageReference` to `IdentityServer4.AccessTokenValidation` alongside ForgePoint.Identity.

## 6. What you should still do by hand

- Authentication scheme strings such as `"IdentityServerAccessToken"` are unchanged
- Signing keys, connection strings, and client configuration stay as they are
- Host UI text that says “IdentityServer4” is cosmetic; the sample hosts in this repo already say ForgePoint.Identity
- Confirm you are on this 10.x line for the [CVE-2024-39694](https://nvd.nist.gov/vuln/detail/CVE-2024-39694) local-URL fix
