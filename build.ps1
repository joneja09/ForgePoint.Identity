$ErrorActionPreference = "Stop"

if (Test-Path ./nuget) {
    Remove-Item ./nuget -Recurse -Force
}
New-Item -ItemType Directory -Force -Path ./nuget | Out-Null

$nugetPackages = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:USERPROFILE ".nuget\packages" }
Get-ChildItem -Path $nugetPackages -Directory -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -like "forgepoint.identity*" } |
    Remove-Item -Recurse -Force

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$buildArgs = $args
$projects = @(
    "./src/Storage",
    "./src/IdentityServer4",
    "./src/EntityFramework.Storage",
    "./src/EntityFramework",
    "./src/AspNetIdentity"
)

foreach ($project in $projects) {
    Push-Location $project
    try {
        & ./build.ps1 @buildArgs
        if ($LASTEXITCODE -ne 0) {
            throw "Build failed in $project (exit $LASTEXITCODE)."
        }
    }
    finally {
        Pop-Location
    }
}
