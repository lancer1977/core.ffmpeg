# Deployment Checklist

## Discovery
- [x] Confirm no existing deployment artifacts are present.
- [x] Confirm host project target/runtime expectations.
- [x] Confirm GitHub Actions build prerequisites for Node, .NET, and FFmpeg tooling.

## Documentation
- [x] Create deployment feature docs.
- [x] Add root-level deployment usage instructions.
- [x] Document CI dependency bootstrap expectations.

## Build
- [x] Add host Dockerfile with multi-stage publish.
- [x] Add `.dockerignore` to control build context size.
- [x] Validate `.NET` release build still passes.
- [x] Add GitHub Actions system package install for `ffmpeg`.
- [x] Add explicit `.NET` restore step for CI.
- [x] Pin CI SDK setup to required major versions (`8.x` and `10.x`).

## Runtime
- [x] Add local `docker-compose.yml` service definition.
- [x] Configure host for container HTTP mode when needed.
- [x] Add health checks for orchestrated deployments.

## Validation
- [x] Run TypeScript tests.
- [x] Run `.NET` test executable.
- [x] Run `.NET` solution release build.
- [x] Validate `docker compose up --build` in local development environment.
- [ ] Validate updated GitHub Actions workflow run on next push/PR.

## Follow-up
- [x] Add reverse-proxy deployment profile.
- [x] Add deployment environment variable matrix and secrets guidance.
- [x] Add auth gating for protected operator routes.
- [ ] Add production deployment notes and rollback procedure.
