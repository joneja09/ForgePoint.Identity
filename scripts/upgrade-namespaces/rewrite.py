#!/usr/bin/env python3
"""Rewrite IdentityServer4 namespaces (and optional package ids) to ForgePoint.Identity.

Used both to migrate this repository and as the engine behind the consumer scripts.
Type names such as AddIdentityServer, IdentityServerOptions, and IdentityServerConstants
are left unchanged.
"""

from __future__ import annotations

import argparse
import os
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

INTERNALS_PLACEHOLDERS = (
    (
        'InternalsVisibleTo("IdentityServer4.EntityFramework.UnitTests',
        'InternalsVisibleTo("___FP_EF_UNIT___',
    ),
    (
        'InternalsVisibleTo("IdentityServer4.EntityFramework.IntegrationTests',
        'InternalsVisibleTo("___FP_EF_INT___',
    ),
)


def rewrite_namespaces(text: str) -> str:
    for original, placeholder in INTERNALS_PLACEHOLDERS:
        text = text.replace(original, placeholder)

    text = text.replace("IdentityServer4.", "ForgePoint.Identity.")
    text = text.replace("global::IdentityServer4", "global::ForgePoint.Identity")
    text = text.replace("using IdentityServer4;", "using ForgePoint.Identity;")
    text = text.replace("using IdentityServer4\n", "using ForgePoint.Identity\n")
    text = text.replace("@using IdentityServer4\n", "@using ForgePoint.Identity\n")
    text = text.replace("@using IdentityServer4;", "@using ForgePoint.Identity;")
    text = text.replace("namespace IdentityServer4\n", "namespace ForgePoint.Identity\n")
    text = text.replace("namespace IdentityServer4;", "namespace ForgePoint.Identity;")
    text = text.replace("namespace IdentityServer4 ", "namespace ForgePoint.Identity ")

    for original, placeholder in INTERNALS_PLACEHOLDERS:
        text = text.replace(placeholder, original)

    return text


def rewrite_packages(text: str) -> str:
    for old, new in PACKAGE_REPLACEMENTS:
        text = text.replace(old, new)
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


def rewrite_file(path: Path, namespaces: bool, packages: bool) -> bool:
    original = path.read_text(encoding="utf-8")
    updated = original
    if namespaces:
        updated = rewrite_namespaces(updated)
    if packages:
        updated = rewrite_packages(updated)
    if updated != original:
        path.write_text(updated, encoding="utf-8")
        return True
    return False


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("root", nargs="?", default=".", help="Solution or project directory")
    parser.add_argument("--namespaces", action="store_true", default=False)
    parser.add_argument("--packages", action="store_true", default=False)
    parser.add_argument("--all", action="store_true", help="Rewrite namespaces and package ids")
    args = parser.parse_args()

    namespaces = args.namespaces or args.all
    packages = args.packages or args.all
    if not namespaces and not packages:
        parser.error("Specify --namespaces, --packages, or --all")

    root = Path(args.root).resolve()
    changed = 0

    if namespaces:
        for path in iter_files(root, SOURCE_SUFFIXES):
            if rewrite_file(path, namespaces=True, packages=False):
                print(f"namespace {path.relative_to(root)}")
                changed += 1

    if packages:
        for path in iter_files(root, PROJECT_SUFFIXES):
            if rewrite_file(path, namespaces=False, packages=True):
                print(f"package  {path.relative_to(root)}")
                changed += 1

    print(f"Updated {changed} file(s) under {root}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
