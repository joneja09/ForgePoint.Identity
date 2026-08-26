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

## 2. Rewrite namespaces

| IdentityServer4 | ForgePoint.Identity |
| --- | --- |
| `IdentityServer4` | `ForgePoint.Identity` |
| `IdentityServer4.Models` | `ForgePoint.Identity.Models` |
| `IdentityServer4.Stores` | `ForgePoint.Identity.Stores` |
| `IdentityServer4.EntityFramework` | `ForgePoint.Identity.EntityFramework` |
| `IdentityServer4.AspNetIdentity` | `ForgePoint.Identity.AspNetIdentity` |

`AddIdentityServer()`, `UseIdentityServer()`, and `AddHealthChecks().AddIdentityServer()` stay the same and still live in `Microsoft.Extensions.DependencyInjection`.

## 3. Run the upgrade script

From a clone of this repository (or copy `tools/upgrade-namespaces` into your app):

```bash
python3 tools/upgrade-namespaces/rewrite.py /path/to/your/app --all
```

or

```bash
./tools/upgrade-namespaces/upgrade.sh /path/to/your/app
```

```powershell
./tools/upgrade-namespaces/upgrade.ps1 -Path C:\path\to\your\app
```

`--all` updates:

- `.cs`, `.cshtml`, `.razor` namespace and `using` directives
- `.csproj` / `.props` / `.targets` package ids
- EF Core snapshots and migrations that store CLR type names as `IdentityServer4.EntityFramework.Entities.*`

The script does **not** rename `AddIdentityServer`, `IdentityServerOptions`, `IdentityServerConstants`, or test assembly names used in `InternalsVisibleTo`.

Review the diff, then build.

## 4. Entity Framework

Table names (`Clients`, `PersistedGrants`, and so on) do not change. Existing databases do not need a schema migration for the namespace rename.

Do run the script over your `Migrations` folder so snapshots use `ForgePoint.Identity.EntityFramework.Entities.*`. After that, do **not** add a new EF migration just for the rename — that would generate a noisy no-op or a false table rebuild.

IdentityServer4 4.x databases still need the PAR column if you use pushed authorization:

```sql
ALTER TABLE [Clients] ADD [RequirePushedAuthorization] bit NOT NULL DEFAULT 0;
```

See `docs/migrations/add-require-pushed-authorization.sql`.

## 5. What you should still do by hand

- Authentication scheme strings such as `"IdentityServerAccessToken"` are unchanged
- Signing keys, connection strings, and client configuration stay as they are
- Host UI text that says “IdentityServer4” is cosmetic; the sample hosts in this repo already say ForgePoint.Identity
