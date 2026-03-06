#!/usr/bin/env bash
# Bootstrap script for PingPong.App MAUI scaffold
# Run once from the repository root: bash src/bootstrap.sh

set -euo pipefail
REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$REPO_ROOT"

echo "==> Creating feature branch..."
git checkout -b feature/s1-scaffold 2>/dev/null || git checkout feature/s1-scaffold

echo "==> Creating directory structure..."
mkdir -p src/PingPong.App/{Game/Core,Game/Systems,Views,ViewModels,Services}
mkdir -p src/PingPong.App/Platforms/{Android,iOS}
mkdir -p src/PingPong.App/Resources/{AppIcon,Splash,Fonts,Images,Raw,Styles}
mkdir -p src/PingPong.App.Tests

echo "==> Done. Run 'git status' to verify."
