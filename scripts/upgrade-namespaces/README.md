# ForgePoint.Identity upgrade scripts

Rewrites IdentityServer4 package ids and C# namespaces to ForgePoint.Identity.

Type names such as `AddIdentityServer` and `IdentityServerOptions` are not changed. Requires Python 3.

```bash
python3 rewrite.py /path/to/your/app --all --dry-run
python3 rewrite.py /path/to/your/app --all
./upgrade.sh /path/to/your/app
```

```powershell
./upgrade.ps1 -Path C:\path\to\your\app
```

Run the tests with:

```bash
python3 test_rewrite.py
```

See [docs/upgrade.md](../../docs/upgrade.md).
