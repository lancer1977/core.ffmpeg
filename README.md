# core.ffmpeg

Reusable FFmpeg core library for OpenClaw media apps.

This package is a shared TypeScript library for the FFmpeg plumbing used by:

- `chop-it`
- `app-busey-box`
- future Svengoolie/media tooling

## What is implemented

The repo currently contains a working foundation for:

- typed pipeline config objects
- deterministic FFmpeg command construction
- overlay filter-chain assembly
- reloadable `drawtext` runtime-text helpers
- basic `ffprobe` media probing
- process start/stop lifecycle helpers with graceful shutdown
- atomic runtime text utilities
- encoder discovery and NVENC fallback helpers

## Public surface

The package currently exports:

- `buildFfmpegCommand(config)`
- `buildOverlayFilterChain(overlays)`
- `buildDrawTextFilter(runtimeText)`
- `probeMedia(filePath)`
- `startFfmpegProcess(command, args)`
- `atomicWriteText(path, value)`
- `writeRuntimeText(path, value)`
- `ensureDirectory(path)`
- `quoteForFfmpegPath(path)`
- encoder helpers for NVENC discovery/fallback
- the typed config interfaces from `typescript/config`

## Current status

- Build: passing (`typescript` sources)
- Typecheck: passing (`typescript` sources)
- Tests: passing (`node:test` suite in `test/`)
- Repository state: scaffold-plus-core-helpers, not a finished migration
- .NET restores are pinned locally via `nuget.config` to avoid inheriting private feeds

## Planned structure

- `typescript/config` - shared config and option types
- `typescript/command` - FFmpeg command builders
- `typescript/overlay` - overlay and filter-chain helpers
- `typescript/process` - process start/stop lifecycle helpers
- `typescript/probe` - media probing helpers
- `typescript/runtime` - runtime text file helpers and reload patterns
- `typescript/utils` - filesystem and generic utility helpers

## Next steps

1. Validate Docker deployment on the target environment
2. Port the remaining shared logic from `chop-it` and `app-busey-box`
3. Replace duplicated helper code in the consuming apps
4. Add app-level integration samples for TS and .NET consumers

## Build and Deploy

### Local build and test

```bash
npm test
dotnet build dotnet/core.ffmpeg.sln -c Release
dotnet build dotnet/test/Core.FFmpeg.Tests/Core.FFmpeg.Tests.csproj -c Release
dotnet ./dotnet/test/Core.FFmpeg.Tests/bin/Release/net10.0/Core.FFmpeg.Tests.dll
```

### Run host locally without Docker

```bash
DISABLE_HTTPS_REDIRECT=true ASPNETCORE_URLS=http://localhost:8080 \
  dotnet run --project dotnet/Polyhydra.Ffmpeg.Host/Polyhydra.Ffmpeg.Host.csproj
```

### Run host with Docker Compose

```bash
docker compose up --build
```

Host URL: `http://localhost:8080`  
Status endpoint: `http://localhost:8080/api/status`
