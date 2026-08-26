# ForgePoint.Identity upgrade scripts

Rewrites IdentityServer4 package ids and C# namespaces to ForgePoint.Identity.

Requires Python 3.

```bash
python3 rewrite.py /path/to/your/app --all
./upgrade.sh /path/to/your/app
```

```powershell
./upgrade.ps1 -Path C:\path\to\your\app
```

See [docs/upgrade.md](../../docs/upgrade.md).
