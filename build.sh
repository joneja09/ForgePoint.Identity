#!/usr/bin/env bash
set -euo pipefail

rm -rf nuget
mkdir nuget

# Same-height MinVer packs reuse the global cache. Drop local ForgePoint.Identity
# packages so restore picks up the nupkgs just written to ./nuget.
nuget_packages="${NUGET_PACKAGES:-$HOME/.nuget/packages}"
rm -rf "$nuget_packages"/forgepoint.identity*

dotnet tool restore

pushd ./src/Storage
./build.sh "$@"
popd

pushd ./src/IdentityServer4
./build.sh "$@"
popd

pushd ./src/EntityFramework.Storage
./build.sh "$@"
popd

pushd ./src/EntityFramework
./build.sh "$@"
popd

pushd ./src/AspNetIdentity
./build.sh "$@"
popd
