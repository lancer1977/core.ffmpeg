# Repo bootstrap checklist

## Goal
Stand up a practical Blazor + MudBlazor host app on top of `core.ffmpeg` for homelab use.

## Phase 1 - Foundation
- [ ] create `ffmpeg.contracts`
- [ ] define job/preset/probe DTOs
- [ ] define core service abstractions
- [ ] implement a basic ffprobe helper
- [ ] implement a basic FFmpeg command builder
- [ ] add a minimal process runner
- [ ] add unit tests for command generation

## Phase 2 - Host app shell
- [ ] create `ffmpeg.host` ASP.NET Core app
- [ ] add MudBlazor
- [ ] wire shared contracts and core library
- [ ] create dashboard page
- [ ] create jobs page
- [ ] create probe page
- [ ] create presets page
- [ ] add system status endpoint

## Phase 3 - Job execution
- [ ] add job creation endpoint
- [ ] add background queue or worker
- [ ] stream progress updates to the UI
- [ ] persist job history
- [ ] add cancel/retry actions

## Phase 4 - Operator tooling
- [ ] add command preview
- [ ] add log viewer
- [ ] add preset editor/clone flow
- [ ] add settings page
- [ ] add file browser or source selector

## Phase 5 - Deployment
- [ ] add Dockerfile
- [ ] add docker-compose
- [ ] add reverse proxy config
- [ ] add auth gating
- [ ] document environment variables

## Phase 6 - Hardening
- [ ] validate allowed source roots
- [ ] restrict risky FFmpeg arguments
- [ ] add health checks
- [ ] add structured logging
- [ ] add cleanup/retention controls

## Recommended first milestone
Deliver a working dashboard + probe page + job creation path before adding durable queueing.
