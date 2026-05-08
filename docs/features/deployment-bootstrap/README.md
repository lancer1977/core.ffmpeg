# Deployment Bootstrap

## Summary
Provide a first deployable baseline for the consolidated `.NET` host and validate build/release flow in a container-friendly way, including CI dependency bootstrap for GitHub Actions.

## What this includes
- Multi-stage Docker build for `Polyhydra.Ffmpeg.Host`.
- Local `docker compose` entrypoint for quick startup.
- Minimal runtime environment defaults for container HTTP serving.
- Build-and-run commands documented in the root README.
- GitHub Actions CI dependency bootstrap for required system libraries and SDKs.

## Current status
- Baseline deployment artifacts added.
- CI workflow now installs `ffmpeg` and restores package dependencies explicitly before build.
- Local build/test validation remains required on each change.
- Host and compose healthchecks now hit `/api/health`.
- A production reverse-proxy profile is available via `docker-compose.proxy.yml` and `Caddyfile`.
- Deployment env and secret guidance lives in [environment-variables.md](environment-variables.md).
- Production deployment and rollback notes live in [production-deployment.md](production-deployment.md).

## Next steps
1. Add release validation for the updated GitHub Actions workflow.
