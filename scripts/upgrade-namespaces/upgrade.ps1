# Rewrite IdentityServer4 package ids and namespaces to ForgePoint.Identity.
# Type names such as AddIdentityServer and IdentityServerOptions are not changed.

param(
    [string]$Path = ".",
    [switch]$Namespaces,
    [switch]$Packages,
    [switch]$All
)

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

if (-not $Namespaces -and -not $Packages -and -not $All) {
    $All = $true
}

$python = Get-Command python3 -ErrorAction SilentlyContinue
if (-not $python) {
    $python = Get-Command python -ErrorAction SilentlyContinue
}

if (-not $python) {
    throw "Python is required to run this upgrade script."
}

$args = @((Resolve-Path $Path).Path)
if ($All) { $args += "--all" }
else {
    if ($Namespaces) { $args += "--namespaces" }
    if ($Packages) { $args += "--packages" }
}

& $python.Source (Join-Path $scriptDir "rewrite.py") @args
