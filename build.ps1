$ErrorActionPreference = "Stop";

if (Test-Path ./nuget) {
    Remove-Item ./nuget -Recurse -Force
}
New-Item -ItemType Directory -Force -Path ./nuget | Out-Null

$nugetPackages = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:USERPROFILE ".nuget\packages" }
Get-ChildItem -Path $nugetPackages -Directory -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -like "forgepoint.identity*" } |
    Remove-Item -Recurse -Force

dotnet tool restore

pushd ./src/Storage
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/IdentityServer4
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/EntityFramework.Storage
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/EntityFramework
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/AspNetIdentity
Invoke-Expression "./build.ps1 $args"
popd
