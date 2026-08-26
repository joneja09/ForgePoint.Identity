#!/usr/bin/env python3
"""Rewrite IdentityServer4 namespaces (and optional package ids) to ForgePoint.Identity.

Used both to migrate this repository and as the engine behind the consumer scripts.
Type names such as AddIdentityServer, IdentityServerOptions, and IdentityServerConstants
are left unchanged.
"""

from __future__ import annotations

import argparse
import os
import re
from pathlib import Path

SKIP_DIR_NAMES = {
    ".git",
    "bin",
    "obj",
    "nuget",
    "artifacts",
    "node_modules",
    "wwwroot",
}

SOURCE_SUFFIXES = {".cs", ".cshtml", ".razor", ".fs", ".vb"}
PROJECT_SUFFIXES = {".csproj", ".fsproj", ".vbproj", ".props", ".targets"}

# Longest-first so IdentityServer4.Storage does not become ForgePoint.Identity.Storage
# after a shorter IdentityServer4 replacement.
PACKAGE_REPLACEMENTS = (
    ("IdentityServer4.EntityFramework.Storage", "ForgePoint.Identity.EntityFramework.Storage"),
    ("IdentityServer4.EntityFramework", "ForgePoint.Identity.EntityFramework"),
    ("IdentityServer4.AspNetIdentity", "ForgePoint.Identity.AspNetIdentity"),
    ("IdentityServer4.Storage", "ForgePoint.Identity.Storage"),
    ("IdentityServer4", "ForgePoint.Identity"),
)

FRIEND_ASSEMBLY_RE = re.compile(r'InternalsVisibleTo\(\s*"IdentityServer4[^"]*"')

USING_LINE_RE = re.compile(r'(?m)^(\s*(?:@)?(?:global\s+)?using )IdentityServer4;')
USING_VB_RE = re.compile(r'(?m)^(\s*Imports )IdentityServer4(\r?\n)')
NAMESPACE_FILE_SCOPED_RE = re.compile(r'(?m)^(namespace )IdentityServer4;')
NAMESPACE_BLOCK_RE = re.compile(r'(?m)^(namespace )IdentityServer4(\r?\n| )')


def protect_friend_assemblies(text: str) -> tuple[str, list[tuple[str, str]]]:
    saved: list[tuple[str, str]] = []

    def repl(match: re.Match[str]) -> str:
        token = f"___FP_IVT_{len(saved)}___"
        saved.append((token, match.group(0)))
        return token

    return FRIEND_ASSEMBLY_RE.sub(repl, text), saved


def restore_friend_assemblies(text: str, saved: list[tuple[str, str]]) -> str:
    for token, original in saved:
        text = text.replace(token, original)
    return text


def rewrite_namespaces(text: str) -> str:
    text, saved = protect_friend_assemblies(text)

    text = text.replace("IdentityServer4.", "ForgePoint.Identity.")
    text = text.replace("global::IdentityServer4", "global::ForgePoint.Identity")
    text = USING_LINE_RE.sub(r"\1ForgePoint.Identity;", text)
    text = USING_VB_RE.sub(r"\1ForgePoint.Identity\2", text)
    text = NAMESPACE_FILE_SCOPED_RE.sub(r"\1ForgePoint.Identity;", text)
    text = NAMESPACE_BLOCK_RE.sub(r"\1ForgePoint.Identity\2", text)

    return restore_friend_assemblies(text, saved)


def rewrite_packages(text: str) -> str:
    """Rewrite PackageReference / PackageVersion / PackageId only.

    AssemblyName, RootNamespace, ProjectReference paths, PackageTags, and
    InternalsVisibleTo stay as they are so friend-test assemblies keep working.
    """
    for old, new in PACKAGE_REPLACEMENTS:
        text = text.replace(f'Include="{old}"', f'Include="{new}"')
        text = text.replace(f"Include='{old}'", f"Include='{new}'")
        text = text.replace(f'Update="{old}"', f'Update="{new}"')
        text = text.replace(f"Update='{old}'", f"Update='{new}'")
        text = text.replace(f"<PackageId>{old}</PackageId>", f"<PackageId>{new}</PackageId>")
    return text


def should_skip_dir(name: str) -> bool:
    return name in SKIP_DIR_NAMES


def iter_files(root: Path, suffixes: set[str]):
    for dirpath, dirnames, filenames in os.walk(root):
        dirnames[:] = [d for d in dirnames if not should_skip_dir(d)]
        for filename in filenames:
            path = Path(dirpath) / filename
            if path.suffix.lower() in suffixes:
                yield path


def rewrite_file(path: Path, namespaces: bool, packages: bool, dry_run: bool) -> bool:
    original = path.read_text(encoding="utf-8")
    updated = original
    if namespaces:
        updated = rewrite_namespaces(updated)
    if packages:
        updated = rewrite_packages(updated)
    if updated == original:
        return False
    if not dry_run:
        path.write_text(updated, encoding="utf-8")
    return True


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("root", nargs="?", default=".", help="Solution or project directory")
    parser.add_argument("--namespaces", action="store_true", default=False)
    parser.add_argument("--packages", action="store_true", default=False)
    parser.add_argument("--all", action="store_true", help="Rewrite namespaces and package ids")
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Print files that would change without writing them",
    )
    args = parser.parse_args()

    namespaces = args.namespaces or args.all
    packages = args.packages or args.all
    if not namespaces and not packages:
        parser.error("Specify --namespaces, --packages, or --all")

    root = Path(args.root).resolve()
    changed = 0
    prefix = "would update" if args.dry_run else "updated"

    if namespaces:
        for path in iter_files(root, SOURCE_SUFFIXES):
            if rewrite_file(path, namespaces=True, packages=False, dry_run=args.dry_run):
                print(f"namespace {prefix} {path.relative_to(root)}")
                changed += 1

    if packages:
        for path in iter_files(root, PROJECT_SUFFIXES):
            if rewrite_file(path, namespaces=False, packages=True, dry_run=args.dry_run):
                print(f"package  {prefix} {path.relative_to(root)}")
                changed += 1

    print(f"{prefix.capitalize()} {changed} file(s) under {root}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
