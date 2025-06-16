#!/bin/bash

# Automatically commit and push changes after publishing

set -e

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || { echo "Not a git repository"; exit 1; }
cd "$repo_root"

changes="$(git status --porcelain)"
if [ -z "$changes" ]; then
    echo "No changes to commit."
    exit 0
fi

# Stage all changes
git add -A

# Commit with a generic message
git commit -m "Update build artifacts after publish"

# Push if a remote is configured
if git remote get-url origin > /dev/null 2>&1; then
    git push
else
    echo "No remote configured, skipping push."
fi
