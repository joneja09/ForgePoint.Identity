#!/usr/bin/env bash
set -euo pipefail

# Rewrite IdentityServer4 package ids and/or namespaces to ForgePoint.Identity.
# Type names such as AddIdentityServer and IdentityServerOptions are not changed.

ROOT="."
EXTRA=()
for arg in "$@"; do
  if [[ "$arg" == --* ]]; then
    EXTRA+=("$arg")
  else
    ROOT="$arg"
  fi
done

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if command -v python3 >/dev/null 2>&1; then
  python3 "$SCRIPT_DIR/rewrite.py" "$ROOT" --all "${EXTRA[@]}"
  exit $?
fi

echo "python3 is required to run this upgrade script." >&2
exit 1
